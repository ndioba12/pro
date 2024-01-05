using SIGRHBack.Dtos.Authorization.Role;
using SIGRHBack.Dtos.Authorization.User;
using SIGRHBack.Dtos.Shared;

namespace SIGRHBack.Services.Authorization.User
{
    public interface IUserService
    {
        /// <summary>
        /// Créer un utilisateur.
        /// </summary>
        /// <param name="user">utilisateur à ajouter</param>
        /// <returns></returns>
        Task<ResponseDto>? CreateUser(InputUserDto user);
        /// <summary>
        /// Mettre à jour un utilisateur.
        /// </summary>
        /// <param name="userId">Identifiant utilisateur</param>
        /// <param name="user">Utilisateur à modifier</param>
        /// <returns></returns>
        Task<ResponseDto>? UpdateUser<T>(string userId, T user) where T : InputUpdatedAccountDto;
        /// <summary>
        /// Activer ou désactiver un utilisateur désactivé.
        /// </summary>
        /// <param name="userId">identifiant utilisateur</param>
        /// <returns></returns>
        Task<ResponseDto?> ActiverOuDesactiverUser(string userId);
        /// <summary>
        /// Réinitialiser le mot de passe d'un utilisateur donné.
        /// </summary>
        /// <param name="userId">identifiant utilisateur</param>
        /// <returns></returns>
        Task<ResponseDto?> ResetPassword(string userId);
        /// <summary>
        /// Mettre à jour le mot de passe d'un utilisateur.
        /// </summary>
        /// <param name="input">les informations du mot de passe</param>
        /// <returns></returns>
        Task<ResponseDto?> UpdatePassword(InputUserUpdatePassword input);
        /// <summary>
        /// Supprimer un utilisateur.
        /// </summary>
        /// <param name="userId">identifiant utilisateur</param>
        /// <returns></returns>
        Task<bool> DeleteUser(string userId);
        /// <summary>
        /// Obtenir un utilisateur par son identifiant.
        /// </summary>
        /// <param name="userId">identifiant utilisateur</param>
        /// <returns></returns>
        Task<ResponseDto<OutputUserDto>> GetUserById(string userId);
        /// <summary>
        /// Obtenir un utilisateur à partir de son adresse electronique.
        /// </summary>
        /// <param name="email">email utilisateur</param>
        /// <returns></returns>
        Task<ResponseDto<ServiceMetier.UserViewModel>> GetUserByEmail(string email);
        /// <summary>
        /// Obtenir tous les utilisateurs.
        /// </summary>
        /// <returns></returns>
        Task<ResponseDto<IEnumerable<ServiceMetier.UserViewModel>>> GetAllUser(ServiceMetier.GetAllUserFilter filter);
        /// <summary>
        /// Assigner un rôle à un utilisateur.
        /// </summary>
        /// <param name="id">identifiant utilisateur</param>
        /// <param name="role">rôle à ajouter</param>
        /// <returns></returns>
        Task<ResponseDto> AssignToRole(string id, RoleDto role);
        /// <summary>
        /// Supprimer un rôle à un utilisateur
        /// </summary>
        /// <param name="id">identifiant utilisateur</param>
        /// <param name="role">rôle à ajouter</param>
        /// <returns></returns>
        Task<ResponseDto> RemoveToRole(string id, RoleDto role);
    }
}
