using System.ComponentModel.DataAnnotations;

namespace SIGRHBack.Dtos.Authorization.Auth
{
    public class InputForgetPasswordDto
    {
        [Required]
        [MaxLength(100)]
        public string? Email { get; set; }
    }
}
