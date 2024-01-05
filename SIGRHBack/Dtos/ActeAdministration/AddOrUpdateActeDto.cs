using System.ComponentModel.DataAnnotations;

namespace SIGRHBack.Dtos.ActeAdministration
{
    public class AddOrUpdateActeDto
    {
        [MaxLength(4)]
        public string CodeTypeDocument { get; set; }

        [MaxLength(4)]  
        public string CodeNatureDecision { get; set; }

        [MaxLength(50)]
        public string NumeroDecision { get; set; }

        [MaxLength(4)]
        public string CodeGradeM { get; set; }

        [MaxLength(4)]
        public string CodeGradeF { get; set; }

        [MaxLength(4)]
        public string CodeGroupe { get; set; }
       
        [MaxLength(4)]
        public string CodeEchelon { get; set; }

        public long ValeurIndice { get; set; }

        [MaxLength(4)]
        public string CodeEmploiJudiciaire { get; set; }

        [MaxLength(4)]
        public string CodeJuridictionEmploi { get; set; }

        [MaxLength(4)]
        public string CodeFonctionJudiciaire { get; set; }

        [MaxLength(4)]
        public string CodeJuridictionFonction { get; set; }

        public DateTime? DateCreation { get; set; } = DateTime.UtcNow;

        [MaxLength(200)]
        public string? Residence { get; set; }
        public int? IdFichePersonnelJudiciaire { get; set; }
        public int? IdUtilisateurCreation { get; set; }

        public string IdUserCreation { get; set; }
        //public int? IdFichier { get; set; }

    }
}
