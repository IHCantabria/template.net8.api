﻿using System.Numerics;
using System.Text.Json.Serialization;
using template.net8.api.Core.Attributes;
using template.net8.api.Core.Interfaces;

namespace template.net8.api.Core.Contracts;

/// <summary>
///     Problem Details Base Resource
/// </summary>
[CoreLibrary]
public record ProblemDetailsBaseResource : IPublicApiContract,
    IEqualityOperators<ProblemDetailsBaseResource, ProblemDetailsBaseResource, bool>
{
    /// <summary>
    ///     Problem Details Base Resource
    /// </summary>
    protected internal ProblemDetailsBaseResource()
    {
    }

    /// <summary>
    ///     A short, human-readable summary of the problem type. It SHOULD NOT change from occurrence to occurrence
    ///     of the problem, except for purposes of localization(e.g., using proactive content negotiation;
    ///     see[RFC9110], Section 12).
    /// </summary>
    [DefaultValue("A short, human-readable summary of the problem type.")]
    [JsonPropertyOrder(-6)]
    [JsonRequired]
    public required string Title { get; init; }

    /// <summary>
    ///     A human-readable explanation specific to this occurrence of the problem.
    /// </summary>
    [DefaultValue("A human-readable explanation specific to this occurrence of the problem")]
    [JsonPropertyOrder(-4)]
    [JsonRequired]
    public string? Detail { get; init; }

    /// <summary>
    ///     A URI reference that identifies the specific occurrence of the problem. It may or may not yield further information
    ///     if dereferenced.
    /// </summary>
    [DefaultValue("A URI reference that identifies the specific occurrence of the problem.")]
    [JsonPropertyOrder(-3)]
    [JsonRequired]
    public required string Instance { get; init; }

    /// <summary>
    ///     The HTTP VERB associated with URI reference that identifies the specific occurrence of the problem. It may or may
    ///     not yield further information
    ///     if dereferenced.
    /// </summary>
    [DefaultValue("The HTTP VERB associated with URI reference that identifies the specific occurrence of the problem")]
    [JsonPropertyOrder(-2)]
    [JsonRequired]
    public required string Method { get; init; }

    /// <summary>
    ///     A unique reference that identifies the specific occurrence of the problem for a moment and consumer intance. It may
    ///     or may not yield further information
    ///     if dereferenced.
    /// </summary>
    [DefaultValue(
        "A unique reference that identifies the specific occurrence of the problem for a moment and consumer.")]
    [JsonPropertyOrder(-1)]
    [JsonRequired]
    public required string RequestId { get; init; }
}

/// <summary>
///     Bad Request Problem Details Resource
/// </summary>
[CoreLibrary]
public sealed record BadRequestProblemDetailsResource : ProblemDetailsBaseResource,
    IEqualityOperators<BadRequestProblemDetailsResource, BadRequestProblemDetailsResource, bool>
{
    /// <summary>
    ///     A URI reference [RFC8820] that identifies the problem type. This specification encourages that, when
    ///     dereferenced, it provide human-readable documentation for the problem type
    ///     (e.g., using HTML [W3C.REC-html5-20141028]). When this member is not present, its value is assumed to be
    ///     "about:blank".
    /// </summary>
    [DefaultValue("https://tools.ietf.org/html/rfc9110#name-400-bad-request")]
    [JsonPropertyOrder(-7)]
    [JsonRequired]
    public required string Type { get; init; }

    /// <summary>
    ///     The HTTP status code([RFC9110], Section 15) generated by the origin server for this occurrence of the problem.
    /// </summary>
    [DefaultValue(StatusCodes.Status400BadRequest)]
    [JsonPropertyOrder(-6)]
    [JsonRequired]
    public required short Status { get; init; }
}

/// <summary>
///     Unauthorized Problem Details Resource
/// </summary>
[CoreLibrary]
public sealed record UnauthorizedProblemDetailsResource : ProblemDetailsBaseResource,
    IEqualityOperators<UnauthorizedProblemDetailsResource, UnauthorizedProblemDetailsResource, bool>
{
    /// <summary>
    ///     A URI reference [RFC8820] that identifies the problem type. This specification encourages that, when
    ///     dereferenced, it provide human-readable documentation for the problem type
    ///     (e.g., using HTML [W3C.REC-html5-20141028]). When this member is not present, its value is assumed to be
    ///     "about:blank".
    /// </summary>
    [DefaultValue("https://tools.ietf.org/html/rfc9110#name-401-unauthorized")]
    [JsonPropertyOrder(-7)]
    [JsonRequired]
    public required string Type { get; init; }

    /// <summary>
    ///     The HTTP status code([RFC9110], Section 15) generated by the origin server for this occurrence of the problem.
    /// </summary>
    [DefaultValue(StatusCodes.Status401Unauthorized)]
    [JsonPropertyOrder(-6)]
    [JsonRequired]
    public required short Status { get; init; }
}

