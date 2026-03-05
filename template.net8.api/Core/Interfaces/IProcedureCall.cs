using System.Diagnostics.CodeAnalysis;

namespace template.net8.api.Core.Interfaces;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
[SuppressMessage("Design",
    "CA1515:Consider making public types internal",
    Justification =
        "The interface is part of the public API contract and must remain publicly accessible.")]
internal interface IProcedureCall
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    string ProcedureName { get; }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    IEnumerable<object> Parameters { get; }
}