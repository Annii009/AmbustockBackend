using Microsoft.Data.SqlClient;
using AmbustockBackend.Models;

namespace AmbustockBackend.Repositories
{
    public class ServicioRepository : IServicioRepository
    {
        private readonly string _connectionString;

        public ServicioRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("CadenaConexionAmbuStock");
        }

        public async Task<IEnumerable<Servicio>> GetAllAsync()
        {
            var servicios = new List<Servicio>();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand(
                "SELECT Id_servicio, fecha_hora, nombre_servicio, Id_responsable FROM servicio",
                connection);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                servicios.Add(MapToServicio(reader));
            }
            return servicios;
        }

        public async Task<Servicio?> GetByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand(
                @"SELECT Id_servicio, fecha_hora, nombre_servicio, Id_responsable 
                  FROM servicio 
                  WHERE Id_servicio = @Id",
                connection);
            command.Parameters.AddWithValue("@Id", id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return MapToServicio(reader);
            }
            return null;
        }

        public async Task<IEnumerable<Servicio>> GetByFechaRangoAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            var servicios = new List<Servicio>();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand(
                @"SELECT Id_servicio, fecha_hora, nombre_servicio, Id_responsable 
                  FROM servicio 
                  WHERE fecha_hora BETWEEN @FechaInicio AND @FechaFin
                  ORDER BY fecha_hora DESC",
                connection);
            command.Parameters.AddWithValue("@FechaInicio", fechaInicio);
            command.Parameters.AddWithValue("@FechaFin", fechaFin);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                servicios.Add(MapToServicio(reader));
            }
            return servicios;
        }

        public async Task<IEnumerable<Servicio>> GetByResponsableIdAsync(int idResponsable)
        {
            var servicios = new List<Servicio>();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand(
                @"SELECT Id_servicio, fecha_hora, nombre_servicio, Id_responsable 
                  FROM servicio 
                  WHERE Id_responsable = @IdResponsable",
                connection);
            command.Parameters.AddWithValue("@IdResponsable", idResponsable);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                servicios.Add(MapToServicio(reader));
            }
            return servicios;
        }

        public async Task<Servicio> AddAsync(Servicio servicio)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand(
                @"INSERT INTO servicio (fecha_hora, nombre_servicio, Id_responsable)
                  OUTPUT INSERTED.Id_servicio
                  VALUES (@FechaHora, @NombreServicio, @IdResponsable)",
                connection);

            command.Parameters.AddWithValue("@FechaHora", servicio.FechaHora);
            command.Parameters.AddWithValue("@NombreServicio", (object)servicio.NombreServicio ?? DBNull.Value);
            command.Parameters.AddWithValue("@IdResponsable", (object)servicio.IdResponsable ?? DBNull.Value);

            var id = (int)await command.ExecuteScalarAsync();
            servicio.IdServicio = id;

            return servicio;
        }

        public async Task UpdateAsync(Servicio servicio)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand(
                @"UPDATE servicio 
                  SET fecha_hora = @FechaHora,
                      nombre_servicio = @NombreServicio,
                      Id_responsable = @IdResponsable
                  WHERE Id_servicio = @Id",
                connection);

            command.Parameters.AddWithValue("@Id", servicio.IdServicio);
            command.Parameters.AddWithValue("@FechaHora", servicio.FechaHora);
            command.Parameters.AddWithValue("@NombreServicio", (object)servicio.NombreServicio ?? DBNull.Value);
            command.Parameters.AddWithValue("@IdResponsable", (object)servicio.IdResponsable ?? DBNull.Value);

            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand("DELETE FROM servicio WHERE Id_servicio = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);

            await command.ExecuteNonQueryAsync();
        }

        private Servicio MapToServicio(SqlDataReader r)
        {
            return new Servicio
            {
                IdServicio = r.GetInt32(r.GetOrdinal("Id_servicio")),
                FechaHora = r.GetDateTime(r.GetOrdinal("fecha_hora")),
                NombreServicio = r.IsDBNull(r.GetOrdinal("nombre_servicio")) ? null : r.GetString(r.GetOrdinal("nombre_servicio")),
                IdResponsable = r.IsDBNull(r.GetOrdinal("Id_responsable")) ? null : r.GetInt32(r.GetOrdinal("Id_responsable"))
            };
        }
    }
}
