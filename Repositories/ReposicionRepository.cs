using Microsoft.Data.SqlClient;
using AmbustockBackend.Models;

namespace AmbustockBackend.Repositories
{
    public class ReposicionRepository : IReposicionRepository
    {
        private readonly string _connectionString;

        public ReposicionRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("CadenaConexionAmbuStock");
        }

        public async Task<IEnumerable<Reposicion>> GetAllAsync()
        {
            var reposiciones = new List<Reposicion>();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand(
                @"SELECT r.id_reposicion, r.Id_Correo, r.Nombre_material, r.Cantidad, 
                         r.Comentarios, r.foto_evidencia
                  FROM Reposicion r",
                connection);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                reposiciones.Add(MapToReposicion(reader));
            }
            return reposiciones;
        }

        public async Task<Reposicion?> GetByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand(
                @"SELECT r.id_reposicion, r.Id_Correo, r.Nombre_material, r.Cantidad, 
                         r.Comentarios, r.foto_evidencia
                  FROM Reposicion r
                  WHERE r.id_reposicion = @Id",
                connection);
            command.Parameters.AddWithValue("@Id", id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return MapToReposicion(reader);
            }
            return null;
        }

        public async Task<IEnumerable<Reposicion>> GetByCorreoIdAsync(int idCorreo)
        {
            var reposiciones = new List<Reposicion>();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand(
                @"SELECT r.id_reposicion, r.Id_Correo, r.Nombre_material, r.Cantidad, 
                         r.Comentarios, r.foto_evidencia
                  FROM Reposicion r
                  WHERE r.Id_Correo = @IdCorreo",
                connection);
            command.Parameters.AddWithValue("@IdCorreo", idCorreo);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                reposiciones.Add(MapToReposicion(reader));
            }
            return reposiciones;
        }

        public async Task<Reposicion> AddAsync(Reposicion reposicion)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand(
                @"INSERT INTO Reposicion (Id_Correo, Nombre_material, Cantidad, Comentarios, foto_evidencia)
                  OUTPUT INSERTED.id_reposicion
                  VALUES (@IdCorreo, @NombreMaterial, @Cantidad, @Comentarios, @FotoEvidencia)",
                connection);

            command.Parameters.AddWithValue("@IdCorreo", reposicion.IdCorreo);
            command.Parameters.AddWithValue("@NombreMaterial", (object)reposicion.NombreMaterial ?? DBNull.Value);
            command.Parameters.AddWithValue("@Cantidad", (object)reposicion.Cantidad ?? DBNull.Value);
            command.Parameters.AddWithValue("@Comentarios", (object)reposicion.Comentarios ?? DBNull.Value);
            command.Parameters.AddWithValue("@FotoEvidencia", (object)reposicion.FotoEvidencia ?? DBNull.Value);

            var id = (int)await command.ExecuteScalarAsync();
            reposicion.IdReposicion = id;

            return reposicion;
        }

        public async Task UpdateAsync(Reposicion reposicion)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand(
                @"UPDATE Reposicion 
                  SET Id_Correo = @IdCorreo,
                      Nombre_material = @NombreMaterial,
                      Cantidad = @Cantidad,
                      Comentarios = @Comentarios,
                      foto_evidencia = @FotoEvidencia
                  WHERE id_reposicion = @Id",
                connection);

            command.Parameters.AddWithValue("@Id", reposicion.IdReposicion);
            command.Parameters.AddWithValue("@IdCorreo", reposicion.IdCorreo);
            command.Parameters.AddWithValue("@NombreMaterial", (object)reposicion.NombreMaterial ?? DBNull.Value);
            command.Parameters.AddWithValue("@Cantidad", (object)reposicion.Cantidad ?? DBNull.Value);
            command.Parameters.AddWithValue("@Comentarios", (object)reposicion.Comentarios ?? DBNull.Value);
            command.Parameters.AddWithValue("@FotoEvidencia", (object)reposicion.FotoEvidencia ?? DBNull.Value);

            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand("DELETE FROM Reposicion WHERE id_reposicion = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);

            await command.ExecuteNonQueryAsync();
        }

        private Reposicion MapToReposicion(SqlDataReader r)
        {
            return new Reposicion
            {
                IdReposicion = r.GetInt32(r.GetOrdinal("id_reposicion")),
                IdCorreo = r.GetInt32(r.GetOrdinal("Id_Correo")),
                NombreMaterial = r.IsDBNull(r.GetOrdinal("Nombre_material")) ? null : r.GetString(r.GetOrdinal("Nombre_material")),
                Cantidad = r.IsDBNull(r.GetOrdinal("Cantidad")) ? null : r.GetInt32(r.GetOrdinal("Cantidad")),
                Comentarios = r.IsDBNull(r.GetOrdinal("Comentarios")) ? null : r.GetString(r.GetOrdinal("Comentarios")),
                FotoEvidencia = r.IsDBNull(r.GetOrdinal("foto_evidencia")) ? null : (byte[])r["foto_evidencia"]
            };
        }
    }
}
