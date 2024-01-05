using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceMetier;
using SIGRHBack.Dtos.ActeGestion;
using SIGRHBack.Dtos.Shared;
using SIGRHBack.Ressources;
using System.Linq.Expressions;

namespace SIGRHBack.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ActeGestionController : ControllerBase
    {

        private readonly IMapper _mapper;
        private string DefaultDirectory = "C:\\Users\\kanem\\Documents\\Gainde 2000\\Projet SIGRHJustice\\sigrhJusticeBackend\\fichiers";

        public ActeGestionController(IMapper mapper)
        {
            _mapper = mapper;
        }


        ServiceMetier.IService1 service = new ServiceMetier.Service1Client();
        ServiceFile.IService1 serviceFile = new ServiceFile.Service1Client();

        [HttpGet]
        [ProducesResponseType(typeof(ResponseDto<IEnumerable<ServiceMetier.ActeGestionConsultationViewModel>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status500InternalServerError)]
        public ActionResult getAllActesGestion([FromQuery] ServiceMetier.GetAllActesGestionFilter filter)
        {
            var result = service.GetAllActesGestion(filter);
            var response = new ResponseDto<IEnumerable<ServiceMetier.ActeGestionConsultationViewModel>>()
            {
                Message = result != null ? Messages.GetAllActesGestionSuccess : Messages.GetAllActesGestionError,
                IsSuccess = result != null ? true : false,
                Result = result
            };
            return result!=null ? Ok(response) : new ObjectResult(response) { StatusCode = StatusCodes.Status500InternalServerError };
        }

        [HttpGet("{idActeGestion:int}")]
        [ProducesResponseType(typeof(ResponseDto<ServiceMetier.ActeGestionConsultationViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status500InternalServerError)]
        public ActionResult getActeGestion(int idActeGestion)
        {
            var result = service.GetActeGestion(idActeGestion);
            var response = new ResponseDto<ServiceMetier.ActeGestionConsultationViewModel>()
            {
                Message = result != null ? Messages.GetSuccess : Messages.GetError,
                IsSuccess = result != null ? true : false,
                Result = result
            };
            return result != null ? Ok(response) : new ObjectResult(response) { StatusCode = StatusCodes.Status500InternalServerError };
        }

        [HttpGet("download/{idDocument:int}")]
        [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status404NotFound)]
        public ActionResult downloadDocument(int idDocument)
        {
            var fichier = service.GetFichier(idDocument);
            byte[] fileBytes;
            var response = new ResponseDto()
            {
                Message = "Une erreur interne est survenue lors de lors de la récupération du fichier.",
                IsSuccess = false
            };
            if (fichier != null)
            {
                try
                {
                    fileBytes = System.IO.File.ReadAllBytes(fichier.Fic_Chemin);
                }
                catch (Exception ex)
                {
                    return new ObjectResult(response) { StatusCode = StatusCodes.Status500InternalServerError };
                    //throw;
                }
                if (fileBytes == null)
                    return new ObjectResult(response) { StatusCode = StatusCodes.Status500InternalServerError };
                else {
                    return File(fileBytes, fichier.Fic_Type, "document.pdf");
                }
            }
            else
            {
                return NotFound();
            }
            
        }

        [HttpPut("{idActeGestion:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public ActionResult UpdateActeGestion(int idActeGestion, [FromForm] ActeGestionModificationDto acteGestion)
        {

            if (ModelState.IsValid)
            {
                ActeGestionModificationViewModel acteGestionModificationModel = _mapper.Map<ServiceMetier.ActeGestionModificationViewModel>(acteGestion);
                if (acteGestion.fichier != null && !acteGestion.fichier.FileName.Equals("") && acteGestion.fichier.FileName!=null)
                {
                    var ext = Path.GetExtension(acteGestion.fichier.FileName);
                    string[] ExtensionsAutorises = new string[] { ".pdf" };
                    ResponseDto resp = new ResponseDto()
                    {
                        Message = "Erreur Interne du serveur.",
                        IsSuccess = false
                    }; ;
                    // verif extension file
                    if (ExtensionsAutorises.Contains(ext))
                    {
                        // verif is directory  exists
                        if (Directory.Exists(DefaultDirectory))
                        {
                            try
                            {
                                using (var fileStream = new FileStream(Path.Combine(DefaultDirectory, DateTime.Now.ToString().Replace("/","").Replace(" ","").Replace(":", "")), FileMode.Create, FileAccess.Write))
                                {
                                    acteGestion.fichier.CopyTo(fileStream);
                                    string chemin = Path.Combine(DefaultDirectory, DateTime.Now.ToString().Replace("/","").Replace(" ","").Replace(":", ""));
                                    acteGestionModificationModel.cheminFichier = chemin;
                                    acteGestionModificationModel.typeFichier = acteGestion.fichier.ContentType;
                                }
                            }
                            catch (Exception ex)
                            {
                                return new ObjectResult(resp) { StatusCode = StatusCodes.Status500InternalServerError };
                            }
                        }
                        else
                        {
                            return new ObjectResult(resp) { StatusCode = StatusCodes.Status500InternalServerError };
                        }
                    }
                    else
                    {
                        resp.Message = "Ce type de fichier n'existe pas.";
                        return BadRequest(resp);
                    }
                }
                else
                {
                    acteGestionModificationModel.idFichier = 0;
                }

                var result = service.UpdateActeGestion(idActeGestion, acteGestionModificationModel);
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
        public ActionResult AddActeGestion([FromForm] ActeGestionCreationDto acteGestion)
        {

            if (ModelState.IsValid)
            {
                var acteGestionCreationViewModel = _mapper.Map<ServiceMetier.ActeGestionCreationViewModel>(acteGestion);
                var ext = Path.GetExtension(acteGestion.fichier.FileName);
                string[] ExtensionsAutorises = new string[] { ".pdf"};
                ResponseDto response = new ResponseDto()
                {
                    Message = "Erreur Interne du serveur.",
                    IsSuccess = false
                }; ;
                // verif extension file
                if (ExtensionsAutorises.Contains(ext))
                {
                    // verif is directory  exists
                    if (Directory.Exists(DefaultDirectory))
                    {
                        try
                        {
                            using (var fileStream = new FileStream(Path.Combine(DefaultDirectory, DateTime.Now.ToString().Replace("/","").Replace(" ","").Replace(":", "")), FileMode.Create, FileAccess.Write))
                            {
                                acteGestion.fichier.CopyTo(fileStream);
                            }
                        }catch(Exception ex)
                        {
                            return new ObjectResult(response) { StatusCode = StatusCodes.Status500InternalServerError };
                        }
                    }
                    else
                    {
                        return new ObjectResult(response) { StatusCode = StatusCodes.Status500InternalServerError };
                    }
                }
                else
                {
                    response.Message = "Ce type de fichier n'existe pas.";
                    return BadRequest(response);
                }

                string chemin = Path.Combine(DefaultDirectory, DateTime.Now.ToString().Replace("/","").Replace(" ","").Replace(":", ""));

                acteGestionCreationViewModel.cheminFichier = chemin;
                acteGestionCreationViewModel.typeFichier = acteGestion.fichier.ContentType;
                acteGestionCreationViewModel.idUtilisateurDeCreation = User.FindFirst("UserId").Value;
                var result = service.AddActeGestion(acteGestionCreationViewModel);

                response.Message = result ? Messages.AddSuccess : Messages.AddError;
                response.IsSuccess = result;
                response.Result = result;

                return result ? Ok(response) : new ObjectResult(response) { StatusCode = StatusCodes.Status500InternalServerError };
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }

}
