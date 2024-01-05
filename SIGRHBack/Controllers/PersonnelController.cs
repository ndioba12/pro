using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIGRHBack.Database.Shared;
using SIGRHBack.Dtos.Personnel;
using SIGRHBack.Dtos.Shared;
using SIGRHBack.Helpers;
using SIGRHBack.Ressources;
using SIGRHBack.Services.Personnel;
using System.Text;

namespace SIGRHBack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PersonnelController : ControllerBase
    {
        private readonly IPersonnelService _servicePersonnel;
        private readonly IMapper _mapper;
        private ServiceMetier.Service1Client service = new();
        private ServiceFile.Service1Client serviceFichier = new();

        public string[] ExtensionsAutorises = new string[] { ".png", ".jpg", ".jpeg" };
        public PersonnelController(IPersonnelService servicePersonnel, IMapper mapper)
        {
            _servicePersonnel = servicePersonnel;
            _mapper = mapper;
        }
        // GET: PersonnelController

        [HttpGet("registre")]
        [ProducesResponseType(typeof(ResponseDto<IEnumerable<ServiceMetier.RegistreViewModel>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> getRegistrePersonnel([FromQuery] ServiceMetier.GetAllRegistreFilter filter)
        {

            var result = service.RegistrePersonnel(filter);
            return Ok(result);

        }

        [HttpPost]
        [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> AddMagistrat([FromBody] ServiceMetier.PersonnelViewModel pers)
        {

            if (ModelState.IsValid)
            {
                var result = await _servicePersonnel.AddMagistrat(pers);
                if (result == null)
                {
                    return BadRequest(result);
                }
                return Ok(result);

            }
            return BadRequest(ModelState);
        }
        [HttpDelete("{id:int}")]
        [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> DeleteFiche(int id)
        {
            return Ok();
        }
        [HttpPost("fonctionnaire")]
        [DisableRequestSizeLimit]
        [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> AddFonctionnaire([FromForm] InputFonctionnaireDto fonctionnaire)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = User;
                    var userId = int.Parse(user.FindFirst("Id")?.Value);
                    // verif file
                    if (fonctionnaire.Fichier is null || fonctionnaire.Fichier is not IFormFile)
                    {
                        return Ok(new ResponseDto
                        {
                            Message = Messages.ErrorPhotoRequired,
                            IsSuccess = false
                        });
                    }
                    var ext = Path.GetExtension(fonctionnaire.Fichier.FileName);
                    var size5mb = 5 * 1024 * 1024;
                    var personnel = _mapper.Map<ServiceMetier.TD_FichePersonnelJudiciaire>(fonctionnaire);

                    // verif extension, size and format  file photo en byte
                    if (fonctionnaire.Fichier.Length > size5mb)
                    {
                        return Ok(new ResponseDto
                        {
                            Message = Messages.ErrorFileLarge,
                            IsSuccess = false
                        });
                    }
                    // verif extensions
                    if (!ExtensionsAutorises.Contains(ext))
                    {
                        return Ok(new ResponseDto
                        {
                            Message = Messages.ErrorextensionFile,
                            IsSuccess = false
                        });
                    }
                    // verif doublon matricule
                    if (service.VerifFicheByMatricule(fonctionnaire.MatriculeSolde) == true)
                    {
                        return Ok(new ResponseDto
                        {
                            Message = Messages.ErrorMatriculeExist,
                            IsSuccess = false
                        });
                    }
                    // verif doublon email
                    if (service.VerifFicheByEmail(fonctionnaire.Email) == true)
                    {
                        return Ok(new ResponseDto
                        {
                            Message = Messages.ErrorEmailExist,
                            IsSuccess = false
                        });
                    }
                    // verif doublon telephone
                    if (service.VerifFicheByTelephone(fonctionnaire.Telephone) == true)
                    {
                        return Ok(new ResponseDto
                        {
                            Message = Messages.ErrorTelephoneExist,
                            IsSuccess = false
                        });
                    }
                    // verif doublon cni
                    if (service.VerifFicheByCni(fonctionnaire.Cni) == true)
                    {
                        return Ok(new ResponseDto
                        {
                            Message = Messages.ErrorCniExist,
                            IsSuccess = false
                        });
                    }
                    // enregistre le fichier
                    var photo = await Utils.ConvertToBytes(fonctionnaire.Fichier);
                    var photoBase64 = Convert.ToBase64String(photo);

                    var newPhotoSignature = new ServiceMetier.TD_Signature_Photo
                    {
                        SIP_contentype = fonctionnaire.Fichier.ContentType,
                        SIP_FileSave = photo,
                        SIP_Name = fonctionnaire.Fichier.FileName
                    };
                    var addedPhoto = service.AddSignaturePhoto(newPhotoSignature);
                    if (addedPhoto == null)
                        return Ok(new ResponseDto
                        {
                            Message = Messages.ErrorAddedPhoto,
                            IsSuccess = false
                        });
                    personnel.FPJ_SIP_Id = addedPhoto.SIP_Id;

                    // affectation de l'identifiant utilisateur
                    personnel.FPJ_Uti_Creation_Id = userId;

                    // validate automatically a fiche where other operateur saisie
                    if (User.IsInRole(UserRoleNames.OperateurSaisie))
                    {
                        personnel.FPJ_EstValide = 0;
                    }
                    else
                    {
                        personnel.FPJ_EstValide = 1;
                    }
                    // add fiche fonctionnaire
                    var result = await service.AddFicheFonctionnaireAsync(personnel);
                    if (result == false)
                    {
                        return Ok(new ResponseDto
                        {
                            Message = Messages.ErrorCreateFiche,
                            IsSuccess = false
                        });
                    }
                    return Ok(new ResponseDto
                    {
                        Message = Messages.SuccessCreateFiche,
                        IsSuccess = true
                    });
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
            return BadRequest(ModelState);
        }

        [HttpPut("fonctionnaire/{id:int}")]
        [DisableRequestSizeLimit]
        [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateFicheFonctionnaire(int id, [FromForm] InputFonctionnaireDto fonctionnaire)
        {
            if (ModelState.IsValid)
            {
                var mappedFiche = _mapper.Map<ServiceMetier.TD_FichePersonnelJudiciaire>(fonctionnaire);
                // update photo if edit
                if (fonctionnaire.Fichier != null )
                {
                    // enregistre le fichier
                    var photo = await Utils.ConvertToBytes(fonctionnaire.Fichier);
                    var photoBase64 = Convert.ToBase64String(photo);

                    var newPhotoSignature = new ServiceMetier.TD_Signature_Photo
                    {
                        SIP_contentype = fonctionnaire.Fichier.ContentType,
                        SIP_FileSave = photo,
                        SIP_Name = fonctionnaire.Fichier.FileName
                    };
                    // q 
                    var signatureUpdate = service.UpdateSignaturePhoto(mappedFiche.FPJ_SIP_Id ?? 1000, newPhotoSignature);
                    if (signatureUpdate == false)
                    {
                        return Ok(new ResponseDto
                        {
                            Message = Messages.ErrorUpdatedSignaturePhoto,
                            IsSuccess = false
                        });
                    }
                }
                var result = await service.UpdateFicheAsync(id, mappedFiche);
                if (result == true)
                    return Ok(new ResponseDto
                    {
                        Message = Messages.SuccessUpdateFiche,
                        IsSuccess = true
                    });
                return Ok(new ResponseDto
                {
                    Message = Messages.ErrorUpdateFiche,
                    IsSuccess = false
                });
            }
            return BadRequest(ModelState);
        }

        [HttpPut("valider/{id:int}")]
        [DisableRequestSizeLimit]
        [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> ValiderFiche(int id)
        {
            var userId = int.Parse(User.FindFirst("Id")?.Value);
            var result = await service.ValiderFicheAsync(id, userId);
            if (result == true)
                return Ok(new ResponseDto
                {
                    IsSuccess = true,
                    Message = Messages.SuccessValidateFiche
                });
            return Ok(new ResponseDto
            {
                IsSuccess = true,
                Message = Messages.ErrorValidateFiche
            });
        }

  //      [HttpGet("fonctionnaire/{id:int}", Name = "GetFicheFonctionnaireById")]
		//[DisableRequestSizeLimit]
  //      [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status201Created)]
  //      [ProducesResponseType(StatusCodes.Status400BadRequest)]
  //      public ActionResult GetOneFicheFonctionnaire(int id)
  //      {
		//	return Ok(service.GetOneFiche(id));
  //      }

        [HttpGet("fonctionnaire/{id:int}", Name = "GetFicheFonctionnaireById")]
        [DisableRequestSizeLimit]
        [ProducesResponseType(typeof(ServiceMetier.TD_FichePersonnelJudiciaire), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult GetOneFicheFonctionnaire(int id)
        {
            return Ok(service.GetOneFiche(id));
        }
        [HttpGet("fonctionnaire/detail/{id:int}", Name = "GetFicheDetailFonctionnaireById")]
        [DisableRequestSizeLimit]
        [ProducesResponseType(typeof(ServiceMetier.FichePersonnelViewModel), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult GetOneFicheDetailFonctionnaire(int id)
        {
            return Ok(service.GetFonctionnaireDetail(id));
        }

    }
}
