using System.Diagnostics.CodeAnalysis;

namespace template.net8.api.Core.Exceptions;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[SuppressMessage("Design",
    "CA1515:Consider making public types internal",
    Justification =
        "Exception types are part of the public contract and must remain public to be consumed by external callers.")]
public class CoreException : Exception
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    protected CoreException()
    {
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    internal CoreException(string message) : base(message)
    {
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    protected CoreException(string message, Exception innerException) : base(message, innerException)
    {
    }
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[SuppressMessage("Design",
    "CA1515:Consider making public types internal",
    Justification =
        "Exception types are part of the public contract and must remain public to be consumed by external callers.")]
[SuppressMessage("ReSharper",
    "MemberCanBeInternal",
    Justification =
        "Exception types are part of the public contract and must remain public to be consumed by external callers.")]
[SuppressMessage("ReSharper",
    "ClassNeverInstantiated.Global",
    Justification =
        "Exception type is part of the public API; instantiation depends on usage by consumers or higher-level layers.")]
[SuppressMessage(
    "ReSharper",
    "UnusedMember.Global",
    Justification = "Standard exception constructor required by CA1032 and RCS1194.")]
public sealed class BadRequestException : CoreException
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public BadRequestException()
    {
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public BadRequestException(string message) : base(message)
    {
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public BadRequestException(string message, Exception innerException) : base(message, innerException)
    {
    }
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[SuppressMessage("Design",
    "CA1515:Consider making public types internal",
    Justification =
        "Exception types are part of the public contract and must remain public to be consumed by external callers.")]
[SuppressMessage("ReSharper",
    "MemberCanBeInternal",
    Justification =
        "Exception types are part of the public contract and must remain public to be consumed by external callers.")]
[SuppressMessage("ReSharper",
    "ClassNeverInstantiated.Global",
    Justification =
        "Exception type is part of the public API; instantiation depends on usage by consumers or higher-level layers.")]
[SuppressMessage(
    "ReSharper",
    "UnusedMember.Global",
    Justification = "Standard exception constructor required by CA1032 and RCS1194.")]
public sealed class UnauthorizedException : CoreException
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public UnauthorizedException()
    {
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public UnauthorizedException(string message) : base(message)
    {
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public UnauthorizedException(string message, Exception innerException) : base(message, innerException)
    {
    }
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[SuppressMessage("Design",
    "CA1515:Consider making public types internal",
    Justification =
        "Exception types are part of the public contract and must remain public to be consumed by external callers.")]
[SuppressMessage("ReSharper",
    "MemberCanBeInternal",
    Justification =
        "Exception types are part of the public contract and must remain public to be consumed by external callers.")]
[SuppressMessage("ReSharper",
    "ClassNeverInstantiated.Global",
    Justification =
        "Exception type is part of the public API; instantiation depends on usage by consumers or higher-level layers.")]
[SuppressMessage(
    "ReSharper",
    "UnusedMember.Global",
    Justification = "Standard exception constructor required by CA1032 and RCS1194.")]
public sealed class ForbiddenException : CoreException
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public ForbiddenException()
    {
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public ForbiddenException(string message) : base(message)
    {
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public ForbiddenException(string message, Exception innerException) : base(message, innerException)
    {
    }
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[SuppressMessage("Design",
    "CA1515:Consider making public types internal",
    Justification =
        "Exception types are part of the public contract and must remain public to be consumed by external callers.")]
[SuppressMessage("ReSharper",
    "MemberCanBeInternal",
    Justification =
        "Exception types are part of the public contract and must remain public to be consumed by external callers.")]
[SuppressMessage("ReSharper",
    "ClassNeverInstantiated.Global",
    Justification =
        "Exception type is part of the public API; instantiation depends on usage by consumers or higher-level layers.")]
[SuppressMessage(
    "ReSharper",
    "UnusedMember.Global",
    Justification = "Standard exception constructor required by CA1032 and RCS1194.")]
public sealed class NotFoundException : CoreException
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public NotFoundException()
    {
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public NotFoundException(string message) : base(message)
    {
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public NotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[SuppressMessage("Design",
    "CA1515:Consider making public types internal",
    Justification =
        "Exception types are part of the public contract and must remain public to be consumed by external callers.")]
[SuppressMessage("ReSharper",
    "MemberCanBeInternal",
    Justification =
        "Exception types are part of the public contract and must remain public to be consumed by external callers.")]
[SuppressMessage("ReSharper",
    "ClassNeverInstantiated.Global",
    Justification =
        "Exception type is part of the public API; instantiation depends on usage by consumers or higher-level layers.")]
[SuppressMessage(
    "ReSharper",
    "UnusedMember.Global",
    Justification = "Standard exception constructor required by CA1032 and RCS1194.")]
public sealed class RequestTimeoutException : CoreException
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public RequestTimeoutException()
    {
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public RequestTimeoutException(string message) : base(message)
    {
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public RequestTimeoutException(string message, Exception innerException) : base(message, innerException)
    {
    }
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[SuppressMessage("Design",
    "CA1515:Consider making public types internal",
    Justification =
        "Exception types are part of the public contract and must remain public to be consumed by external callers.")]
[SuppressMessage("ReSharper",
    "MemberCanBeInternal",
    Justification =
        "Exception types are part of the public contract and must remain public to be consumed by external callers.")]
[SuppressMessage("ReSharper",
    "ClassNeverInstantiated.Global",
    Justification =
        "Exception type is part of the public API; instantiation depends on usage by consumers or higher-level layers.")]
[SuppressMessage(
    "ReSharper",
    "UnusedMember.Global",
    Justification = "Standard exception constructor required by CA1032 and RCS1194.")]
public sealed class ConflictException : CoreException
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public ConflictException()
    {
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public ConflictException(string message) : base(message)
    {
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public ConflictException(string message, Exception innerException) : base(message, innerException)
    {
    }
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[SuppressMessage("Design",
    "CA1515:Consider making public types internal",
    Justification =
        "Exception types are part of the public contract and must remain public to be consumed by external callers.")]
[SuppressMessage("ReSharper",
    "MemberCanBeInternal",
    Justification =
        "Exception types are part of the public contract and must remain public to be consumed by external callers.")]
[SuppressMessage("ReSharper",
    "ClassNeverInstantiated.Global",
    Justification =
        "Exception type is part of the public API; instantiation depends on usage by consumers or higher-level layers.")]
[SuppressMessage(
    "ReSharper",
    "UnusedMember.Global",
    Justification = "Standard exception constructor required by CA1032 and RCS1194.")]
public sealed class GoneException : CoreException
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public GoneException()
    {
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public GoneException(string message) : base(message)
    {
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public GoneException(string message, Exception innerException) : base(message, innerException)
    {
    }
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[SuppressMessage("Design",
    "CA1515:Consider making public types internal",
    Justification =
        "Exception types are part of the public contract and must remain public to be consumed by external callers.")]
[SuppressMessage("ReSharper",
    "MemberCanBeInternal",
    Justification =
        "Exception types are part of the public contract and must remain public to be consumed by external callers.")]
[SuppressMessage("ReSharper",
    "ClassNeverInstantiated.Global",
    Justification =
        "Exception type is part of the public API; instantiation depends on usage by consumers or higher-level layers.")]
[SuppressMessage(
    "ReSharper",
    "UnusedMember.Global",
    Justification = "Standard exception constructor required by CA1032 and RCS1194.")]
public sealed class UnprocessableEntityException : CoreException
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public UnprocessableEntityException()
    {
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public UnprocessableEntityException(string message) : base(message)
    {
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public UnprocessableEntityException(string message, Exception innerException) : base(message, innerException)
    {
    }
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[SuppressMessage("Design",
    "CA1515:Consider making public types internal",
    Justification =
        "Exception types are part of the public contract and must remain public to be consumed by external callers.")]
[SuppressMessage("ReSharper",
    "MemberCanBeInternal",
    Justification =
        "Exception types are part of the public contract and must remain public to be consumed by external callers.")]
[SuppressMessage("ReSharper",
    "ClassNeverInstantiated.Global",
    Justification =
        "Exception type is part of the public API; instantiation depends on usage by consumers or higher-level layers.")]
[SuppressMessage(
    "ReSharper",
    "UnusedMember.Global",
    Justification = "Standard exception constructor required by CA1032 and RCS1194.")]
public sealed class InternalServerErrorException : CoreException
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public InternalServerErrorException()
    {
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public InternalServerErrorException(string message) : base(message)
    {
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public InternalServerErrorException(string message, Exception innerException) : base(message, innerException)
    {
    }
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[SuppressMessage("Design",
    "CA1515:Consider making public types internal",
    Justification =
        "Exception types are part of the public contract and must remain public to be consumed by external callers.")]
[SuppressMessage("ReSharper",
    "MemberCanBeInternal",
    Justification =
        "Exception types are part of the public contract and must remain public to be consumed by external callers.")]
[SuppressMessage("ReSharper",
    "ClassNeverInstantiated.Global",
    Justification =
        "Exception type is part of the public API; instantiation depends on usage by consumers or higher-level layers.")]
[SuppressMessage(
    "ReSharper",
    "UnusedMember.Global",
    Justification = "Standard exception constructor required by CA1032 and RCS1194.")]
public sealed class NotImplementedException : CoreException
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public NotImplementedException()
    {
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public NotImplementedException(string message) : base(message)
    {
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public NotImplementedException(string message, Exception innerException) : base(
        message, innerException)
    {
    }
}