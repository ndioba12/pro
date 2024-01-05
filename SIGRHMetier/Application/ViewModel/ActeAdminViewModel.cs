using System;

namespace SIGRHMetier.ViewModel
{
    public class ActeAdminViewModel
    {
        public int Id { get; set; }
        public string Residence { get; set; }
        public string CodeEchelon { get; set; }
        public string CodeEmploi { get; set; }
        public string CodeFonction { get; set; }
        public string CodeJurEmploi { get; set; }
        public string CodeJurFonction { get; set; }
        public string NumeroDecision { get; set; }
        public DateTime? DateCreation { get; set; }
        public string CodeNatureDecison{ get; set; }
        public int? EstValide { get; set; }
        public DateTime? DateValidation { get; set; }
        public DateTime? DateSoumission { get; set; }
        public DateTime? DateModification { get; set; }
        public string CodeTypeDocument { get; set; }
        public int? IdFichier { get; set; }
        public int? IdFichePersonnelJudiciaire { get; set; }
        public int? IdUtiCreation { get; set; }
        public int? IdUtiValidation { get; set; }
        public string CodeGroupe { get; set; }
        public string CodeGradeMagistrat { get; set; }
        public string CodeGradeFonctionnaire { get; set; }
        public string CodeCorpsJudiciaire { get; set; }
        public string CodeIndice { get; set; }
    }
}