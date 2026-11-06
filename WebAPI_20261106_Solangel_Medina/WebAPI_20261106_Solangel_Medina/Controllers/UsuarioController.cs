using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI_20261106_Solangel_Medina.Dtos;
using WebAPI_20261106_Solangel_Medina.Services;

namespace WebAPI_20261106_Solangel_Medina.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioService _service;

        public UsuarioController(UsuarioService service)
        {
            _service = service;
        }

   
        [HttpPost("registro")]
        [AllowAnonymous]
        public async Task<IActionResult> Registro(RegistroDto dto)
        {
            var id = await _service.Registrar(dto);

            if (id == null)
                return Conflict("El correo ya está registrado");

            return Ok(new
            {
                Mensaje = "Usuario registrado correctamente",
                Id = id
            });
        }


        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var token = await _service.Login(dto);

            if (token == null)
                return Unauthorized("Credenciales incorrectas");

            return Ok(new { Token = token });
        }

 
        [HttpGet]
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> GetAll()
        {
            var usuarios = await _service.GetAll();
            return Ok(usuarios);
        }

       
        [HttpDelete("{id}")]
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> Delete(int id)
        {
            var eliminado = await _service.Delete(id);

            if (!eliminado)
                return NotFound("Usuario no encontrado");

            return Ok("Usuario eliminado correctamente");
        }
    }
}
