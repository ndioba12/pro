using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ServiceMetier;
using SIGRHBack.Dtos.ActeAdministration;
using SIGRHBack.Dtos.ActeGestion;
using SIGRHBack.Dtos.Shared;
using SIGRHBack.Ressources;

namespace SIGRHBack.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class ActeAdministration : ControllerBase
    {

        private readonly IMapper _mapper;

        public ActeAdministration(IMapper mapper)
        {
            _mapper = mapper;
        }


        IService1 service = new Service1Client();

        [HttpGet]
        [ProducesResponseType(typeof(ResponseDto<IEnumerable<ActeAdminViewModel>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status500InternalServerError)]
        public ActionResult GetAllActeAdministration([FromQuery] GetAllActesAdministrationFilter filter)
        {
            var result = service.GetListeActeAdministration(filter);
            var response = new ResponseDto<IEnumerable<ActeAdminViewModel>>()
            {
                Message = result != null ? Messages.GetAllActesGestionSuccess : Messages.GetAllActesGestionError,
                IsSuccess = result != null ? true : false,
                Result = result
            };
            return result!=null ? Ok(response) : new ObjectResult(response) { StatusCode = StatusCodes.Status500InternalServerError };
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ResponseDto<ActeAdminViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status500InternalServerError)]
        public ActionResult GetActeAdministration(int id)
        {
            var result = service.GetActeAdministrationByCode(id);
            var response = new ResponseDto<ActeAdminViewModel>()
            {
                Message = result != null ? Messages.GetSuccess : Messages.GetError,
                IsSuccess = result != null ? true : false,
               // Result = result
            };
            return result != null ? Ok(response) : new ObjectResult(response) { StatusCode = StatusCodes.Status500InternalServerError };
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public ActionResult UpdateActeAdministration(int id, [FromBody] AddOrUpdatecteAdminViewModel acte)
        {

            if (ModelState.IsValid)
            {
               var result = service.UpdateActeAdministration(id, acte);
                var response = new ResponseDto()
                {
                    Message = result ? Messages.UpdateSuccess : Messages.UpdateError,
                    IsSuccess = result,
                    Result = result
                };
                return result ? Ok(response) : new ObjectResult(response) { StatusCode = StatusCodes.Status500InternalServerError };
            }
            else 
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> AddActeAdministration([FromBody] ServiceMetier.AddOrUpdatecteAdminViewModel acte)
        {

            if (ModelState.IsValid)
            {
                //var acteAdminViewModel = _mapper.Map<ActeAdminViewModel>(acte);
                //var id = User.FindFirst("UserId").Value;
               // acte.IdUserCreation = User.FindFirst("UserId").Value;
                var result =service.AddActeAdministration(acte);
                var response = new ResponseDto()
                {
                    Message = result ? Messages.AddSuccess : Messages.AddError,
                    IsSuccess = result,
                    Result = result
                };
                return result ? Ok(response) : new ObjectResult(response) { StatusCode = StatusCodes.Status500InternalServerError };
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }

}
