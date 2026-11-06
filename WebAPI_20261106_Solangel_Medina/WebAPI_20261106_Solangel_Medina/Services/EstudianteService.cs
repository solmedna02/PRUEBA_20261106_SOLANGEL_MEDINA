using Dapper;
using WebAPI_20261106_Solangel_Medina.Data;
using WebAPI_20261106_Solangel_Medina.Models;

namespace WebAPI_20261106_Solangel_Medina.Services
{
    public class EstudianteService
    {
        private readonly DataContext _context;

        public EstudianteService(DataContext context)
        {
            _context = context;
        }


        public async Task<IEnumerable<Estudiantes>> GetAllEstudiantes()
        {
            using var connection = _context.CreateConnection();

            string sql = @"
            SELECT Id, Usuario, Progreso,
                   Calificacion, Retroalimentacion
            FROM Estudiantes
            WHERE Estado = 1
            ORDER BY Id DESC";

            return await connection.QueryAsync<Estudiantes>(sql);
        }


        public async Task<Estudiantes?> GetEstudiantesById(int id)
        {
            using var connection = _context.CreateConnection();

            string sql = @"
            SELECT Id, Usuario, Progreso,
                   Calificacion, Retroalimentacion
            FROM Estudiantes
            WHERE Id = @Id AND Estado = 1";

            return await connection.QueryFirstOrDefaultAsync<Estudiantes>(
                sql, new { Id = id });
        }


        public async Task<int> CreateEstudiantes(Estudiantes estudiantes)
        {
            using var connection = _context.CreateConnection();

            string sql = @"
                INSERT INTO Estudiantes
                (Usuario, Progreso, Calificacion, Retroalimentacion)
                OUTPUT INSERTED.Id
                VALUES
                (@Usuario, @Progreso, @Calificacion, @Retroalimentacion)";

            return await connection.ExecuteScalarAsync<int>(
                sql, estudiantes);
        }


        public async Task<bool> UpdateEstudiantes(int id, Estudiantes estudiantes)
        {
            using var connection = _context.CreateConnection();

            string sql = @"
                UPDATE Estudiantes
                SET Usuario= @Usuario, 
                Progreso= @Progreso, 
                Calificacion= @Calificacion, 
                Retroalimentacion= @Retroalimentacion
                WHERE Id = @Id AND Estado = 1";

            int rows = await connection.ExecuteAsync(
                sql,
                new
                {
                    Id = id,
                    estudiantes.Usuario,
                    estudiantes.Progreso,
                    estudiantes.Calificacion,
                    estudiantes.Retroalimentacion
                });

            return rows > 0;
        }


        public async Task<bool> DeleteEstudiantes(int id)
        {
            using var connection = _context.CreateConnection();

            string sql = @"
            UPDATE Estudiantes
            SET Estado = 0
            WHERE Id = @Id AND Estado = 1";

            int rows = await connection.ExecuteAsync(
                sql, new { Id = id });

            return rows > 0;
        }
    }
}
