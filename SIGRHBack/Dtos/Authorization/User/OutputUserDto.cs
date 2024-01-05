using System.ComponentModel.DataAnnotations;

namespace SIGRHBack.Dtos.Authorization.User
{
    public class OutputUserDto
    {
        public string? IdUtilisateur { get; set; }
        public string? NomUtilisateur { get; set; }
        public string? PrenomUtilisateur { get; set; }
        public string? EmailUtilisateur { get; set; }
        public string? Identifiant { get; set; }
        public string? CodeProfil { get; set; }
        public string? LibelleProfil { get; set; }
        public string? Telephone { get; set; }
        public string? Poste { get; set; }
        public string? Adresse { get; set; }
        public string ActifOuiNon { get; set; } = "1";
        public DateTime? DateCreation { get; set; } = DateTime.UtcNow;
    }
}