/// <summary>
///     Forbidden Problem Details Resource
/// </summary>
[CoreLibrary]
public sealed record ForbiddenProblemDetailsResource : ProblemDetailsBaseResource,
    IEqualityOperators<ForbiddenProblemDetailsResource, ForbiddenProblemDetailsResource, bool>
{
    /// <summary>
    ///     A URI reference [RFC8820] that identifies the problem type. This specification encourages that, when
    ///     dereferenced, it provide human-readable documentation for the problem type
    ///     (e.g., using HTML [W3C.REC-html5-20141028]). When this member is not present, its value is assumed to be
    ///     "about:blank".
    /// </summary>
    [DefaultValue("https://tools.ietf.org/html/rfc9110#name-403-forbidden")]
    [JsonPropertyOrder(-7)]
    [JsonRequired]
    public required string Type { get; init; }

    /// <summary>
    ///     The HTTP status code([RFC9110], Section 15) generated by the origin server for this occurrence of the problem.
    /// </summary>
    [DefaultValue(StatusCodes.Status403Forbidden)]
    [JsonPropertyOrder(-6)]
    [JsonRequired]
    public required short Status { get; init; }
}

/// <summary>
///     NotFound Problem Details Resource
/// </summary>
[CoreLibrary]
public sealed record NotFoundProblemDetailsResource : ProblemDetailsBaseResource,
    IEqualityOperators<NotFoundProblemDetailsResource, NotFoundProblemDetailsResource, bool>
{
    /// <summary>
    ///     A URI reference [RFC8820] that identifies the problem type. This specification encourages that, when
    ///     dereferenced, it provide human-readable documentation for the problem type
    ///     (e.g., using HTML [W3C.REC-html5-20141028]). When this member is not present, its value is assumed to be
    ///     "about:blank".
    /// </summary>
    [DefaultValue("https://tools.ietf.org/html/rfc9110#name-404-not-found")]
    [JsonPropertyOrder(-7)]
    [JsonRequired]
    public required string Type { get; init; }

    /// <summary>
    ///     The HTTP status code([RFC9110], Section 15) generated by the origin server for this occurrence of the problem.
    /// </summary>
    [DefaultValue(StatusCodes.Status404NotFound)]
    [JsonPropertyOrder(-6)]
    [JsonRequired]
    public required short Status { get; init; }
}

/// <summary>
///     Request Timeout Problem Details Resource
/// </summary>
[CoreLibrary]
public sealed record RequestTimeoutProblemDetailsResource : ProblemDetailsBaseResource,
    IEqualityOperators<RequestTimeoutProblemDetailsResource, RequestTimeoutProblemDetailsResource, bool>
{
    /// <summary>
    ///     A URI reference [RFC8820] that identifies the problem type. This specification encourages that, when
    ///     dereferenced, it provide human-readable documentation for the problem type
    ///     (e.g., using HTML [W3C.REC-html5-20141028]). When this member is not present, its value is assumed to be
    ///     "about:blank".
    /// </summary>
    [DefaultValue("https://tools.ietf.org/html/rfc9110#name-408-request-timeout")]
    [JsonPropertyOrder(-7)]
    [JsonRequired]
    public required string Type { get; init; }

    /// <summary>
    ///     The HTTP status code([RFC9110], Section 15) generated by the origin server for this occurrence of the problem.
    /// </summary>
    [DefaultValue(StatusCodes.Status408RequestTimeout)]
    [JsonPropertyOrder(-6)]
    [JsonRequired]
    public required short Status { get; init; }
}

/// <summary>
///     Conflict Problem Details Resource
/// </summary>
[CoreLibrary]
public sealed record ConflictProblemDetailsResource : ProblemDetailsBaseResource,
    IEqualityOperators<ConflictProblemDetailsResource, ConflictProblemDetailsResource, bool>
{
    /// <summary>
    ///     A URI reference [RFC8820] that identifies the problem type. This specification encourages that, when
    ///     dereferenced, it provide human-readable documentation for the problem type
    ///     (e.g., using HTML [W3C.REC-html5-20141028]). When this member is not present, its value is assumed to be
    ///     "about:blank".
    /// </summary>
    [DefaultValue("https://tools.ietf.org/html/rfc9110#name-409-conflict")]
    [JsonPropertyOrder(-7)]
    [JsonRequired]
    public required string Type { get; init; }

    /// <summary>
    ///     The HTTP status code([RFC9110], Section 15) generated by the origin server for this occurrence of the problem.
    /// </summary>
    [DefaultValue(StatusCodes.Status409Conflict)]
    [JsonPropertyOrder(-6)]
    [JsonRequired]
    public required short Status { get; init; }
}

