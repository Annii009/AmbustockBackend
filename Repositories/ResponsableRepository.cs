using Microsoft.Data.SqlClient;
using AmbustockBackend.Models;

namespace AmbustockBackend.Repositories
{
    public class ResponsableRepository : IResponsableRepository
    {
        private readonly string _connectionString;

        public ResponsableRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("CadenaConexionAmbuStock");
        }

        public async Task<IEnumerable<Responsable>> GetAllAsync()
        {
            var responsables = new List<Responsable>();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand(
                @"SELECT Id_responsable, Nombre_Responsable, Fecha_Servicio, 
                         Id_servicio, Id_usuario, Id_Reposicion
                  FROM responsable",
                connection);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                responsables.Add(MapToResponsable(reader));
            }
            return responsables;
        }

        public async Task<Responsable?> GetByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand(
                @"SELECT Id_responsable, Nombre_Responsable, Fecha_Servicio, 
                         Id_servicio, Id_usuario, Id_Reposicion
                  FROM responsable
                  WHERE Id_responsable = @Id",
                connection);
            command.Parameters.AddWithValue("@Id", id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return MapToResponsable(reader);
            }
            return null;
        }

        public async Task<IEnumerable<Responsable>> GetByServicioIdAsync(int idServicio)
        {
            var responsables = new List<Responsable>();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand(
                @"SELECT Id_responsable, Nombre_Responsable, Fecha_Servicio, 
                         Id_servicio, Id_usuario, Id_Reposicion
                  FROM responsable
                  WHERE Id_servicio = @IdServicio",
                connection);
            command.Parameters.AddWithValue("@IdServicio", idServicio);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                responsables.Add(MapToResponsable(reader));
            }
            return responsables;
        }

        public async Task<IEnumerable<Responsable>> GetByUsuarioIdAsync(int idUsuario)
        {
            var responsables = new List<Responsable>();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand(
                @"SELECT Id_responsable, Nombre_Responsable, Fecha_Servicio, 
                         Id_servicio, Id_usuario, Id_Reposicion
                  FROM responsable
                  WHERE Id_usuario = @IdUsuario",
                connection);
            command.Parameters.AddWithValue("@IdUsuario", idUsuario);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                responsables.Add(MapToResponsable(reader));
            }
            return responsables;
        }

        public async Task<Responsable> AddAsync(Responsable responsable)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand(
                @"INSERT INTO responsable (Nombre_Responsable, Fecha_Servicio, Id_servicio, Id_usuario, Id_Reposicion)
                  OUTPUT INSERTED.Id_responsable
                  VALUES (@NombreResponsable, @FechaServicio, @IdServicio, @IdUsuario, @IdReposicion)",
                connection);

            command.Parameters.AddWithValue("@NombreResponsable", responsable.NombreResponsable);
            command.Parameters.AddWithValue("@FechaServicio", (object)responsable.FechaServicio ?? DBNull.Value);
            command.Parameters.AddWithValue("@IdServicio", (object)responsable.IdServicio ?? DBNull.Value);
            command.Parameters.AddWithValue("@IdUsuario", (object)responsable.IdUsuario ?? DBNull.Value);
            command.Parameters.AddWithValue("@IdReposicion", (object)responsable.IdReposicion ?? DBNull.Value);

            var id = (int)await command.ExecuteScalarAsync();
            responsable.IdResponsable = id;

            return responsable;
        }

        public async Task UpdateAsync(Responsable responsable)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand(
                @"UPDATE responsable 
                  SET Nombre_Responsable = @NombreResponsable,
                      Fecha_Servicio = @FechaServicio,
                      Id_servicio = @IdServicio,
                      Id_usuario = @IdUsuario,
                      Id_Reposicion = @IdReposicion
                  WHERE Id_responsable = @Id",
                connection);

            command.Parameters.AddWithValue("@Id", responsable.IdResponsable);
            command.Parameters.AddWithValue("@NombreResponsable", responsable.NombreResponsable);
            command.Parameters.AddWithValue("@FechaServicio", (object)responsable.FechaServicio ?? DBNull.Value);
            command.Parameters.AddWithValue("@IdServicio", (object)responsable.IdServicio ?? DBNull.Value);
            command.Parameters.AddWithValue("@IdUsuario", (object)responsable.IdUsuario ?? DBNull.Value);
            command.Parameters.AddWithValue("@IdReposicion", (object)responsable.IdReposicion ?? DBNull.Value);

            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand("DELETE FROM responsable WHERE Id_responsable = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);

            await command.ExecuteNonQueryAsync();
        }

        private Responsable MapToResponsable(SqlDataReader r)
        {
            return new Responsable
            {
                IdResponsable = r.GetInt32(r.GetOrdinal("Id_responsable")),
                NombreResponsable = r.GetString(r.GetOrdinal("Nombre_Responsable")),
                FechaServicio = r.IsDBNull(r.GetOrdinal("Fecha_Servicio")) ? null : r.GetDateTime(r.GetOrdinal("Fecha_Servicio")),
                IdServicio = r.IsDBNull(r.GetOrdinal("Id_servicio")) ? null : r.GetInt32(r.GetOrdinal("Id_servicio")),
                IdUsuario = r.IsDBNull(r.GetOrdinal("Id_usuario")) ? null : r.GetInt32(r.GetOrdinal("Id_usuario")),
                IdReposicion = r.IsDBNull(r.GetOrdinal("Id_Reposicion")) ? null : r.GetInt32(r.GetOrdinal("Id_Reposicion"))
            };
        }
    }
}
