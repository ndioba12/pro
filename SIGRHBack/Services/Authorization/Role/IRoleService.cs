using SIGRHBack.Dtos.Authorization.Role;
using SIGRHBack.Dtos.Shared;

namespace SIGRHBack.Services.Authorization.Role
{
    public interface IRoleService
    {
        /// <summary>
        /// Obtenir tous les rôles.
        /// </summary>
        /// <returns></returns>
        Task<ResponseDto<IEnumerable<RoleDto>>> GetAll();
        /// <summary>
        /// Créer un rôle.
        /// </summary>
        /// <param name="name">le nom du rôle</param>
        /// <returns></returns>
        Task<ResponseDto> CreateRole(string name);
        /// <summary>
        /// Supprimer un rôle.
        /// </summary>
        /// <param name="id">identifiant du rôle</param>
        /// <returns></returns>
        Task<ResponseDto> Delete(string id);
        /// <summary>
        /// Obtenir un rôle à partir de son identifiant.
        /// </summary>
        /// <param name="id">identifiant du rôle</param>
        /// <returns></returns>
        Task<ResponseDto<RoleDto>> GetOne(string id);
        /// <summary>
        /// Mettre à jour un rôle.
        /// </summary>
        /// <param name="id">identifiant du rôle</param>
        /// <param name="name">le nom du rôle</param>
        /// <returns></returns>
        Task<ResponseDto> UpdateRole(string id, string name);
    }
}