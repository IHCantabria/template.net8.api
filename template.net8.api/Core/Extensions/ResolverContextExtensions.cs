using HotChocolate.Language;
using HotChocolate.Resolvers;
using template.net8.api.Core.Attributes;

namespace template.net8.api.Core.Extensions;

[CoreLibrary]
internal static class ResolverContextExtensions
{
    /// <summary>
    ///     Gets the names of the fields selected in the GraphQL query.
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    internal static Dictionary<string, HashSet<string>> GetSelectedFieldsTree(this IResolverContext context)
    {
        var result = new Dictionary<string, HashSet<string>>(StringComparer.OrdinalIgnoreCase);
        VisitSelection(context.Selection.SyntaxNode, result, string.Empty);
        return result;
    }

    private static void VisitSelection(FieldNode fieldNode, Dictionary<string, HashSet<string>> tree, string parentPath)
    {
        var currentPath = string.IsNullOrEmpty(parentPath)
            ? fieldNode.Name.Value
            : $"{parentPath}.{fieldNode.Name.Value}";

        if (fieldNode.SelectionSet != null)
        {
            foreach (var selection in fieldNode.SelectionSet.Selections.OfType<FieldNode>())
                VisitSelection(selection, tree, currentPath);
        }
        else
        {
            if (!tree.ContainsKey(parentPath))
                tree[parentPath] = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            tree[parentPath].Add(fieldNode.Name.Value);
        }
    }
}