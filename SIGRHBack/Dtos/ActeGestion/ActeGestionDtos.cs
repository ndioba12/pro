using SIGRHBack.Database.Shared;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace SIGRHBack.Dtos.ActeGestion
{
    public class ActeGestionCreationDto
    {
        [Required(ErrorMessage = "L'id de la fiche de personnel concernée doit être spécifiée.")]
        public int idFichePersonnel { get; set; }

        [Required(ErrorMessage = "La date d'entrée en vigueur est un champ obligatoire.")]
        public DateTime dateEntreeEnVigueur { get; set; }

        [Required(ErrorMessage = "La nature de la décision est un champ obligatoire.")]
        [RegularExpression("^(" + CodesNatureDecision.autre + "|" + CodesNatureDecision.arrete + "|" +
            CodesNatureDecision.noteDeService + "|" + CodesNatureDecision.decret + ")$", ErrorMessage = "Cette nature de décision n'existe pas.")]
        public string codeNatureDecision { get; set; }

        [Required(ErrorMessage = "La numéro de décision est un champ obligatoire.")]
        [MaxLength(50, ErrorMessage = "Le numéro de décision ne peut pas dépasser 50 caractères.")]
        public string numeroDecision { get; set; }

        [Required(ErrorMessage = "Le type de document joint doit être spécifié.")]
        [RegularExpression("^(" + CodesTypeDocument.affectionAG + "|" + CodesTypeDocument.congeAnnuelAG + "|" +
        CodesTypeDocument.congeExamenAG + "|" + CodesTypeDocument.congeMaladieAG + CodesTypeDocument.congeMaterniteAG + "|" +
        CodesTypeDocument.miseADispositionAG + ")$", ErrorMessage = "Ce type de décision n'existe pas.")]
        public string codeTypeDocument { get; set; }
        [Required(ErrorMessage = "Un fichier doit obligatoirement être joint.")]
        public IFormFile fichier { get; set; }

        //[Required(ErrorMessage = "Le chemin du fichier doit être spécifié.")]
        //[MaxLength(1000, ErrorMessage = "Le chemin du fichier ne peut pas dépasser 1000 caractères.")]
        //public string cheminFichier { get; set; }

        //[Required(ErrorMessage = "Le type de fichier doit être spécifié.")]
        //[MaxLength(50, ErrorMessage = "Le type de fichier ne peut pas dépasser 50 caractères.")]
        //public string typeFichier { get; set; }

    }

    public class ActeGestionModificationDto
    {
        [Required(ErrorMessage = "La date d'entrée en vigueur est un champ obligatoire.")]
        public DateTime dateEntreeEnVigueur { get; set; }
        [Required(ErrorMessage = "La nature de la décision est un champ obligatoire.")]
        [RegularExpression("^(" + CodesNatureDecision.autre + "|" + CodesNatureDecision.arrete + "|" +
            CodesNatureDecision.noteDeService + "|" + CodesNatureDecision.decret + ")$", ErrorMessage = "Cette nature de décision n'existe pas.")]
        public string codeNatureDecision { get; set; }

        [Required(ErrorMessage = "La numéro de décision est un champ obligatoire.")]
        [MaxLength(50, ErrorMessage = "Le numéro de décision ne peut pas dépasser 50 caractères.")]
        [MinLength(1, ErrorMessage = "Le numéro de décision ne peut pas être vide.")]
        public string numeroDecision { get; set; }

        [Required(ErrorMessage = "Le type de document joint doit être spécifié.")]
        [RegularExpression("^(" + CodesTypeDocument.affectionAG + "|" + CodesTypeDocument.congeAnnuelAG + "|" +
            CodesTypeDocument.congeExamenAG + "|" + CodesTypeDocument.congeMaladieAG + CodesTypeDocument.congeMaterniteAG + "|" + CodesTypeDocument.miseADispositionAG + ")$", ErrorMessage = "Ce type de décision n'existe pas.")]
        public string codeTypeDocument { get; set; }

        [Required(ErrorMessage = "L'identifiant du fichier à remplacer doit être spécifiée.")]
        public int idFichier { get; set; }

        public IFormFile? fichier { get; set; }

    }

    public class ActeGestionConsultationDto
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
