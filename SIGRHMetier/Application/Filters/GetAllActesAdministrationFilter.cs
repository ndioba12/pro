using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIGRHMetier.Application.Filters
{
    public class GetAllActesAdministrationFilter
    {
        //  public String CodeTypeFichePersonnel { get; set; } = string.Empty;
        public String CodeTypeDocument { get; set; } = string.Empty;
        public String CodeGroupe { get; set; } = string.Empty;
        public long ValeurIndice { get; set; } = 0;
        public String CodeEchelon { get; set; } = string.Empty;
        public String CodeGradeM { get; set; } = string.Empty;
        public String CodeGradeF { get; set; } = string.Empty;
        public String CodeNatureDecision { get; set; } = string.Empty;
        public DateTime DateCreation { get; set; }
        public DateTime DateSoummission { get; set; }

    }
}