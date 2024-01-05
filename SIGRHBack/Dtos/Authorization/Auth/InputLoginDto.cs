using System.ComponentModel.DataAnnotations;

namespace SIGRHBack.Dtos.Authorization.Auth
{
    public class InputLoginDto
    {
        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string Identifiant { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 8)]
        public string Password { get; set; }
        public bool RememberMe { get; set; } = true;
    }
}
