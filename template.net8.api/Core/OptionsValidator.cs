using Microsoft.IdentityModel.Protocols.Configuration;
using template.net8.api.Core.OpenTelemetry.Options;
using template.net8.api.Settings.Options;

namespace template.net8.api.Core;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal static class OptionsValidator
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    /// <exception cref="InvalidConfigurationException">The Jwt configuration in the appsettings file is incorrect</exception>
    internal static void ValidateJwtOptions(JwtOptions? config)
    {
        var optionsValidator = new JwtOptionsValidator();
        if (config is null)
            throw new InvalidConfigurationException(
                "The Jwt configuration in the appsettings file is incorrect");

        var validation = optionsValidator.Validate(null, config);
        if (validation.Failed)
            throw new InvalidConfigurationException(validation.FailureMessage);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    /// <exception cref="InvalidConfigurationException">The Cors configuration in the appsettings file is incorrect</exception>
    internal static void ValidateCorsOptions(CorsOptions? config)
    {
        var optionsValidator = new CorsOptionsValidator();
        if (config is null)
            throw new InvalidConfigurationException(
                "The Cors configuration in the appsettings file is incorrect");

        var validation = optionsValidator.Validate(null, config);
        if (validation.Failed)
            throw new InvalidConfigurationException(validation.FailureMessage);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    /// <exception cref="InvalidConfigurationException">The OpenTelemetry configuration in the appsettings file is incorrect</exception>
    internal static void ValidateOpenTelemetryOptions(OpenTelemetryOptions? config)
    {
        var optionsValidator = new OpenTelemetryOptionsValidator();
        if (config is null)
            throw new InvalidConfigurationException(
                "The OpenTelemetry configuration in the appsettings file is incorrect");

        var validation = optionsValidator.Validate(null, config);
        if (validation.Failed)
            throw new InvalidConfigurationException(validation.FailureMessage);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    /// <exception cref="InvalidConfigurationException">Condition.</exception>
    internal static void ValidateAppDbOptions(AppDbOptions? config)
    {
        var optionsValidator = new AppDbOptionsValidator();
        if (config is null)
            throw new InvalidConfigurationException(
                "The App Db configuration in the appsettings file is incorrect");

        var validation = optionsValidator.Validate(null, config);
        if (validation.Failed)
            throw new InvalidConfigurationException(validation.FailureMessage);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    /// <exception cref="InvalidConfigurationException">The Api configuration in the appsettings file is incorrect</exception>
    internal static void ValidateApiOptions(ApiOptions? config)
    {
        var optionsValidator = new ApiOptionsValidator();
        if (config is null)
            throw new InvalidConfigurationException(
                "The Api configuration in the appsettings file is incorrect");

        var validation = optionsValidator.Validate(null, config);
        if (validation.Failed)
            throw new InvalidConfigurationException(validation.FailureMessage);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    /// <exception cref="InvalidConfigurationException">The Swagger configuration in the appsettings file is incorrect</exception>
    internal static void ValidateSwaggerOptions(SwaggerOptions? config)
    {
        var optionsValidator = new SwaggerOptionsValidator();
        if (config is null)
            throw new InvalidConfigurationException(
                "The Swagger configuration in the appsettings file is incorrect");

        var validation = optionsValidator.Validate(null, config);
        if (validation.Failed)
            throw new InvalidConfigurationException(validation.FailureMessage);
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    /// <exception cref="InvalidConfigurationException">The Swagger Security configuration in the appsettings file is incorrect</exception>
    internal static void ValidateSwaggerSecurityOptions(SwaggerSecurityOptions? config)
    {
        var optionsValidator = new SwaggerSecurityOptionsValidator();
        if (config is null)
            throw new InvalidConfigurationException(
                "The Swagger Security configuration in the appsettings file is incorrect");

        var validation = optionsValidator.Validate(null, config);
        if (validation.Failed)
            throw new InvalidConfigurationException(validation.FailureMessage);
    }
}