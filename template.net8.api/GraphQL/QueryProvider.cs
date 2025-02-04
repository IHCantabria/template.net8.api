using JetBrains.Annotations;
using MediatR;
using template.net8.api.Domain.Persistence.Models;
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
    public Task<IQueryable<Dummy>> DummiesAsync([Service] IMediator mediatr)
    {
        ArgumentNullException.ThrowIfNull(mediatr);
        return mediatr.Send(new GraphQLQueryGetDummies());
    }
}