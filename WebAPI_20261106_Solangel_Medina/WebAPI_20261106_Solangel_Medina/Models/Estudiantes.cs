namespace WebAPI_20261106_Solangel_Medina.Models
{
    public class Estudiantes
    {
        public int Id { get; set; }
        public string? Usuario { get; set; }
        public int Progreso { get; set; }
        public int Calificacion { get; set; }
        public string? Retroalimentacion { get; set; }
        public bool Estado { get; set; } = true;
    }
}
