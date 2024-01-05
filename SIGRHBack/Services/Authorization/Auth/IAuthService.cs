using SIGRHBack.Dtos.Authorization.Auth;
using SIGRHBack.Dtos.Shared;

namespace SIGRHBack.Services.Authorization.Auth
{
    public interface IAuthService
    {
        /// <summary>
        /// Cette méthode permet à un utilisateur de se connecter en donnant ses identifiants.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<OutputLoginDto> LoginUserAsync(InputLoginDto model);
        /// <summary>
        /// cette méthode nous permet de d'envoyer un mail de réinitialisation à l'utilisateur
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResponseDto> ForgotPassword(InputForgetPasswordDto model);
        /// <summary>
        /// Cette méthode permet à un utilisateur de mettre à jour son mot de passe après réinitialisation.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        Task<ResponseDto> ResetPassword(InputUpdatePassword item);
    }
}