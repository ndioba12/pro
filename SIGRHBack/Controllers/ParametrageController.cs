using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SIGRHBack.Dtos.Shared;
using SIGRHBack.Ressources;

namespace SIGRHBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParametrageController : ControllerBase
    {

        private ServiceMetier.Service1Client service = new ServiceMetier.Service1Client();

        [HttpGet("corpsjudiciaire")]
        [ProducesResponseType(typeof(ResponseDto<IEnumerable<ServiceMetier.TP_CorpsJudiciaire>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status500InternalServerError)]
        public ActionResult GetAllCorpsJudiciaire([FromQuery] ServiceMetier.GetAllCorpsFilter filter)
        {
            return GetAllHandle(() => service.GetAllCorpsJudiciaire(filter));
        }

        [HttpGet("classejuridiction")]
        [ProducesResponseType(typeof(ResponseDto<IEnumerable<ServiceMetier.TP_ClasseJuridiction>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status500InternalServerError)]
        public ActionResult GetAllClasseJuridiction([FromQuery] ServiceMetier.GetAllClasseJuridictionFilter filter)
        {
            return GetAllHandle(() => service.GetAllClasseJuridiction(filter));
        }

        [HttpGet("echelon")]
        [ProducesResponseType(typeof(ResponseDto<IEnumerable<ServiceMetier.TP_Echelon>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status500InternalServerError)]
        public ActionResult GetAllEchelon([FromQuery] ServiceMetier.GetAllEchelonFilter filter)
        {
            return GetAllHandle(() => service.GetAllEchelon(filter));
        }

        [HttpGet("naturedecision")]
        [ProducesResponseType(typeof(ResponseDto<IEnumerable<ServiceMetier.TP_NatureDecision>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status500InternalServerError)]
        public ActionResult GetAllNatureDecision([FromQuery] ServiceMetier.GetAllNatureDecisionFilter filter)
        {
            return GetAllHandle(() => service.GetAllNatureDecision(filter));
        }
        [HttpGet("fonctionfonctionnaire")]
        [ProducesResponseType(typeof(ResponseDto<IEnumerable<ServiceMetier.TP_FonctionFonctionnaireJustice>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status500InternalServerError)]
        public ActionResult GetAllFonctionFonctionnaire([FromQuery] ServiceMetier.GetAllFonctionFonctionnaireFilter filter)
        {
            return GetAllHandle(() => service.GetAllFonctionFonctionnaire(filter));
        }
        [HttpGet("grademagistrat")]
        [ProducesResponseType(typeof(ResponseDto<IEnumerable<ServiceMetier.TP_GradeMagistrat>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status500InternalServerError)]
        public ActionResult GetAllGroupeMagistrat([FromQuery] ServiceMetier.GetAllGradeMagistratFilter filter)
        {
            return GetAllHandle(() => service.GetAllGradeMagistrat(filter));
        }

        [HttpGet("fonction")]
        [ProducesResponseType(typeof(ResponseDto<IEnumerable<ServiceMetier.TP_Fonction>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status500InternalServerError)]
        public ActionResult GetAllFonction([FromQuery] ServiceMetier.GetAllFonctionFilter filter)
        {
            return GetAllHandle(() => service.GetAllFonction(filter));
        }

        [HttpGet("emploijudiciaire")]
        [ProducesResponseType(typeof(ResponseDto<IEnumerable<ServiceMetier.TP_EmploiJudiciaire>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status500InternalServerError)]
        public ActionResult GetAllEmploiJudiciaire([FromQuery] ServiceMetier.GetAllEmploiJudiciaireFilter filter)
        {
            return GetAllHandle(() => service.GetAllEmploiJudiciaire(filter));
        }

        [HttpGet("indice")]
        [ProducesResponseType(typeof(ResponseDto<IEnumerable<ServiceMetier.TP_Indice>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status500InternalServerError)]
        public ActionResult GetAllIndice([FromQuery] ServiceMetier.GetAllIndiceFilter filter)
        {
            return GetAllHandle(() => service.GetAllIndice(filter));
        }

        [HttpGet("typedocument")]
        [ProducesResponseType(typeof(ResponseDto<IEnumerable<ServiceMetier.TP_TypeDocument>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status500InternalServerError)]
        public ActionResult GetAllTypeDocument([FromQuery] ServiceMetier.GetAllTypeDocumentFilter filter)
        {
            return GetAllHandle(() => service.GetAllTypeDocument(filter));
        }

        [HttpGet("typejuridiction")]
        [ProducesResponseType(typeof(ResponseDto<IEnumerable<ServiceMetier.TP_TypeJuridiction>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status500InternalServerError)]
        public ActionResult GetAllTypeJuridiction([FromQuery] ServiceMetier.GetAllTypeJuridctionFilter filter)
        {
            return GetAllHandle(() => service.GetAllTypeJuridiction(filter));
        }

        [HttpGet("juridiction")]
        [ProducesResponseType(typeof(ResponseDto<IEnumerable<ServiceMetier.JuridictionViewModel>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status500InternalServerError)]
        public ActionResult GetAllJuridiction([FromQuery] ServiceMetier.GetAllJuridictionFilter filter)
        {
            var result =  GetAllHandle(() => service.GetAllJuridiction(filter));
            return result;
        }

        [HttpGet("groupe")]
        [ProducesResponseType(typeof(ResponseDto<IEnumerable<ServiceMetier.TP_Groupe>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status500InternalServerError)]
        public ActionResult GetAllGroupe([FromQuery] ServiceMetier.GetAllGroupeFilter filter)
        {
            return GetAllHandle(() => service.GetAllGroupe(filter));
        }

        [HttpGet("gradefonctionnaire")]
        [ProducesResponseType(typeof(ResponseDto<IEnumerable<ServiceMetier.TP_GradeFonctionnaireJustice>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status500InternalServerError)]
        public ActionResult GetAllGradeFonctionnaire([FromQuery] ServiceMetier.GetAllGradeFoncJusticeFilter filter)
        {
            return GetAllHandle(() => service.GetAllGradeFoncJustice(filter));
        }

        [HttpGet("region")]
        [ProducesResponseType(typeof(ResponseDto<IEnumerable<ServiceMetier.TP_Region>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status500InternalServerError)]
        public ActionResult GetAllRegion([FromQuery] ServiceMetier.GetAllRegionFilter filter)
        {
            return GetAllHandle(() => service.GetAllRegion(filter));
        }

        [HttpGet("departement")]
        [ProducesResponseType(typeof(ResponseDto<IEnumerable<ServiceMetier.TP_Departement>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status500InternalServerError)]
        public ActionResult GetAllDepartement([FromQuery] ServiceMetier.GetAllDepartementFilter filter)
        {
            return GetAllHandle(() => service.GetAllDepartement(filter));
        }

        [HttpGet("commune")]
        [ProducesResponseType(typeof(ResponseDto<IEnumerable<ServiceMetier.TP_Commune>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status500InternalServerError)]
        public ActionResult GetAllCommune([FromQuery] ServiceMetier.GetAllCommuneFilter filter)
        {
            return GetAllHandle(() => service.GetAllCommune(filter));
        }

        [HttpGet("quartier")]
        [ProducesResponseType(typeof(ResponseDto<IEnumerable<ServiceMetier.TP_Quartier>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status500InternalServerError)]
        public ActionResult GetAllQuartier([FromQuery] ServiceMetier.GetAllQuartierFilter filter)
        {
            return GetAllHandle(() => service.GetAllQuartier(filter));
        }

        [HttpGet("typepersonnel")]
        [ProducesResponseType(typeof(ResponseDto<IEnumerable<ServiceMetier.TP_TypePersonnel>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status500InternalServerError)]
        public ActionResult GetAllTypePersonnel([FromQuery] ServiceMetier.GetAllTypePersonnelFilter filter)
        {
            return GetAllHandle(() => service.GetAllTypePersonnel(filter));
        }

        [HttpGet("modepassage")]
        [ProducesResponseType(typeof(ResponseDto<IEnumerable<ServiceMetier.TP_ModePassage>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status500InternalServerError)]
        public ActionResult GetAllModePassage([FromQuery] ServiceMetier.GetAllModePassageFilter filter)
        {
            return GetAllHandle(() => service.GetAllModePassage(filter));
        }
        // méthode générique pour appeler toutes les méthodes "getAll" concernant les tables de paramétrage.
        protected ActionResult GetAllHandle<T>(Func<T> methode)
        {
            try
            {
                var response = new ResponseDto<T>()
                {
                    IsSuccess = true,
                    Result = methode()
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
    }
}
