using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTO
{
    public class RegisterRequest
    {
        [Required]
        public string Name { get; set; } = null!;

        [Required, EmailAddress]
        public string Email { get; set; } = null!;

        [Required, MinLength(6)]
        public string Password { get; set; } = null!;
    }

}
