using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using template.net8.api.Core.Base;
using template.net8.api.Core.Interfaces;
using template.net8.api.Persistence.Models;

namespace template.net8.api.Domain.Specifications;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal sealed class UserEmailVerification : VerificationBase<User>
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal UserEmailVerification(string email)
    {
        AddFilter(u => u.Email == email);
    }
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal sealed class UserDisabledVerification : VerificationBase<User>
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal UserDisabledVerification(Guid key)
    {
        AddFilter(u => u.Uuid == key);
        AddFilter(static u => u.IsDisabled);
    }
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal sealed class UserEnabledVerification : VerificationBase<User>
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal UserEnabledVerification(Guid key)
    {
        AddFilter(u => u.Uuid == key);
        AddFilter(static u => !u.IsDisabled);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal UserEnabledVerification(string email)
    {
        AddFilter(u => u.Email == email);
        AddFilter(static u => !u.IsDisabled);
    }
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal sealed class UserReadByKeySpecification : SpecificationBase<User>
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal UserReadByKeySpecification(Guid key)
    {
        AddFilter(u => u.Uuid == key);
        AddOrderBy(static u => u.InsertDatetime ?? DateTime.SpecifyKind(DateTime.MinValue, DateTimeKind.Utc),
            OrderByType.Desc);
        SetQueryTrackStrategy(QueryTrackingBehavior.NoTracking);
        SetQuerySplitStrategy(QuerySplittingBehavior.SingleQuery);
    }
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal sealed class UserWriteByKeySpecification : SpecificationBase<User>
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal UserWriteByKeySpecification(Guid key)
    {
        AddFilter(u => u.Uuid == key);
        AddOrderBy(static u => u.InsertDatetime ?? DateTime.SpecifyKind(DateTime.MinValue, DateTimeKind.Utc),
            OrderByType.Desc);
        SetQueryTrackStrategy(QueryTrackingBehavior.TrackAll);
        SetQuerySplitStrategy(QuerySplittingBehavior.SingleQuery);
    }
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal sealed class UserWriteDeleteByKeySpecification : SpecificationBase<User>
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumentedOptional",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
    internal UserWriteDeleteByKeySpecification(Guid key)
    {
        AddFilter(u => u.Uuid == key);
        AddInclude(static q => q.Include(static u => u.Claims));
        AddOrderBy(static u => u.InsertDatetime ?? DateTime.SpecifyKind(DateTime.MinValue, DateTimeKind.Utc),
            OrderByType.Desc);
        SetQueryTrackStrategy(QueryTrackingBehavior.TrackAll);
        SetQuerySplitStrategy(QuerySplittingBehavior.SingleQuery);
    }
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal sealed class UserReadByEmailSpecification : SpecificationBase<User>
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal UserReadByEmailSpecification(string email)
    {
        AddFilter(u => u.Email == email);
        AddOrderBy(static u => u.InsertDatetime ?? DateTime.SpecifyKind(DateTime.MinValue, DateTimeKind.Utc),
            OrderByType.Desc);
        SetQueryTrackStrategy(QueryTrackingBehavior.NoTracking);
        SetQuerySplitStrategy(QuerySplittingBehavior.SingleQuery);
    }
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal sealed class UsersReadSpecification : SpecificationBase<User>
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal UsersReadSpecification()
    {
        AddOrderBy(static u => u.InsertDatetime ?? DateTime.SpecifyKind(DateTime.MinValue, DateTimeKind.Utc),
            OrderByType.Desc);
        SetQueryTrackStrategy(QueryTrackingBehavior.NoTracking);
        SetQuerySplitStrategy(QuerySplittingBehavior.SingleQuery);
    }
}