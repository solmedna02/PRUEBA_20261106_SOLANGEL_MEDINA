using Microsoft.AspNetCore.Mvc;
using WebAPI_20261106_Solangel_Medina.Models;
using WebAPI_20261106_Solangel_Medina.Services;

namespace WebAPI_20261106_Solangel_Medina.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CursoController : ControllerBase
    {
        private readonly CursosService _service;

        public CursoController(CursosService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var cursos = await _service.GetAllCursos();
            return Ok(cursos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var curso = await _service.GetCursoById(id);

            if (curso == null)
                return NotFound("Curso no encontrado");

            return Ok(curso);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Cursos curso)
        {
            var id = await _service.CreateCurso(curso);

            return CreatedAtAction(
                nameof(GetById),
                new { id },
                new { id });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id, Cursos curso)
        {
            var actualizado =
                await _service.UpdateCurso(id, curso);

            if (!actualizado)
                return NotFound("Curso no encontrado");

            return Ok("Curso actualizado correctamente");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var eliminado = await _service.DeleteCurso(id);

            if (!eliminado)
                return NotFound("Curso no encontrado");

            return Ok("Curso eliminado correctamente");
        }
    }
}
