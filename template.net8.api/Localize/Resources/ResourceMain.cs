using System.Diagnostics.CodeAnalysis;

namespace template.net8.api.Localize.Resources;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[SuppressMessage(
    "Major Code Smell",
    "S2094:Classes should not be empty",
    Justification = "Marker type used for resource localization via IStringLocalizer<T>.")]
[SuppressMessage(
    "Design",
    "CA1515:Consider making public types internal",
    Justification = "Public visibility is required as the type is used for generic resource localization binding.")]
[SuppressMessage(
    "ReSharper",
    "ClassNeverInstantiated.Global",
    Justification = "Marker type used for generic resource localization via IStringLocalizer<T>.")]
public sealed class ResourceMain;