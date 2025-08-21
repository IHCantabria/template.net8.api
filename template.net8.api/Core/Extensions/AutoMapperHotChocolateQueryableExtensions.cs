using System.Linq.Expressions;
using System.Reflection;
using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using HotChocolate.Data.Projections.Expressions;
using HotChocolate.Resolvers;
using template.net8.api.Core.Attributes;
using static HotChocolate.Data.Projections.Expressions.QueryableProjectionProvider;

namespace template.net8.api.Core.Extensions;

[CoreLibrary]
internal static class AutoMapperHotChocolateQueryableExtensions
{
    public static IQueryable<TResult> ProjectTo<TSource, TResult>(this IQueryable<TSource> query,
        IResolverContext context)
    {
        var mapper = context.Service<IMapper>();
        var type = context.Selection.Field.Type;

        context.LocalContextData = context.LocalContextData.SetItem(SkipProjectionKey, true);

        var visitorContext =
            new QueryableProjectionContext(context, context.ObjectType, type.UnwrapRuntimeType(), true);

        QueryableProjectionVisitor.Default.Visit(visitorContext);

        var projection = visitorContext.Project<TResult, object?>();

        List<Expression<Func<TResult, object?>>> membersToExpand = [];

        VisitRoot(projection, membersToExpand);

        return query.UseAsDataSource(mapper.ConfigurationProvider)
            .For(mapper.ConfigurationProvider, membersToExpand.ToArray());
    }

    private static void VisitRoot<TDest>(Expression<Func<TDest, object?>> root,
        List<Expression<Func<TDest, object?>>> expressions)
    {
        var lambdaBody = (UnaryExpression)root.Body;

        var memberInit = (MemberInitExpression)lambdaBody.Operand;

        const int level = 0;

        var parameter = MakeParameter(memberInit.Type, level);

        expressions.AddRange(VisitMemberInit(memberInit, parameter, level).Select(expression =>
            Expression.Lambda<Func<TDest, object?>>(expression, parameter)));
    }

    private static IEnumerable<Expression> VisitMemberInit(MemberInitExpression memberInit,
        Expression path, int level)
    {
        return memberInit.Bindings.Cast<MemberAssignment>()
            .SelectMany(memberAssignment => VisitMemberAssignment(memberAssignment, path, level));
    }

    private static IEnumerable<Expression> VisitMemberAssignment(MemberAssignment memberAssignment, Expression path,
        int level)
    {
        var memberExpression = memberAssignment.Expression;

        var member = (PropertyInfo)memberAssignment.Member;

        switch (memberExpression)
        {
            case MemberExpression:
                yield return VisitPrimitive(member, path);

                break;
            case MemberInitExpression nestedMemberInit:
            {
                foreach (var expression in VisitNestedObject(nestedMemberInit, member, path, level))
                    yield return expression;

                break;
            }
            case MethodCallExpression toArrayCallExpr:
            {
                foreach (var expression in VisitNestedCollection(toArrayCallExpr, member, path, level))
                    yield return expression;

                break;
            }
        }
    }

    private static Expression VisitPrimitive(PropertyInfo member, Expression path)
    {
        Expression memberAccess = Expression.MakeMemberAccess(path, member);

        if (member.PropertyType.IsValueType && Nullable.GetUnderlyingType(member.PropertyType) == null)
            memberAccess = Expression.Convert(memberAccess, typeof(object));

        return memberAccess;
    }

    private static IEnumerable<Expression> VisitNestedObject(MemberInitExpression nestedMemberInit, PropertyInfo member,
        Expression path, int level)
    {
        var nestedPath = Expression.MakeMemberAccess(path, member);

        foreach (var expression in VisitMemberInit(nestedMemberInit, nestedPath, level)) yield return expression;
    }

    private static IEnumerable<Expression> VisitNestedCollection(
        MethodCallExpression toArrayCall, PropertyInfo member, Expression path, int level)
    {
        if (!TryGetSelectCall(toArrayCall, out _, out var selectLambda))
            yield break;

        var nestedPath = Expression.MakeMemberAccess(path, member);

        level++;

        var parameter = MakeParameter(((MemberInitExpression)selectLambda!.Body).Type, level);

        var selectMethod = GetSelectMethod(parameter);

        foreach (var expression in VisitMemberInit((MemberInitExpression)selectLambda.Body, parameter, level))
        {
            var lambda = Expression.Lambda(expression, parameter);
            yield return Expression.Call(null, selectMethod, nestedPath, lambda);
        }
    }

    // Helper method to extract the Select method call and its lambda expression
    private static bool TryGetSelectCall(
        MethodCallExpression methodCall, out MethodCallExpression? selectCall, out LambdaExpression? selectLambda)
    {
        selectCall = null;
        selectLambda = null;

        while (methodCall.Arguments[0] is MethodCallExpression innerCall) methodCall = innerCall;

        if (methodCall.Method.Name != nameof(Enumerable.Select))
            return false;

        selectCall = methodCall;
        selectLambda = methodCall.Arguments[1] as LambdaExpression;
        return selectLambda != null;
    }

// Method to get the correct generic Select method
    private static MethodInfo GetSelectMethod(ParameterExpression parameter)
    {
        var allMethods = typeof(Enumerable).GetMethods(BindingFlags.Static | BindingFlags.Public);
        var selectMethods = allMethods.Where(mi => mi.Name == nameof(Enumerable.Select));
        var targetMethod = selectMethods.Single(IsCorrectSelectMethod);

        return targetMethod.MakeGenericMethod(parameter.Type, typeof(object));
    }

    private static ParameterExpression MakeParameter(Type type, int lambdaLevel)
    {
        return Expression.Parameter(type, "p_" + lambdaLevel);
    }

    // Method to determine if a given method is the correct Select overload
    private static bool IsCorrectSelectMethod(MethodInfo mi)
    {
        var parameters = mi.GetParameters();

        // Ensure method has the expected number of parameters
        if (parameters.Length < 2)
            return false;

        // Check if the second parameter has two generic arguments (Func<TSource, TResult>)
        var secondParameterType = parameters[1].ParameterType;
        var genericArguments = secondParameterType.GetGenericArguments();

        return genericArguments.Length == 2;
    }
}