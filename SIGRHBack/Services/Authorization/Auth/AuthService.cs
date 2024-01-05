using Microsoft.AspNetCore.Identity;
using SIGRHBack.Database;
using SIGRHBack.Database.Shared;
using SIGRHBack.Dtos.Authorization.Auth;
using SIGRHBack.Dtos.Shared;
using SIGRHBack.Helpers;
using SIGRHBack.Models;
using SIGRHBack.Ressources;
using SIGRHBack.Services.Authorization.Token;
using SIGRHBack.Services.Messagerie.Mail;
using System.Security.Claims;
using System.Text;
using System.Web;

namespace SIGRHBack.Services.Authorization.Auth
{
    public class AuthService : IAuthService
    {
        private readonly SIGRHBackDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _serviceToken;
        private readonly IMailService _mailService;
        private ServiceMetier.Service1Client service = new ServiceMetier.Service1Client();

        public AuthService(UserManager<AppUser> userManager, SIGRHBackDbContext context, ITokenService serviceToken, IMailService mailService)
        {
            _userManager = userManager;
            _context = context;
            _serviceToken = serviceToken;
            _mailService = mailService;
        }
        public async Task<OutputLoginDto> LoginUserAsync(InputLoginDto model)
        { 
            OutputLoginDto response = new();
            // check email
            var user = await _userManager.FindByNameAsync(model.Identifiant);
            if (user == null)
            {
                response.Message = Messages.ErrorLoginIdentity.ToString();
                response.IsSuccess = false;
                return response;
            }
            // check lock on user
            if (await _userManager.IsLockedOutAsync((user)))
            {
                response.Message = Messages.ErrorLoginFailedCount.ToString();
                response.IsSuccess = false;
                return response;
            }
            // check password
            bool result = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!result)
            {
                // mettre à jour le nombre de tentatives de connexion
                await _userManager.AccessFailedAsync(user);
                response.Message = Messages.ErrorLoginIdentity.ToString();
                response.IsSuccess = false;
                return response;
            }
            var tdUser = await service.GetOneUserByParentAsync(user.Id);
            // mettre à zero le compteur d'echecs de connexion d'un utilisateur
            await _userManager.ResetAccessFailedCountAsync(user);
            // lier
            if (tdUser.Uti_ActifOuiNon == "0" || tdUser.Uti_Pro_Code.Equals(UserRoleNames.Revoquer,StringComparison.OrdinalIgnoreCase))
            {
                response.Message = Messages.ErrorLoginSuspended.ToString();
                response.IsSuccess = false;
                return response;
            }
            // gets roles
            var role = await _userManager.GetRolesAsync(user);
            // create token 
            var claims = new[]
            {
                new Claim("Email",user.Email),
                new Claim("Id",tdUser.Uti_Id.ToString()),
                new Claim("UserId",user.Id),
                new Claim("Username",user.UserName ?? ""),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.Role,tdUser.Uti_Pro_Code),
                new Claim(ClaimTypes.NameIdentifier,user.Id),
            };
            var token = _serviceToken.CreateToken(claims);
            if (token != null)
            {
                var refreshToken = _serviceToken.GetRefreshToken();
                response.Message = Messages.LoginSuccess;
                response.IsSuccess = true;
                response.Profil = tdUser.Uti_Pro_Code;
                response.Token = token.TokenString;
                response.TypeToken = "Bearer ";
                response.ExpiresToken = DateTime.UtcNow.AddHours(2);
                response.EmailUser = user.Email;
                response.Name = user.UserName;
                response.RefreshToken = refreshToken;
                response.NomUtilisateur = tdUser.Uti_Nom;
                response.PrenomUtilisateur = tdUser.Uti_Prenom;
                response.IdUser = user.Id;
                response.avatar = tdUser?.Uti_Nom[0].ToString() + tdUser?.Uti_Prenom [0].ToString();
                // menus for role
                response.Menus = service.GetAllMenu(role.FirstOrDefault());
                // save refresh token 
                var tokenInfo = _context.TokenInfo.FirstOrDefault(a => a.Usename == user.UserName);
                if (tokenInfo == null)
                {
                    var info = new TokenInfo
                    {
                        Usename = user.UserName,
                        RefreshToken = refreshToken,
                        RefreshTokenExpiry = DateTime.Now.AddDays(1)
                    };
                    _context.TokenInfo.Add(info);
                }
                else
                {
                    tokenInfo.RefreshToken = refreshToken;
                    tokenInfo.RefreshTokenExpiry = DateTime.Now.AddDays(1);
                }
                try
                {
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    //throw;
                }
                return response;
            }
            return response;
        }
        public async Task<ResponseDto> ForgotPassword(InputForgetPasswordDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null && model.Email != AppConsts.EmailAdmin)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var tokenEncoded = HttpUtility.UrlEncode(token);
                // send email
                    var resultSendEmail = _mailService.SendMailResetPassword(user.Email, tokenEncoded);
                //_sendMailService.AddOne(resultSendEmail);
                return new ResponseDto
                {
                    IsSuccess = true,
                    Message = Messages.ForgotPasswordSuccess.ToString(),
                    Result = tokenEncoded,
                };
            }
            else
            {
                return new ResponseDto
                {
                    IsSuccess = false,
                    Message = string.Format(Messages.ErrorForgotPasswordNotFound.ToString(), model.Email),
                    Result = null,
                };
            }
        }

        public async Task<ResponseDto> ResetPassword(InputUpdatePassword item)
        {
            var user = await _userManager.FindByEmailAsync(item.Email);
            if (user != null)
            {
                var response = new ResponseDto();
                var token = item?.Token?.Replace(" ", "+");
                var result = await _userManager.ResetPasswordAsync(user, item?.Token, item?.NewPassword);
                if (result.Succeeded)
                {

                    response.IsSuccess = true;
                    response.Message = Messages.ResetPasswordSuccess.ToString();
                    response.Result = result;
                    return response;
                }
                else
                {
                    if (result.Errors.Any(e => e.Code == "PasswordRequiresDigit"))
                    {
                        response.Message = string.Format(Messages.PasswordRequireDigits.ToString(), user.UserName);
                    }
                    else if (result.Errors.Any(e => e.Code == "PasswordRequiresNonAlphanumeric"))
                    {
                        response.Message = string.Format(Messages.PasswordRequiresNonAlphanumeric.ToString(), user.UserName);
                    }
                    else if (result.Errors.Any(e => e.Code == "PasswordRequiresUpper"))
                    {
                        response.Message = string.Format(Messages.PasswordRequiresUpper.ToString(), user.Email);
                    }
                    else if (result.Errors.Any(e => e.Code == "PasswordRequiresLower"))
                    {
                        response.Message = string.Format(Messages.PasswordRequiresLower.ToString(), user.Email);
                    }
                    else if (result.Errors.Any(e => e.Code == "InvalidToken"))
                    {
                        response.Message = string.Format(Messages.InvalidToken, user.Email);
                    }
                    else
                    {
                        response.Message = Messages.ErrorResetPassword.ToString();
                    }
                    response.IsSuccess = false;
                    response.Result = result;
                    return response;
                }
            }
            else
            {
                return new ResponseDto
                {
                    IsSuccess = false,
                    Message = Messages.ErrorResetPasswordUserNotFound.ToString(),
                    Result = null,
                };
            }
        }
    }
}
