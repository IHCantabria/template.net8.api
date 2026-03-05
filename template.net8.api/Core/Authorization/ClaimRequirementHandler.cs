using System.Numerics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace template.net8.api.Core.Authorization;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal sealed class ClaimRequirementHandler : AuthorizationHandler<ClaimRequirements>
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="context" /> is <see langword="null" />.
    ///     <paramref name="requirement" /> is <see langword="null" />.
    /// </exception>
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ClaimRequirements requirement)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(requirement);
        if (HasRequiredClaims(context.User, requirement))
            context.Succeed(requirement);
        return Task.CompletedTask;
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
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

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static bool HasRequiredClaim(IEnumerable<Claim> claims, ClaimRequirement requirement)
    {
        return claims.Any(c =>
            c.Type == requirement.ClaimType &&
            c.Value.Contains(requirement.ClaimValue, StringComparison.InvariantCultureIgnoreCase));
    }
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal sealed record ClaimRequirements(
    IEnumerable<ClaimRequirement> ClaimRequirementsCollection,
    ClaimLogic ClaimLogic = ClaimLogic.All)
    : IAuthorizationRequirement, IEqualityOperators<ClaimRequirements, ClaimRequirements, bool>
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public IEnumerable<ClaimRequirement> ClaimRequirementsCollection { get; } = ClaimRequirementsCollection;

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public ClaimLogic ClaimLogic { get; } = ClaimLogic;
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal sealed record ClaimRequirement(string ClaimType, string ClaimValue)
    : IEqualityOperators<ClaimRequirement, ClaimRequirement, bool>
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public string ClaimType { get; } = ClaimType;

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    public string ClaimValue { get; } = ClaimValue;
}

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal enum ClaimLogic
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    All = 0,

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    Any = 1
}