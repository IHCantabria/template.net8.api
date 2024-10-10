using System.Numerics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using template.net8.api.Core.Attributes;

namespace template.net8.api.Core.Authorization;

/// <summary>
///     Claim Requirement Handler
/// </summary>
[CoreLibrary]
public sealed class ClaimRequirementHandler : AuthorizationHandler<ClaimRequirements>
{
    /// <summary>
    ///     Handle Requirement Async
    /// </summary>
    /// <param name="context"></param>
    /// <param name="requirement"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref>
    ///         <name>argument</name>
    ///     </paramref>
    ///     is <see langword="null" />.
    /// </exception>
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ClaimRequirements requirement)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(requirement);
        if (HasRequiredClaims(context.User, requirement))
            context.Succeed(requirement);
        return Task.CompletedTask;
    }

    private static bool HasRequiredClaims(ClaimsPrincipal principal, ClaimRequirements requirements)
    {
        return requirements.ClaimLogic switch
        {
            ClaimLogic.All => requirements.ClaimRequirementsCollection.All(claimRequirement =>
                HasRequiredClaim(principal.Claims, claimRequirement)),
            ClaimLogic.Any => requirements.ClaimRequirementsCollection.Any(claimRequirement =>
                HasRequiredClaim(principal.Claims, claimRequirement)),
            _ => false
        };
    }

    private static bool HasRequiredClaim(IEnumerable<Claim> claims, ClaimRequirement requirement)
    {
        return claims.Any(c =>
            c.Type == requirement.ClaimType &&
            c.Value.Contains(requirement.ClaimValue, StringComparison.InvariantCultureIgnoreCase));
    }
}

/// <summary>
///     Claim Requirements
/// </summary>
/// <param name="ClaimRequirementsCollection"></param>
/// <param name="ClaimLogic"></param>
[CoreLibrary]
public sealed record ClaimRequirements(
    IEnumerable<ClaimRequirement> ClaimRequirementsCollection,
    ClaimLogic ClaimLogic = ClaimLogic.All)
    : IAuthorizationRequirement, IEqualityOperators<ClaimRequirements, ClaimRequirements, bool>
{
    /// <summary>
    ///     Claim Requirements
    /// </summary>
    public IEnumerable<ClaimRequirement> ClaimRequirementsCollection { get; } = ClaimRequirementsCollection;

    /// <summary>
    ///     Claim Logic
    /// </summary>
    public ClaimLogic ClaimLogic { get; } = ClaimLogic;
}

/// <summary>
///     Claim Requirement
/// </summary>
/// <param name="ClaimType"></param>
/// <param name="ClaimValue"></param>
[CoreLibrary]
public sealed record ClaimRequirement(string ClaimType, string ClaimValue)
    : IEqualityOperators<ClaimRequirement, ClaimRequirement, bool>
{
    /// <summary>
    ///     Claim Type
    /// </summary>
    public string ClaimType { get; } = ClaimType;

    /// <summary>
    ///     Claim Value
    /// </summary>
    public string ClaimValue { get; } = ClaimValue;
}

/// <summary>
///     Enum Claim Logic
/// </summary>
[CoreLibrary]
public enum ClaimLogic
{
    /// <summary>
    ///     All Must be satisfied
    /// </summary>
    All = 0,

    /// <summary>
    ///     Any Must be satisfied
    /// </summary>
    Any = 1
}