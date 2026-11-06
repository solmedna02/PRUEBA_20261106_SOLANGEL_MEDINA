using System.ComponentModel.DataAnnotations;

namespace WebAPI_20261106_Solangel_Medina.Dtos
{
    public class RegistroDto
    {
        [Required]
        public string Nombre { get; set; } = "";

        [Required, EmailAddress]
        public string Correo { get; set; } = "";

        [Required, MinLength(8)]
        public string Password { get; set; } = "";
    }
}
