using Microsoft.EntityFrameworkCore;
using template.net8.api.Core.Attributes;

namespace template.net8.api.Domain.Base;

/// <summary>
///     Db Repository Base
/// </summary>
[CoreLibrary]
public class DbRepositoryScopedDbContextBase : RepositoryBase
{
    internal readonly DbContext Context;

    /// <summary>
    ///     Db Repository Base Constructor
    /// </summary>
    /// <param name="context"></param>
    /// <param name="logger"></param>
    /// <exception cref="ArgumentNullException">Condition.</exception>
    protected DbRepositoryScopedDbContextBase(DbContext context, ILogger<DbRepositoryScopedDbContextBase> logger) :
        base(logger)
    {
        Context = context ?? throw new ArgumentNullException(nameof(context));
    }
}

/// <summary>
///     Db Repository Base
/// </summary>
[CoreLibrary]
public class DbRepositoryTransientDbContextBase : RepositoryBase
{
    /// <summary>
    ///     Db Repository Base Constructor
    /// </summary>
    /// <param name="logger"></param>
    /// <exception cref="ArgumentNullException">Condition.</exception>
    protected DbRepositoryTransientDbContextBase(ILogger<DbRepositoryTransientDbContextBase> logger) : base(logger)
    {
    }
}