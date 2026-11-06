using Dapper;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAPI_20261106_Solangel_Medina.Data;
using WebAPI_20261106_Solangel_Medina.Dtos;
using WebAPI_20261106_Solangel_Medina.Models;

namespace WebAPI_20261106_Solangel_Medina.Services
{
    public class UsuarioService
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;

        public UsuarioService(
            DataContext context,
            IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

    
        public async Task<int?> Registrar(RegistroDto dto)
        {
            using var connection = _context.CreateConnection();

            string correo = dto.Correo.Trim().ToLowerInvariant();

            string verificar = @"
                SELECT COUNT(1)
                FROM Users
                WHERE Correo = @Correo";

            int existe = await connection.ExecuteScalarAsync<int>(
                verificar, new { Correo = correo });

            if (existe > 0)
                return null;

            string hash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            string sql = @"
                INSERT INTO Users
                    (Nombre, Correo, PasswordHash, Rol, Estado)
                OUTPUT INSERTED.Id
                VALUES
                    (@Nombre, @Correo, @PasswordHash,
                     'Estudiante', 1)";

            return await connection.ExecuteScalarAsync<int>(
                sql,
                new
                {
                    Nombre = dto.Nombre.Trim(),
                    Correo = correo,
                    PasswordHash = hash
                });
        }


        public async Task<string?> Login(LoginDto dto)
        {
            using var connection = _context.CreateConnection();

            string sql = @"
                SELECT Id, Nombre, Correo,
                       PasswordHash, Rol, Estado
                FROM Users
                WHERE Correo = @Correo AND Estado = 1";

            var usuario = await connection.QueryFirstOrDefaultAsync<Users>(
                sql,
                new { Correo = dto.Correo.Trim().ToLowerInvariant() });

            if (usuario == null)
                return null;

            if (!BCrypt.Net.BCrypt.Verify(
                dto.Password, usuario.PasswordHash))
                return null;

            return GenerarToken(usuario);
        }


        private string GenerarToken(Users usuario)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,
                    usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.Nombre),
                new Claim(ClaimTypes.Email, usuario.Correo),
                new Claim(ClaimTypes.Role, usuario.Rol)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    _configuration["Jwt:Key"]!));

            var credentials = new SigningCredentials(
                key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }

        
        public async Task<IEnumerable<Users>> GetAll()
        {
            using var connection = _context.CreateConnection();

            string sql = @"
                SELECT Id, Nombre, Correo, Rol, Estado
                FROM Users
                WHERE Estado = 1
                ORDER BY Id DESC";

            return await connection.QueryAsync<Users>(sql);
        }


        public async Task<bool> Delete(int id)
        {
            using var connection = _context.CreateConnection();

            string sql = @"
                UPDATE Users
                SET Estado = 0
                WHERE Id = @Id AND Estado = 1";

            int rows = await connection.ExecuteAsync(
                sql, new { Id = id });

            return rows > 0;
        }
    }
}
