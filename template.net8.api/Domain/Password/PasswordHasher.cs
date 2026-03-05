using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Text;
using LanguageExt;
using template.net8.api.Domain.DTOs;

namespace template.net8.api.Domain.Password;

/// <summary>
///     ADD DOCUMENTATION
/// </summary>
internal static class PasswordHasher
{
    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private const short KeySize = 64;

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private const int Iterations = 350000;

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static readonly HashAlgorithmName HashAlgorithm = HashAlgorithmName.SHA512;

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    /// <exception cref="EncoderFallbackException">
    ///     A fallback occurred (for more information, see Character Encoding in .NET)
    ///     -and-
    ///     <see cref="EncoderFallback" /> is set to <see cref="EncoderExceptionFallback" />.
    /// </exception>
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumentedOptional",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
    internal static Try<(string, string)> HashPasword(CreateUserPasswordDto payload)
    {
        return () =>
        {
            var salt = RandomNumberGenerator.GetBytes(KeySize);
            var spicyPassword = $"{payload.Password}{payload.Pepper}";
            var hash = Rfc2898DeriveBytes.Pbkdf2(Encoding.UTF8.GetBytes(spicyPassword), salt, Iterations, HashAlgorithm,
                KeySize);
            return (Convert.ToHexString(hash), Convert.ToHexString(salt));
        };
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumented",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
    [SuppressMessage(
        "ReSharper",
        "ExceptionNotDocumentedOptional",
        Justification =
            "Potential exceptions originate from underlying implementation details and are not part of the method contract.")]
    internal static Try<bool> VerifyPassword(VerifyUserPasswordDto payload)
    {
        return () =>
        {
            var spicyPassword = $"{payload.Password}{payload.Pepper}";
            var saltBytes = SaltHexStringToByteArray(payload.Salt);
            var hashToCompare =
                Rfc2898DeriveBytes.Pbkdf2(spicyPassword, saltBytes, Iterations, HashAlgorithm, KeySize);
            return CryptographicOperations.FixedTimeEquals(hashToCompare,
                Convert.FromHexString(payload.Hash));
        };
    }

    /// <summary>
    ///     ADD DOCUMENTATION
    /// </summary>
    private static byte[] SaltHexStringToByteArray(string hex)
    {
        return Enumerable.Range(0, hex.Length)
            .Where(static x => x % 2 == 0)
            .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
            .ToArray();
    }
}