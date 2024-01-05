using SIGRHMetier.Application.Filters;
using System;

namespace SIGRHMetier
{
    public class GetAllEchelonFilter : FilterCommun
    {
        public Nullable<int> Duree { get; set; } = default(Nullable<int>);
        public string CodeTypePersonnel { get; set; } = string.Empty;
        public string CodeGroupe { get; set; } = string.Empty;
        public string CodeGrade { get; set; } = string.Empty;
        public string CodeModePassage { get; set; } = string.Empty;
        public Nullable<int> IdIndice { get; set; } = default(Nullable<int>);
    }
}