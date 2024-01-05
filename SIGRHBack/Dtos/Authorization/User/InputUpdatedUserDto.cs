using System.ComponentModel.DataAnnotations;
using SIGRHBack.Database.Shared;

namespace SIGRHBack.Dtos.Authorization.User
{
    public class InputUpdatedUserDto : InputUpdatedAccountDto
    {
        [Required(ErrorMessage = "Le profil est un champs requis.")]
        [
         RegularExpression("^("+UserRoleNames.Admin+"|"+ UserRoleNames.DirecteurAssistantServicesJudiciaires + "|" +
         UserRoleNames.SecretaireGeneralMinistereJustice + "|" + UserRoleNames.OperateurSaisie + "|" +
         UserRoleNames.ChefPersonnel + "|" + UserRoleNames.DirecteurServicesJudiciaires + "|" + UserRoleNames.Revoquer + ")$", ErrorMessage ="Ce profil n'existe pas.")
        ]
        public string? Profil { get; set; }
    }

    public class InputUpdatedAccountDto
    {
        [Required(ErrorMessage = "Le nom de famille est un champs requis.")]
        [MaxLength(50, ErrorMessage = "le nom de famille ne peut pas dépasser 50 caractères.")]
        public string? NomUtilisateur { get; set; }

        [Required(ErrorMessage = "Le prénom est un champs requis.")]

        [MaxLength(100, ErrorMessage = "le prénom ne peut pas dépasser 100 caractères.")]
        public string? PrenomUtilisateur { get; set; }

        [Required(ErrorMessage = "Le numéro de téléphone est un champs requis.")]
        [MaxLength(15, ErrorMessage = "le numéro de téléphone ne peut pas dépasser 15 caractères.")]
        [RegularExpression(@"^\d{9}$", ErrorMessage ="Le format du numéro de téléphone saisi est invalide")]
        public string? Telephone { get; set; }

        [Required(ErrorMessage = "Le poste est un champs requis.")]
        [MaxLength(50, ErrorMessage = "Le libellé du poste ne peut pas dépasser 50 caractères.")]
        public string? Poste { get; set; }

        [Required(ErrorMessage = "L'adresse est un champs requis.")]
        [MaxLength(150, ErrorMessage = "L'adresse ne peut pas dépasser 150 caractères.")]
        public string? Adresse { get; set; }
    }

    

}