/// <summary>
///     Gone Problem Details Resource
/// </summary>
[CoreLibrary]
public sealed record GoneProblemDetailsResource : ProblemDetailsBaseResource,
    IEqualityOperators<GoneProblemDetailsResource, GoneProblemDetailsResource, bool>
{
    /// <summary>
    ///     A URI reference [RFC8820] that identifies the problem type. This specification encourages that, when
    ///     dereferenced, it provide human-readable documentation for the problem type
    ///     (e.g., using HTML [W3C.REC-html5-20141028]). When this member is not present, its value is assumed to be
    ///     "about:blank".
    /// </summary>
    [DefaultValue("https://tools.ietf.org/html/rfc9110#name-410-gone")]
    [JsonPropertyOrder(-7)]
    [JsonRequired]
    public required string Type { get; init; }

    /// <summary>
    ///     The HTTP status code([RFC9110], Section 15) generated by the origin server for this occurrence of the problem.
    /// </summary>
    [DefaultValue(StatusCodes.Status410Gone)]
    [JsonPropertyOrder(-6)]
    [JsonRequired]
    public required short Status { get; init; }
}

/// <summary>
///     Unprocessable Entity Problem Details Resource
/// </summary>
[CoreLibrary]
public sealed record UnprocessableEntityProblemDetailsResource : ProblemDetailsBaseResource,
    IEqualityOperators<UnprocessableEntityProblemDetailsResource, UnprocessableEntityProblemDetailsResource, bool>
{
    /// <summary>
    ///     A URI reference [RFC8820] that identifies the problem type. This specification encourages that, when
    ///     dereferenced, it provide human-readable documentation for the problem type
    ///     (e.g., using HTML [W3C.REC-html5-20141028]). When this member is not present, its value is assumed to be
    ///     "about:blank".
    /// </summary>
    [DefaultValue("https://tools.ietf.org/html/rfc9110#name-422-unprocessable-content")]
    [JsonPropertyOrder(-7)]
    [JsonRequired]
    public required string Type { get; init; }

    /// <summary>
    ///     The HTTP status code([RFC9110], Section 15) generated by the origin server for this occurrence of the problem.
    /// </summary>
    [DefaultValue(StatusCodes.Status422UnprocessableEntity)]
    [JsonPropertyOrder(-6)]
    [JsonRequired]
    public required short Status { get; init; }
}

/// <summary>
///     Internal Server Error Problem Details Resource
/// </summary>
[CoreLibrary]
public sealed record InternalServerProblemDetailsResource : ProblemDetailsBaseResource,
    IEqualityOperators<InternalServerProblemDetailsResource, InternalServerProblemDetailsResource, bool>
{
    /// <summary>
    ///     A URI reference [RFC8820] that identifies the problem type. This specification encourages that, when
    ///     dereferenced, it provide human-readable documentation for the problem type
    ///     (e.g., using HTML [W3C.REC-html5-20141028]). When this member is not present, its value is assumed to be
    ///     "about:blank".
    /// </summary>
    [DefaultValue("https://tools.ietf.org/html/rfc9110#name-500-internal-server-error")]
    [JsonPropertyOrder(-7)]
    [JsonRequired]
    public required string Type { get; init; }

    /// <summary>
    ///     The HTTP status code([RFC9110], Section 15) generated by the origin server for this occurrence of the problem.
    /// </summary>
    [DefaultValue(StatusCodes.Status500InternalServerError)]
    [JsonPropertyOrder(-6)]
    [JsonRequired]
    public required short Status { get; init; }
}

/// <summary>
///     Not Implemented Problem Details Resource
/// </summary>
[CoreLibrary]
public sealed record NotImplementedProblemDetailsResource : ProblemDetailsBaseResource,
    IEqualityOperators<NotImplementedProblemDetailsResource, NotImplementedProblemDetailsResource, bool>
{
    /// <summary>
    ///     A URI reference [RFC8820] that identifies the problem type. This specification encourages that, when
    ///     dereferenced, it provide human-readable documentation for the problem type
    ///     (e.g., using HTML [W3C.REC-html5-20141028]). When this member is not present, its value is assumed to be
    ///     "about:blank".
    /// </summary>
    [DefaultValue("https://tools.ietf.org/html/rfc9110#name-501-not-implemented")]
    [JsonPropertyOrder(-7)]
    [JsonRequired]
    public required string Type { get; init; }

    /// <summary>
    ///     The HTTP status code([RFC9110], Section 15) generated by the origin server for this occurrence of the problem.
    /// </summary>
    [DefaultValue(StatusCodes.Status501NotImplemented)]
    [JsonPropertyOrder(-6)]
    [JsonRequired]
    public required short Status { get; init; }
}