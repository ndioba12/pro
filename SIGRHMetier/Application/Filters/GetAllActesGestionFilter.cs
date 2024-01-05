using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIGRHMetier.Application.Filters
{
    public class GetAllActesGestionFilter
    {
        public string idFichePersonnel { get; set; }
        public String prenomFichePersonnel { get; set; } = string.Empty;
        public String nomFichePersonnel { get; set; } = string.Empty;
        public String codeTypeFichePersonnel { get; set; } = string.Empty;
        public DateTime dateCreationMax { get; set; }
        public DateTime dateCreationMin { get; set; }
        public DateTime dateEntreeEnVigueurMax { get; set; }
        public DateTime dateEntreeEnVigueurMin { get; set; }
        public String codeNatureDecision { get; set; } = string.Empty;
        public String codeTypeDocument { get; set; } = string.Empty;
        public String estValide { get; set; } = string.Empty;
        public String numeroDecision {  get; set; } = string.Empty;

    }
}