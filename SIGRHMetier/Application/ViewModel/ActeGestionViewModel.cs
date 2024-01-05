using System;
using System.ComponentModel.DataAnnotations;

namespace SIGRHMetier.Application.ViewModel
{
    public class ActeGestionCreationViewModel
    {
        public int idFichePersonnel { get; set; }

        public string idUtilisateurDeCreation { get; set; }

        public DateTime dateEntreeEnVigueur { get; set; }

        public string codeNatureDecision { get; set; }

        public string numeroDecision { get; set; }

        public string codeTypeDocument { get; set; }

        public string cheminFichier { get; set; }

        public string typeFichier { get; set; }

    }

    public class ActeGestionModificationViewModel
    {
        public DateTime dateEntreeEnVigueur { get; set; }
        
        public string codeNatureDecision { get; set; }

        public string numeroDecision { get; set; }

        public string codeTypeDocument { get; set; }

        public string cheminFichier { get; set; }

        public string typeFichier { get; set; }

        public int idFichier { get; set; }

    }

    public class ActeGestionConsultationViewModel
    {
        public int id { get; set; }
        public int idFichePersonnel { get; set; }
        public string NomFichePersonnel { get; set; }
        public string PrenomFichePersonnel { get; set; }
        public string TypeFichePersonnel { get; set; }
        public int idUtilisateurDeCreation { get; set; }
        public string EmailUtilisateurDeCreation { get; set; }
        public string NomUtilisateurDeCreation { get; set; }
        public string PrenomUtilisateurDeCreation { get; set; }
        public int idUtilisateurDeValidation { get; set; }
        public string EmailUtilisateurDeValidation { get; set; }
        public string NomUtilisateurDeValidation { get; set; }
        public string PrenomUtilisateurDeValidation { get; set; }
        public DateTime? dateCreation { get; set; }
        public DateTime? dateModification { get; set; }
        public DateTime? dateSoumission { get; set; }
        public DateTime? dateEntreeEnVigueur { get; set; }
        public DateTime? dateValidation { get; set; }
        public int? estValide { get; set; }
        public string codeNatureDecision { get; set; }
        public string libelleNatureDecision { get; set; }
        public string numeroDecision { get; set; }
        public string codeTypeDocument { get; set; }
        public string libelleTypeDocument { get; set; }
        public string cheminFichier { get; set; }
        public string typeFichier { get; set; }
        public int idFichier { get; set; }
    }

}