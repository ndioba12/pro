using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIGRHBack.Models
{
    [Table("TD_SendMail")]
    public class SendMail
    {
        [Required]
        [MaxLength(100)]
        public string? AdresseDestinataire { get; set; }
        [Required]
        [MaxLength(100)]
        public string? ObjetEmail { get; set; }
        [Required]
        [MaxLength(1000)]
        public string? TextEmail { get; set; }
        public bool? StatusEmail { get; set; }
    }
}
