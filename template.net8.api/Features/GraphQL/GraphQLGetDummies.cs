using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using HotChocolate.Resolvers;
using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using template.net8.api.Core.Extensions;
using template.net8.api.Domain.DTOs;
using template.net8.api.Persistence.Context;
using template.net8.api.Persistence.Models;

namespace template.net8.api.Features.GraphQL;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[SuppressMessage(
    "Design",
    "CA1515:Consider making public types internal",
    Justification =
        "Public visibility is required because this request is part of the application messaging contract (MediatR).")]
[SuppressMessage(
    "Design",
    "MemberCanBeInternal",
    Justification =
        "Public visibility is required because this request is part of the application messaging contract (MediatR).")]
public sealed record GraphQLQueryGetUsers(IResolverContext Context) : IRequest<IQueryable<UserDto>>,
    IEqualityOperators<GraphQLQueryGetUsers, GraphQLQueryGetUsers, bool>;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[MustDisposeResource]
internal sealed class GraphQLGetUsersHandlerQuery(IDbContextFactory<AppDbContext> context)
    : IRequestHandler<GraphQLQueryGetUsers, IQueryable<UserDto>>, IAsyncDisposable
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private readonly AppDbContext _context =
        context.CreateDbContext() ?? throw new ArgumentNullException(nameof(context));

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public ValueTask DisposeAsync()
    {
        return _context.DisposeAsync();
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumentedOptional",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
    public Task<IQueryable<UserDto>> Handle(GraphQLQueryGetUsers request,
        CancellationToken cancellationToken)
    {
        return Task.FromResult(_context.Users.AsNoTracking().AsSplitQuery()
            .ApplyProjection(UserProjections.UserProjection, request.Context));
    }
}