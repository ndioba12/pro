using SIGRHBack.Dtos.ActeAdministration;
using SIGRHBack.Dtos.Shared;

namespace SIGRHBack.Services.Authorization.User
{
    public interface IActeAdministrationService
    {
        /// <summary>
        /// Créer un acte administration
        /// </summary>
        /// <param name="user">utilisateur à ajouter</param>
        /// <returns></returns>
        Task<ResponseDto>? CreateActeAdministration(ServiceMetier.AddOrUpdatecteAdminViewModel acte);
        Task<ResponseDto>? UpdateActeAdministration<T>(int id, T acte) where T : ServiceMetier.AddOrUpdatecteAdminViewModel;


    }
}
