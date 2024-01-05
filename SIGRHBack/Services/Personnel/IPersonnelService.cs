using SIGRHBack.Dtos.Authorization.User;
using SIGRHBack.Dtos.Personnel;
using SIGRHBack.Dtos.Shared;

namespace SIGRHBack.Services.Personnel
{
	public interface IPersonnelService
	{

		///Task<ResponseDto<IEnumerable<ServiceMetier.RegistreViewModel>>> GetRegistrePersonnel();

		Task<ResponseDto<IEnumerable<ServiceMetier.RegistreViewModel>>> GetRegistrePersonnel(ServiceMetier.GetAllRegistreFilter filter);
		Task<ResponseDto>? AddMagistrat(ServiceMetier.PersonnelViewModel magistrat);
		Task<ResponseDto>? AddFonctionnaire(InputFonctionnaireDto fonctionnaire);


    }
}
