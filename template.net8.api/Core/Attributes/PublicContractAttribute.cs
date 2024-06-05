using JetBrains.Annotations;

namespace template.net8.Api.Core.Attributes;

/// <summary>
///     This attribute is intended to mark public API Contracts, which should not be removed and so is treated as used.
/// </summary>
[MeansImplicitUse(ImplicitUseTargetFlags.WithMembers)]
[AttributeUsage(AttributeTargets.Interface)]
[CoreLibrary]
internal sealed class PublicApiContractAttribute : Attribute;