using JetBrains.Annotations;

namespace template.net8.Api.Core.Attributes;

/// <summary>
///     This attribute is intended to mark core code, This code should be left unchanged if you are not sure what you are
///     doing. Not be removed and so is treated as used.
/// </summary>
[MeansImplicitUse(ImplicitUseTargetFlags.WithMembers)]
[AttributeUsage(AttributeTargets.All, Inherited = false)]
[CoreLibrary]
internal sealed class CoreLibraryAttribute : Attribute;