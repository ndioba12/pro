using System;

namespace SIGRHMetier.Application.ViewModel
{
    public class ActeAdminViewModel
    {
        public int Id { get; set; }
        public string LibelleTypeDocument { get; set; }
        public string Grade { get; set; }
        public string LibelleGroupe { get; set; }
        public string LibelleEchelon { get; set; }
        public long? ValeurIndice { get; set; }
        public DateTime? DateCreation { get; set; }
        public DateTime? DateSoumission { get; set; }   

    }

        public class AddOrUpdatecteAdminViewModel
        {
            public int Id { get; set; }
            public string CodeTypeDocument { get; set; }
            public string CodeNatureDecision { get; set; }
            public string NumeroDecision { get; set; }
            public string CodeGradeM { get; set; }
            public string CodeGradeF { get; set; }
            public string CodeGroupe { get; set; }
            public string CodeEchelon { get; set; }
            public long? ValeurIndice { get; set; }
            public string CodeEmploiJudiciaire { get; set; }
            public string CodeJuridictionEmploi { get; set; }
            public string CodeFonctionJudiciaire { get; set; }
            public string CodeJuridictionFonction { get; set; }
            public DateTime? DateCreation { get; set; }
            public string Residence { get; set; }       
            //public string CheminFichier { get; set; }
            //public string TypeFichier { get; set; }
           // public int IdFichier { get; set; }
            public int IdFichePersonnelJudiciaire { get; set; }
            public int IdUtilisateurCreation { get; set; }
           // public string IdUserCreation { get; set; }


        }

    }
