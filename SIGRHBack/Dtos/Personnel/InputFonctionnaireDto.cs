using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SIGRHBack.Dtos.Personnel
{
    public class InputFonctionnaireDto
    {
        [Required]
        [JsonPropertyName("nom")]
        public string? Nom { get; set; }
        [Required]
        public string? Prenom { get; set; }
        [Required]
        public DateTime DateNaissance { get; set; }
        [Required]
        public string? LieuNaissance { get; set; }
        [Required]
        public string? Sexe { get; set; }
        [Required]
        public string? SituationMatrimoniale { get; set; }
        [Required]
        public string? Cni { get; set; }
        [Required]
        public string? Promotion { get; set; }        
        [Required]
        public string? Grade { get; set; }        
        [Required]
        public string? Echelon { get; set; }
        [Required]
        public string? commune { get; set; }
        [Required]
        public string? Quartier { get; set; }
        [Required]
        public string? Telephone { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? MatriculeSolde { get; set; }
        [Required]
        public string? JuridictionF { get; set; }
        //[Required]
        public IFormFile? Fichier { get; set; }
        public int IdPhoto { get; set; }
        [Required]
        public string? Fonction { get; set; }
        [Required]
        public string? TypePersonnel { get; set; }
        [Required]
        public string? Corps { get; set; }
        [Required]
        public string? Departement { get; set; }
        [Required]
        public string? Region { get; set; }
        [Required]
        public string? RegionJ { get; set; }
        public long? Reference { get; set; }

        public int? ConjointMagistratOuiNON { get; set; }
        public string? MatriculeConjoint { get; set; }
        public int? UserValidation { get; set; }
        public int? UserCreation { get; set; }
        public DateTime? DateSoumission { get; set; }
        public string? Position { get; set; }
        public int? EstValide { get; set; }  = 0;
    }
}
