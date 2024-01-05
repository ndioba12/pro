using System.ComponentModel.DataAnnotations;

namespace SIGRHBack.Dtos.Authorization.Auth
{
    public class InputUpdatePassword
    {
        [Required]
        public string? Token { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 8)]
        public string? NewPassword { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 8)]
        [Compare("NewPassword")]
        public string? ConfirmNewPassword { get; set; }
    }
}
