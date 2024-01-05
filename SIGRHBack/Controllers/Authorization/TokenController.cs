using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIGRHBack.Database;
using SIGRHBack.Dtos.Shared;
using SIGRHBack.Dtos.Token;
using SIGRHBack.Ressources;
using SIGRHBack.Services.Authorization.Token;

namespace SIGRHBack.Controllers.Authorization
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class TokenController : ControllerBase
    {
        private readonly ITokenService _serviceToken;
        private readonly SIGRHBackDbContext _context;

        public TokenController(ITokenService serviceToken, SIGRHBackDbContext context)
        {
            _serviceToken = serviceToken;
            _context = context;
        }
        /// <summary>
        /// Rafraichir le token de l'utilisateur.   
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<ResponseDto<InputRefreshTokenDto>> Refresh(InputRefreshTokenDto model)
        {
            if (model is null)
                return BadRequest("Vérifier les informations entrées.");
            var principal = _serviceToken.GetPrincipalFromExpiredToken(model.AccessToken);
            var username = principal.FindFirst("Username")?.Value;
            var user = _context.TokenInfo
                .SingleOrDefault(u => u.Usename == username);
            if (user is null || user.RefreshToken != model.RefreshToken || user.RefreshTokenExpiry <= DateTime.Now)
                return BadRequest(Messages.InvalidToken);
            var newAccessToken = _serviceToken.CreateToken(principal.Claims);
            var newRefreshToken = _serviceToken.GetRefreshToken();
            user.RefreshToken = newRefreshToken;
            _context.SaveChanges();
            var result = new ResponseDto<InputRefreshTokenDto>()
            {
                Message = Messages.TokenRefreshSuccess,
                IsSuccess = true,
                Result = new InputRefreshTokenDto()
                {
                    AccessToken = newAccessToken.TokenString,
                    RefreshToken = newRefreshToken
                }
            };
            return Ok(new InputRefreshTokenDto()
            {
                AccessToken = newAccessToken.TokenString,
                RefreshToken = newRefreshToken
            });
        }
        /// <summary>
        /// Supprime le refresh token de l'utilisateur dans le système.
        /// </summary>
        /// <returns></returns>
        [HttpPost, Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<ResponseDto> Revoke()
        {
            try
            {
                var username = User.FindFirst("Username")?.Value;
                var user = _context.TokenInfo.SingleOrDefault(u => u.Usename == username);
                if (user is null)
                    return BadRequest(Messages.TokenRevokeUserNotFound);
                user.RefreshToken = string.Empty;
                _context.SaveChanges();
                return Ok(new ResponseDto
                {
                     Message = Messages.TokenRevokeSuccess,
                     IsSuccess = true,
                     Result = true
                });
            }
            catch (Exception ex)
            {
                return BadRequest(Messages.ErrorTokenRevoke);
            }
        }
    }
}
