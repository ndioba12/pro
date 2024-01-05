using System;

namespace SIGRHMetier.Application.ViewModel
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Email { get; set; }
        public string Adresse { get; set; }
        public string Login { get; set; }
        public string Poste { get; set; }
        public string IdUser { get; set; }
        public string Telephone { get; set; }
        public DateTime DateCreation { get; set; }
        public string LibelleProfil { get; set; }
        public string CodeProfil { get; set; }
        public int IdProfil { get; set; }
        public string Statut { get; set; }
    }
}