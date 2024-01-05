using SIGRHMetier.Application.Filters;
using System;

namespace SIGRHMetier
{
    public class GetAllTypeDocumentFilter : FilterCommun
    {
        public string MagistratOuiNon { get; set; }
        public string FonctionnaireJudiciaireOuiNon { get; set; }
        public string CodeTypeActe { get; set; }
        public Nullable<int> Echeance { get; set; } = default(Nullable<int>);
        public Nullable<int> AlerteRouge { get; set; } = default(Nullable<int>);
        public Nullable<int> AlerteJaune { get; set; } = default(Nullable<int>);
        public string Mouvement { get; set; } = string.Empty;
    }
}