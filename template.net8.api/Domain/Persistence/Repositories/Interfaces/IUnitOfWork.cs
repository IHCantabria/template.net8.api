﻿using LanguageExt.Common;
using Microsoft.EntityFrameworkCore;
using template.net8.api.Core.Attributes;

namespace template.net8.api.Domain.Persistence.Repositories.Interfaces;

/// <summary>
///     Interface for the Unit of Work pattern.
/// </summary>
/// <typeparam name="TDbContext"></typeparam>
[CoreLibrary]
public interface IUnitOfWork<TDbContext> where TDbContext : DbContext
{
    /// <summary>
    ///     Complete the pending changes in the repository.
    /// </summary>
    /// <returns></returns>
    Task<Result<bool>> CompleteAsync(CancellationToken cancellationToken);
}