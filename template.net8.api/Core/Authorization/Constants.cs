using template.net8.api.Core.Attributes;

namespace template.net8.api.Core.Authorization;

[CoreLibrary]
internal static class TokenTypesIdentityConstants
{
    internal const string IdTokenType = "id_token";
    internal const string AccessTokenType = "access_token";
}

[CoreLibrary]
internal static class GenieIdentityConstants
{
    internal const string UserName = "genio";
    internal const string FirsName = "will";
    internal const string LastName = "smith";
    internal const string Email = "un_genio_no_necesita_email";
    internal const string RoleName = "el_genio_de_la_lampara";
    internal const string Identifier = "no_hay_un_genio_tan_genial";
    internal const string Scope = "un_espacio_chiquitin_para_vivir";
}

[CoreLibrary]
internal static class ClaimCoreConstants
{
    internal const string ScopeClaim = "scope";
    internal const string TokenTypeClaim = "token_type";
    internal const string ApplicationPrivilegesClaim = "application_privileges";
}