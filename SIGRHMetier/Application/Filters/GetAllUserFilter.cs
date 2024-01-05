using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIGRHMetier.Application.Filters
{
    public class GetAllUserFilter
    {
        public String Prenom { get; set; } = string.Empty;
        public String Nom { get; set; } = string.Empty;
        public String Statut { get; set; } = string.Empty;
        public String Identifiant { get; set; } = String.Empty;
        public String Profil { get; set; } = string.Empty;
        public DateTime DateCreation { get; set; }
    }
}