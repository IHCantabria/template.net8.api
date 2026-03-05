using System.Diagnostics.CodeAnalysis;
using HotChocolate.Resolvers;
using JetBrains.Annotations;
using MediatR;
using template.net8.api.Domain.DTOs;
using template.net8.api.Features.GraphQL;

namespace template.net8.api.GraphQL;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[SuppressMessage(
    "ReSharper",
    "CA1822:Mark members as static",
    Justification =
        "Resolver methods are intentionally instance members to align with HotChocolate query type registration.")]
[GraphQLName("Users")]
[GraphQLDescription("GraphQL Users")]
internal sealed class QueryProvider
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="mediatr" /> is <see langword="null" />.
    ///     <paramref name="context" /> is <see langword="null" />.
    /// </exception>
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    [GraphQLName("users")]
    [GraphQLDescription("Users")]
    [UsedImplicitly]
    public Task<IQueryable<UserDto>> UsersAsync([Service] IMediator mediatr, IResolverContext context)
    {
        ArgumentNullException.ThrowIfNull(mediatr);
        ArgumentNullException.ThrowIfNull(context);
        return mediatr.Send(new GraphQLQueryGetUsers(context));
    }
}