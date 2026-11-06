using Dapper;
using WebAPI_20261106_Solangel_Medina.Data;
using WebAPI_20261106_Solangel_Medina.Models;

namespace WebAPI_20261106_Solangel_Medina.Services
{
    public class InscripcionService
    {
        private readonly DataContext _context;

        public InscripcionService(DataContext context)
        {
            _context = context;
        }

        
        public async Task<string> Inscribir(int usuarioId, int cursoId)
        {
            using var connection = _context.CreateConnection();

            string verificar = @"
                SELECT COUNT(1)
                FROM Usuarios
                WHERE Id = @UsuarioId
                  AND Rol = 'Estudiante'
                  AND Estado = 1";

            int estudiante = await connection.ExecuteScalarAsync<int>(
                verificar, new { UsuarioId = usuarioId });

            if (estudiante == 0)
                return "ESTUDIANTE_INVALIDO";

            string verificarCurso = @"
                SELECT COUNT(1)
                FROM Cursos
                WHERE Id = @CursoId AND Estado = 1";

            int curso = await connection.ExecuteScalarAsync<int>(
                verificarCurso, new { CursoId = cursoId });

            if (curso == 0)
                return "CURSO_NO_EXISTE";

            string sql = @"
                INSERT INTO Inscripciones
                    (UsuarioId, CursoId, FechaInscripcion, Estado)
                SELECT
                    @UsuarioId, @CursoId, GETDATE(), 1
                WHERE NOT EXISTS (
                    SELECT 1
                    FROM Inscripciones
                    WHERE UsuarioId = @UsuarioId
                      AND CursoId = @CursoId
                )";

            int rows = await connection.ExecuteAsync(
                sql, new { UsuarioId = usuarioId, CursoId = cursoId });

            return rows > 0 ? "OK" : "YA_INSCRITO";
        }

        
        public async Task<IEnumerable<Inscripciones>> GetAll()
        {
            using var connection = _context.CreateConnection();

            string sql = @"
                SELECT i.Id, i.UsuarioId, i.CursoId,
                       i.FechaInscripcion, i.Estado
                FROM Inscripciones i
                INNER JOIN Usuarios u ON i.UsuarioId = u.Id
                INNER JOIN Cursos c ON i.CursoId = c.Id
                WHERE i.Estado = 1
                  AND u.Estado = 1
                  AND c.Estado = 1
                ORDER BY i.Id DESC";

            return await connection.QueryAsync<Inscripciones>(sql);
        }

        
        public async Task<IEnumerable<Cursos>> GetCursosEstudiante(
            int usuarioId)
        {
            using var connection = _context.CreateConnection();

            string sql = @"
                SELECT c.Id, c.Titulo, c.Descripcion,
                       c.Duracion, c.Materiales,
                       c.Cronograma, c.Estado
                FROM Cursos c
                INNER JOIN Inscripciones i ON c.Id = i.CursoId
                INNER JOIN Usuarios u ON i.UsuarioId = u.Id
                WHERE i.UsuarioId = @UsuarioId
                  AND i.Estado = 1
                  AND c.Estado = 1
                  AND u.Estado = 1";

            return await connection.QueryAsync<Cursos>(
                sql, new { UsuarioId = usuarioId });
        }

        
        public async Task<bool> Cancelar(int id, int usuarioId)
        {
            using var connection = _context.CreateConnection();

            string sql = @"
                UPDATE Inscripciones
                SET Estado = 0
                WHERE Id = @Id
                  AND UsuarioId = @UsuarioId
                  AND Estado = 1";

            int rows = await connection.ExecuteAsync(
                sql, new { Id = id, UsuarioId = usuarioId });

            return rows > 0;
        }
    }
}
