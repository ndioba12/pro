using System.ComponentModel.DataAnnotations;

namespace SIGRHBack.Dtos.Authorization.User
{
    public class InputUserUpdatePassword
    {
        //[Required]
        public string? IdUser { get; set; }
        [Required]
        [MaxLength(80)]
        [MinLength(6)]
        public string? OldPassword { get; set; }
        [Required]
        [MinLength(6)]
        [MaxLength(80)]
        public string? Password { get; set; }
        [Required]
        [MinLength(6)]
        [MaxLength(80)]
        [Compare("Password")]
        public string? ConfirmPassword { get; set; }
    }
}
