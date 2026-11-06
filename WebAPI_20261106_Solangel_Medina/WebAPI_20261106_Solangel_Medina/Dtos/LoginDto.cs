using System.ComponentModel.DataAnnotations;

namespace WebAPI_20261106_Solangel_Medina.Dtos
{
    public class LoginDto
    {
        [Required, EmailAddress]
        public string Correo { get; set; } = "";

        [Required]
        public string Password { get; set; } = "";
    }
}
