using System.Numerics;
using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using template.net8.api.Domain.Persistence.Context;
using template.net8.api.Domain.Persistence.Models;

namespace template.net8.api.Features.GraphQL;

/// <summary>
///     GraphQL Query Get Dummies CQRS
/// </summary>
public sealed record GraphQLQueryGetDummies : IRequest<IQueryable<Dummy>>,
    IEqualityOperators<GraphQLQueryGetDummies, GraphQLQueryGetDummies, bool>;

[UsedImplicitly]
[MustDisposeResource]
internal sealed class GraphQLGetDummiesHandlerQuery(IDbContextFactory<ProjectDbContext> context)
    : IRequestHandler<GraphQLQueryGetDummies, IQueryable<Dummy>>, IAsyncDisposable
{
    private readonly ProjectDbContext _context =
        context.CreateDbContext() ?? throw new ArgumentNullException(nameof(context));

    public ValueTask DisposeAsync()
    {
        return _context.DisposeAsync();
    }

    /// <summary>
    ///     Handle the Get Dummies Query request
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>source</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    public Task<IQueryable<Dummy>> Handle(GraphQLQueryGetDummies request,
        CancellationToken cancellationToken)
    {
        return Task.FromResult(_context.Dummies.AsNoTracking().AsSplitQuery());
    }
}