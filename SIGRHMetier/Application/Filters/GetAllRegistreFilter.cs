using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIGRHMetier.Application.Filters
{
	public class GetAllRegistreFilter
	{
		public String TypePersonnel { get; set; } = string.Empty;
		public String Prenom { get; set; } = string.Empty;
		public String Nom { get; set; } = string.Empty;
		public String Matricule { get; set; } = string.Empty;
		public String Grade { get; set; } = String.Empty;
		public String Juridiction { get; set; } = string.Empty;
		public String Fonction { get; set; } = string.Empty;
		public String MatriculeSolde { get; set; } = string.Empty;
		public String Statut { get; set; } = string.Empty;
	}
}