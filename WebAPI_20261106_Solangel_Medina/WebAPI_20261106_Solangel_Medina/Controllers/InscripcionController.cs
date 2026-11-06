using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebAPI_20261106_Solangel_Medina.Dtos;
using WebAPI_20261106_Solangel_Medina.Services;

namespace WebAPI_20261106_Solangel_Medina.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class InscripcionController: ControllerBase
    {
        private readonly InscripcionService _service;

        public InscripcionController(InscripcionService service)
        {
            _service = service;
        }

        private int ObtenerUsuarioId()
        {
            return int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        }

        
        [HttpPost]
        [Authorize(Roles = "Estudiante")]
        public async Task<IActionResult> Inscribir(InscripcionDto dto)
        {
            int usuarioId = ObtenerUsuarioId();

            string resultado = await _service.Inscribir(
                usuarioId, dto.CursoId);

            return resultado switch
            {
                "OK" => Ok("Inscripción realizada correctamente"),

                "YA_INSCRITO" => Conflict(
                    "Ya existe una inscripción para este curso"),

                "CURSO_NO_EXISTE" => NotFound(
                    "Curso no encontrado"),

                _ => BadRequest("Estudiante no válido")
            };
        }

        
        [HttpGet]
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> GetAll()
        {
            var inscripciones = await _service.GetAll();
            return Ok(inscripciones);
        }

        
        [HttpGet("mis-cursos")]
        [Authorize(Roles = "Estudiante")]
        public async Task<IActionResult> MisCursos()
        {
            int usuarioId = ObtenerUsuarioId();

            var cursos = await _service.GetCursosEstudiante(usuarioId);

            return Ok(cursos);
        }

       
        [HttpDelete("{id}")]
        [Authorize(Roles = "Estudiante")]
        public async Task<IActionResult> Cancelar(int id)
        {
            int usuarioId = ObtenerUsuarioId();

            bool cancelado = await _service.Cancelar(id, usuarioId);

            if (!cancelado)
                return NotFound("Inscripción no encontrada");

            return Ok("Inscripción cancelada correctamente");
        }
    }
}
