using System.Numerics;
using HotChocolate.Resolvers;
using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using template.net8.api.Core.Extensions;
using template.net8.api.Domain.DTOs;
using template.net8.api.Domain.Persistence.Context;
using template.net8.api.Domain.Persistence.Models;

namespace template.net8.api.Features.GraphQL;

/// <summary>
///     GraphQL Query Get Dummies CQRS
/// </summary>
public sealed record GraphQLQueryGetDummies(IResolverContext Context) : IRequest<IQueryable<DummyDto>>,
    IEqualityOperators<GraphQLQueryGetDummies, GraphQLQueryGetDummies, bool>;

[UsedImplicitly]
[MustDisposeResource]
internal sealed class GraphQLGetDummiesHandlerQuery(IDbContextFactory<ProjectDbContext> context)
    : IRequestHandler<GraphQLQueryGetDummies, IQueryable<DummyDto>>, IAsyncDisposable
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
    /// <exception cref="ArgumentException">
    ///     <paramref>
    ///         <name>member</name>
    ///     </paramref>
    ///     does not represent a field or property.
    ///     -or-
    ///     The property represented by
    ///     <paramref>
    ///         <name>member</name>
    ///     </paramref>
    ///     does not have a <see langword="set" /> accessor.
    ///     -or-
    ///     <paramref>
    ///         <name>expression</name>
    ///     </paramref>
    ///     .Type is not assignable to the type of the field or property that
    ///     <paramref>
    ///         <name>member</name>
    ///     </paramref>
    ///     represents.
    /// </exception>
    public Task<IQueryable<DummyDto>> Handle(GraphQLQueryGetDummies request,
        CancellationToken cancellationToken)
    {
        return Task.FromResult(_context.Dummies.AsNoTracking().AsSplitQuery()
            .ApplyProjection(DummyProjections.Projection, request.Context));
    }
}