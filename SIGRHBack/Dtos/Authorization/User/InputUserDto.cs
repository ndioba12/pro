using SIGRHBack.Database.Shared;
using System.ComponentModel.DataAnnotations;

namespace SIGRHBack.Dtos.Authorization.User
{
    public class InputUserDto
    {
        [Required]
        [MaxLength(80)]
        public string? Nom { get; set; }
        [Required]
        [MaxLength(150)]
        public string? Prenom { get; set; }
        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        [MaxLength(80)]
        public string? Identifiant { get; set; }
        [Required]
        [
         RegularExpression("^(" + UserRoleNames.Admin + "|" + UserRoleNames.DirecteurAssistantServicesJudiciaires + "|" +
         UserRoleNames.SecretaireGeneralMinistereJustice + "|" + UserRoleNames.OperateurSaisie + "|" +
         UserRoleNames.ChefPersonnel + "|" + UserRoleNames.DirecteurServicesJudiciaires + ")$", ErrorMessage = "Ce profil n'existe pas.")
        ]
        public string? Profil { get; set; }
        [Required]
        [MaxLength(15)]
        public string? Telephone { get; set; }
        [Required]
        [MaxLength(150)]
        public string? Poste { get; set; }
        [Required]
        [MaxLength(200)]
        public string? Adresse { get; set; }
        public string ActifOuiNon { get; set; } = "1";
        public DateTime? DateCreation { get; set; } = DateTime.UtcNow;  
    }
}
