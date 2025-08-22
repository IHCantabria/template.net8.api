using HotChocolate.Resolvers;
using JetBrains.Annotations;
using MediatR;
using template.net8.api.Domain.DTOs;
using template.net8.api.Features.GraphQL;

namespace template.net8.api.GraphQL;

/// <summary>
///     GraphQL Query Provider
/// </summary>
[UsedImplicitly]
[GraphQLName("Template")]
[GraphQLDescription("Template Dummy")]
public sealed class QueryProvider
{
    /// <summary>
    ///     Campaigns
    /// </summary>
    /// <param name="mediatr"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>argument</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    [UsedImplicitly]
    [GraphQLName("dummies")]
    [GraphQLDescription("Dummies")]
    public Task<IQueryable<DummyDto>> DummiesAsync([Service] IMediator mediatr, IResolverContext context)
    {
        ArgumentNullException.ThrowIfNull(mediatr);
        ArgumentNullException.ThrowIfNull(context);
        return mediatr.Send(new GraphQLQueryGetDummies(context));
    }
}