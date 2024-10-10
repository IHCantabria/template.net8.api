using Microsoft.AspNetCore.Authorization;
using template.net8.api.Core.Attributes;
using template.net8.api.Core.Authorization;

namespace template.net8.api.Settings.Extensions;

[CoreLibrary]
internal static class AuthorizationBuilderExtensions
{
    /// <exception cref="Exception">A delegate callback throws an exception.</exception>
    internal static void AddPolicies(this AuthorizationBuilder authorizationBuilder, bool isProduction)
    {
        var policies = GetPolicies();

        foreach (var policy in policies)
            authorizationBuilder.AddPolicy(policy.Key, policyOptions =>
            {
                var requirements = policy.Value(isProduction);
                var claimLogic = isProduction ? ClaimLogic.All : ClaimLogic.Any;
                policyOptions.AddRequirements(new ClaimRequirements(requirements, claimLogic));
            });
    }

    private static Dictionary<string, Func<bool, List<ClaimRequirement>>> GetPolicies()
    {
        var policies = new Dictionary<string, Func<bool, List<ClaimRequirement>>>();

        //ADD Here the policies like this. ApplicationAccessPolicy is the name of the policy. ApplicationAccessClaimValue is the value of the claim
        //policies.Add(PoliciesConstants.ApplicationAccessPolicy, isProduction =>
        //    CreatePolicy(isProduction, ClaimIdentityConstants.ApplicationAccessClaimValue));
        return policies;
    }

    private static List<ClaimRequirement> CreatePolicy(bool isProduction, string claimValue)
    {
        return isProduction
            ? [new ClaimRequirement(ClaimCoreConstants.ApplicationPrivilegesClaim, claimValue)]
            :
            [
                new ClaimRequirement(ClaimCoreConstants.ScopeClaim, GenieIdentityConstants.Scope),
                new ClaimRequirement(ClaimCoreConstants.ApplicationPrivilegesClaim, claimValue)
            ];
    }
}