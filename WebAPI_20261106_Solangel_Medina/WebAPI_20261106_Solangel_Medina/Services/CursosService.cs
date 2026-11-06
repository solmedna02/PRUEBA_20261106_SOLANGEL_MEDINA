using Dapper;
using WebAPI_20261106_Solangel_Medina.Data;
using WebAPI_20261106_Solangel_Medina.Models;

namespace WebAPI_20261106_Solangel_Medina.Services
{
    public class CursosService
    {
        private readonly DataContext _context;

        public CursosService(DataContext context)
        {
            _context = context;
        }


        public async Task<IEnumerable<Cursos>> GetAllCursos()
        {
            using var connection = _context.CreateConnection();

            string sql = @"
            SELECT Id, Titulo, Descripcion,
                   Duracion, Materiales, Cronograma, Estado
            FROM Cursos
            WHERE Estado = 1
            ORDER BY Id DESC";

             return await connection.QueryAsync<Cursos>(sql);
        }


        public async Task<Cursos?> GetCursoById(int id)
        {
            using var connection = _context.CreateConnection();

            string sql = @"
        SELECT Id, Titulo, Descripcion,
               Duracion, Materiales, Cronograma, Estado
        FROM Cursos
        WHERE Id = @Id AND Estado = 1";

            return await connection.QueryFirstOrDefaultAsync<Cursos>(
                sql, new { Id = id });
        }


        public async Task<int> CreateCurso(Cursos curso)
        {
            using var connection = _context.CreateConnection();

            string sql = @"
                INSERT INTO Cursos
                (Titulo, Descripcion, Duracion, Materiales, Cronograma)
                OUTPUT INSERTED.Id
                VALUES
                (@Titulo, @Descripcion, @Duracion,
                 @Materiales, @Cronograma)";

            return await connection.ExecuteScalarAsync<int>(
                sql, curso);
        }

        
        public async Task<bool> UpdateCurso(int id, Cursos curso)
        {
            using var connection = _context.CreateConnection();

            string sql = @"
                UPDATE Cursos
                SET Titulo = @Titulo,
                    Descripcion = @Descripcion,
                    Duracion = @Duracion,
                    Materiales = @Materiales,
                    Cronograma = @Cronograma
                WHERE Id = @Id AND Estado = 1";

            int rows = await connection.ExecuteAsync(
                sql,
                new
                {
                    Id = id,
                    curso.Titulo,
                    curso.Descripcion,
                    curso.Duracion,
                    curso.Materiales,
                    curso.Cronograma
                });

            return rows > 0;
        }


        public async Task<bool> DeleteCurso(int id)
        {
            using var connection = _context.CreateConnection();

            string sql = @"
            UPDATE Cursos
            SET Estado = 0
            WHERE Id = @Id AND Estado = 1";

            int rows = await connection.ExecuteAsync(
                sql, new { Id = id });

            return rows > 0;
        }
    }
}
