using template.net8.api.Core.Attributes;

namespace template.net8.api.Core.Interfaces;

/// <summary>
///     Interface for Procedure Call Implementation for Querying Data with EF Core
/// </summary>
[CoreLibrary]
public interface IProcedureCall
{
    /// <summary>
    ///     Procedure name to execute
    /// </summary>
    string ProcedureName { get; }

    /// <summary>
    ///     Parameters to pass to the procedure
    /// </summary>
    IEnumerable<object> Parameters { get; }
}