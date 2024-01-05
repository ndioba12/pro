using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SIGRHBack.Dtos.Shared;
using SIGRHBack.Ressources;

namespace SIGRHBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfilController : ControllerBase
    {
        private ServiceMetier.Service1Client service = new ServiceMetier.Service1Client();

        [HttpGet]
        [ProducesResponseType(typeof(ResponseDto<IEnumerable<ServiceMetier.TP_Profil>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status500InternalServerError)]
        public ActionResult getAll()
        {
            try
            {
                var response = new ResponseDto<IEnumerable<ServiceMetier.TP_Profil>>()
                {
                    Message = Messages.GetAllProfil,
                    IsSuccess = true,
                    Result = service.GetListProfils()    
                };
                return new ObjectResult(response)
                {
                    StatusCode = StatusCodes.Status200OK
                };
            }
            catch (Exception ex)
            {
                var response = new ResponseDto
                {
                    Message = Messages.ErrorInternal500,
                    IsSuccess = false
                };
                return new ObjectResult(response)
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }
        [HttpPut("{codeProfil}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public ActionResult<ResponseDto> Update([FromBody] ServiceMetier.TP_Profil profil, string codeProfil)
        {
            var result = service.UpdateProfil(codeProfil, profil);
            var response = new ResponseDto()
            {
                Message = result ? Messages.UpdateSuccess : Messages.UpdateError,
                IsSuccess = result,
                Result = result
            };
            return Ok(response);
        }
        
        [HttpPut("activerOuDesactiver/{codeProfil}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public ActionResult<ResponseDto> ActiverOuDesactiver(string codeProfil)
        {
            var result = service.ActiverOuDesactiverProfil(codeProfil);
            var response = new ResponseDto();
            if (result != null) {
                response.Message = result.Pro_ActifOuiNon == "0" ? Messages.DesactiverSuccess : Messages.ActiverSuccess;
                response.IsSuccess = true;
                response.Result = result;
            }
            else
            {
                response.Message = Messages.OperationError;
                response.IsSuccess = false;
                response.Result = result;
            }
            return Ok(response);
        }
    }
}
