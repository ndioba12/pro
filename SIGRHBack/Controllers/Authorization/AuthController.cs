using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIGRHBack.Dtos.Authorization.Auth;
using SIGRHBack.Dtos.Shared;
using SIGRHBack.Services.Authorization.Auth;

namespace SIGRHBack.Controllers.Authorization
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(OutputLoginDto), StatusCodes.Status201Created)]
        public async Task<ActionResult> Login([FromBody] InputLoginDto model)
        {
            if (ModelState.IsValid)
            {
                var result = await _authService.LoginUserAsync(model);
                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                return Ok(result);
            }
            return BadRequest("Veuiller vérfiier vos entrants."); // status code 400
        }
        [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        [HttpPost("forgetpassword")]
        public async Task<ActionResult> ForgotPassword(InputForgetPasswordDto model)
        {
            if (ModelState.IsValid)
            {
                var result = await _authService.ForgotPassword(model);
                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                return Ok(result);
            }
            return BadRequest("Veuiller vérfiier vos entrants."); // status code 400
        }
        [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        [HttpPost("resetpassword")]
        public async Task<ActionResult> ResetPassword([FromBody] InputUpdatePassword model)
        {
            if (ModelState.IsValid)
            {
                var result = await _authService.ResetPassword(model);
                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                return Ok(result);
            }
            return BadRequest("Veuiller vérfiier vos entrants."); // status code 400
        }
    }
}
