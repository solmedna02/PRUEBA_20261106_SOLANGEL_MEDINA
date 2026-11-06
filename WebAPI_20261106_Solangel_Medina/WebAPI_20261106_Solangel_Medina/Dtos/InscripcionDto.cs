using System.ComponentModel.DataAnnotations;

namespace WebAPI_20261106_Solangel_Medina.Dtos
{
    public class InscripcionDto
    {
        [Range(1, int.MaxValue)]
        public int CursoId { get; set; }
    }
}
