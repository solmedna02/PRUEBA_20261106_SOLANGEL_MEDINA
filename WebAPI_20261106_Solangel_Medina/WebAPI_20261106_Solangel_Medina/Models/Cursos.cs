namespace WebAPI_20261106_Solangel_Medina.Models
{
    public class Cursos
    {
        public int Id { get; set; }
        public string? Titulo { get; set; }
        public string? Descripcion { get; set; }
        public int Duracion { get; set; }
        public string? Materiales { get; set; }
        public DateTime Cronograma { get; set; }
        public bool Estado { get; set; } = true;
    }
}
