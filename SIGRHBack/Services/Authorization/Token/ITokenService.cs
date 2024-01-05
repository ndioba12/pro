using SIGRHBack.Dtos.Token;
using System.Security.Claims;

namespace SIGRHBack.Services.Authorization.Token
{
    public interface ITokenService
    {
        TokenResponse? CreateToken(IEnumerable<Claim> claims);
        string GetRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}