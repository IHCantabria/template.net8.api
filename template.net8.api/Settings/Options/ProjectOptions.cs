﻿using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Options;
using template.net8.Api.Core.Attributes;

namespace template.net8.Api.Settings.Options;

/// <summary>
///     Project Options
/// </summary>
[CoreLibrary]
public sealed record ProjectOptions
{
    /// <summary>
    ///     Version of the project
    /// </summary>
    [Required]
    public required string Version { get; init; }
}

/// <summary>
///     Project Options Validator
/// </summary>
[OptionsValidator]
[CoreLibrary]
public sealed partial class ProjectOptionsValidator : IValidateOptions<ProjectOptions>;