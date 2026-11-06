namespace WebAPI_20261106_Solangel_Medina.Models
{
    public class Users
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = "";
        public string Correo { get; set; } = "";
        public string PasswordHash { get; set; } = "";
        public string Rol { get; set; } = "Estudiante";
        public bool Estado { get; set; } = true;
    }
}
