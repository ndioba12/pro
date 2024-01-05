using AutoMapper;
using SIGRHBack.Database;
using SIGRHBack.Dtos.ActeAdministration;
using SIGRHBack.Dtos.Shared;
using SIGRHBack.Ressources;

namespace SIGRHBack.Services.Authorization.User
{
    public class ActeAdministrationService : IActeAdministrationService
    {
        private readonly SIGRHBackDbContext _context;
        private readonly IMapper _mapper;
        private ServiceMetier.Service1Client service = new ServiceMetier.Service1Client();


        //constructor
        public ActeAdministrationService(IMapper mapper,SIGRHBackDbContext context)
        {
            _mapper = mapper;

            _context = context;
        }

        public async Task<ResponseDto<IEnumerable<ServiceMetier.ActeAdminViewModel>>> GetAllActeAdmin(ServiceMetier.GetAllActesAdministrationFilter filter)
        {

            var allActes = service.GetListeActeAdministration(filter);
            return new ResponseDto<IEnumerable<ServiceMetier.ActeAdminViewModel>>
            {
                Errors = null,
                IsSuccess = true,
                Message = Messages.GetDatasSuccess.ToString(),
                CountTotal = allActes.Count(),
                Result = allActes
            };
        }


        public async Task<ResponseDto>? CreateActeAdministration(ServiceMetier.AddOrUpdatecteAdminViewModel acte)
        {


            try
            {
              //  var acte = _mapper.Map<ServiceMetier.TD_ActeAdminstration>(acteDto);
                var result =await service.AddActeAdministrationAsync(acte);
                if (result)
                {
                    return new ResponseDto
                    {
                        IsSuccess = true,
                        Message = "Acte crée avec succès.",
                    };
                }
                return new ResponseDto
                {
                    //Errors = result.Errors,
                    IsSuccess = false,
                    Message = "Erreur de création de l'acte  ."

                };
            }
            catch (Exception ex)
            {
                return new ResponseDto
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }


        public async Task<ResponseDto>? DeleteActeAdministration(int id)
        {
            try
            {
                var acte = service.GetActeAdministrationByCode(id);
                if (acte != null)
                {
                    var result = await service.DeleteActeAdministrationAsync(acte.Ata_Id);
                    return new ResponseDto
                    {
                        IsSuccess = true,
                        Message = "Acte supprimé avec succès.",
                    };
                }
                return new ResponseDto
                {
                    IsSuccess = false,
                    Message = "Cet Acte n'exixte pas",
                };
            }
            catch (Exception e)
            {
                return new ResponseDto
                {
                    IsSuccess = false,
                    Message = e.Message,
                };
            }
        }


        public async Task<ResponseDto>? UpdateActeAdministration<T>(int id, T acte) where T : ServiceMetier.AddOrUpdatecteAdminViewModel
        {
            //var acte = _mapper.Map<ServiceMetier.TD_ActeAdminstration>(acteDTO);
          
           // acte.Ata_Id = id;
            var result = await service.UpdateActeAdministrationAsync(id, acte);
            if (result)
            {
                return new ResponseDto
                {
                    IsSuccess = true,
                    Message = Messages.UpdateActeSuccess.ToString(),
                    Result =acte
                };
            }
            return new ResponseDto
            {
                IsSuccess = false,
                Message = Messages.ErrorUpdateActe.ToString(),
                 Result = null
            };

        }

    }
}
