using System.ComponentModel.DataAnnotations.Schema;

namespace SIGRHBack.Models
{
    [Table("TD_TokenInfo")]
    public class TokenInfo
    {
        public int Id { get; set; }
        public string Usename { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiry { get; set; }
    }
}
