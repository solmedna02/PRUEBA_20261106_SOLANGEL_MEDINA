namespace WebAPI_20261106_Solangel_Medina.Models
{
    public class Inscripciones
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int CursoId { get; set; }
        public DateTime FechaInscripcion { get; set; }
        public bool Estado { get; set; } = true;
    }
}
