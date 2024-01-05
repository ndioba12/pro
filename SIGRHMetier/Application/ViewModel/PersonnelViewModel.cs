using Microsoft.AspNetCore.Http;
using System;

namespace SIGRHMetier.Application.ViewModel
{
	public class PersonnelViewModel
	{
		public int Id { get; set; }
		public string Nom { get; set; }
		public string Prenom { get; set; }
		public DateTime DateNaissance { get; set; }
		public string LieuNaissance { get; set; }
		public string Sexe { get; set; }
		public string SituationMatrimoniale { get; set; }
		public string NumeroCNI { get; set; }
		public string Promotion { get; set; }
		public DateTime DateEntreVigueur { get; set; }
		public int RangExamen { get; set; }
		public string commune { get; set; }
		public string Quartier { get; set; }
		public string Telephone { get; set; }
		public string Email { get; set; }
		public string Matricule { get; set; }
		public string Emploi { get; set; }
		public string JuridictionF { get; set; }
		public IFormFile Photo { get; set; }
		public string Fonction { get; set; }
		public string echelon { get; set; }
		public int? ConjointMagistratOuiNON { get; set; }
		public string NomConjoint { get; set; }
		public string PrenomConjoint { get; set; }
		public string MatriculeConjoint { get; set; }
		public DateTime DateCreation { get; set; }
		public int UserCreation { get; set; }
		public string TypePersonnel { get; set; }
		public DateTime? DateValidation { get; set; }
		public DateTime? DateSoumission { get; set; }
		public string JuridictionE { get; set; }
		public int UserValidation { get; set; }
		public string Position { get; set; }
		public int EstValide { get; set; }
		public long Reference { get; set; }
		public string Motif { get; set; }
		public string Groupe { get; set; }
		public string GradeM { get; set; }
		public string GradeF { get; set; }
		public string Corps { get; set; }
		public string Departement { get; set; }
		public string Region { get; set; }

	}

	public class RegistreViewModel
	{
		public int Id { get; set; }
		public string Nom { get; set; }
		public string Prenom { get; set; }	
		public string Grade { get; set; }
		public string Juridiction { get; set; }
		public string MatriculeSolde { get; set; }
		public string Fonction { get; set; }
		public string TypePersonnel { get; set; }
		
	}

	public class GradeViewModel
	{
		public int Id { get; set; }
		public string Libelle { get; set; }
		public string TypePersonnel { get; set; }

	}
	
}