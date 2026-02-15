using Microsoft.Data.SqlClient;
using AmbustockBackend.Models;

namespace AmbustockBackend.Repositories
{
    public class ServicioAmbulanciaRepository : IServicioAmbulanciaRepository
    {
        private readonly string _connectionString;

        public ServicioAmbulanciaRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("CadenaConexionAmbuStock");
        }

        public async Task<IEnumerable<ServicioAmbulancia>> GetAllAsync()
        {
            var serviciosAmbulancia = new List<ServicioAmbulancia>();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand(
                @"SELECT sa.Id_servicioAmbulancia, sa.Id_Ambulancia, sa.Id_Servicio,
                         a.Nombre as AmbulanciaNombre, a.Matricula,
                         s.fecha_hora, s.nombre_servicio
                  FROM Servicio_Ambulancia sa
                  INNER JOIN ambulancia a ON sa.Id_Ambulancia = a.Id_ambulancia
                  INNER JOIN servicio s ON sa.Id_Servicio = s.Id_servicio",
                connection);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                serviciosAmbulancia.Add(MapToServicioAmbulancia(reader));
            }
            return serviciosAmbulancia;
        }

        public async Task<ServicioAmbulancia?> GetByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand(
                @"SELECT sa.Id_servicioAmbulancia, sa.Id_Ambulancia, sa.Id_Servicio,
                         a.Nombre as AmbulanciaNombre, a.Matricula,
                         s.fecha_hora, s.nombre_servicio
                  FROM Servicio_Ambulancia sa
                  INNER JOIN ambulancia a ON sa.Id_Ambulancia = a.Id_ambulancia
                  INNER JOIN servicio s ON sa.Id_Servicio = s.Id_servicio
                  WHERE sa.Id_servicioAmbulancia = @Id",
                connection);
            command.Parameters.AddWithValue("@Id", id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return MapToServicioAmbulancia(reader);
            }
            return null;
        }

        public async Task<IEnumerable<ServicioAmbulancia>> GetByAmbulanciaIdAsync(int idAmbulancia)
        {
            var serviciosAmbulancia = new List<ServicioAmbulancia>();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand(
                @"SELECT sa.Id_servicioAmbulancia, sa.Id_Ambulancia, sa.Id_Servicio,
                         a.Nombre as AmbulanciaNombre, a.Matricula,
                         s.fecha_hora, s.nombre_servicio
                  FROM Servicio_Ambulancia sa
                  INNER JOIN ambulancia a ON sa.Id_Ambulancia = a.Id_ambulancia
                  INNER JOIN servicio s ON sa.Id_Servicio = s.Id_servicio
                  WHERE sa.Id_Ambulancia = @IdAmbulancia",
                connection);
            command.Parameters.AddWithValue("@IdAmbulancia", idAmbulancia);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                serviciosAmbulancia.Add(MapToServicioAmbulancia(reader));
            }
            return serviciosAmbulancia;
        }

        public async Task<IEnumerable<ServicioAmbulancia>> GetByServicioIdAsync(int idServicio)
        {
            var serviciosAmbulancia = new List<ServicioAmbulancia>();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand(
                @"SELECT sa.Id_servicioAmbulancia, sa.Id_Ambulancia, sa.Id_Servicio,
                         a.Nombre as AmbulanciaNombre, a.Matricula,
                         s.fecha_hora, s.nombre_servicio
                  FROM Servicio_Ambulancia sa
                  INNER JOIN ambulancia a ON sa.Id_Ambulancia = a.Id_ambulancia
                  INNER JOIN servicio s ON sa.Id_Servicio = s.Id_servicio
                  WHERE sa.Id_Servicio = @IdServicio",
                connection);
            command.Parameters.AddWithValue("@IdServicio", idServicio);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                serviciosAmbulancia.Add(MapToServicioAmbulancia(reader));
            }
            return serviciosAmbulancia;
        }

        public async Task<ServicioAmbulancia> AddAsync(ServicioAmbulancia servicioAmbulancia)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand(
                @"INSERT INTO Servicio_Ambulancia (Id_Ambulancia, Id_Servicio)
                  OUTPUT INSERTED.Id_servicioAmbulancia
                  VALUES (@IdAmbulancia, @IdServicio)",
                connection);

            command.Parameters.AddWithValue("@IdAmbulancia", servicioAmbulancia.IdAmbulancia);
            command.Parameters.AddWithValue("@IdServicio", servicioAmbulancia.IdServicio);

            var id = (int)await command.ExecuteScalarAsync();
            servicioAmbulancia.IdServicioAmbulancia = id;

            return servicioAmbulancia;
        }

        public async Task UpdateAsync(ServicioAmbulancia servicioAmbulancia)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand(
                @"UPDATE Servicio_Ambulancia 
                  SET Id_Ambulancia = @IdAmbulancia,
                      Id_Servicio = @IdServicio
                  WHERE Id_servicioAmbulancia = @Id",
                connection);

            command.Parameters.AddWithValue("@Id", servicioAmbulancia.IdServicioAmbulancia);
            command.Parameters.AddWithValue("@IdAmbulancia", servicioAmbulancia.IdAmbulancia);
            command.Parameters.AddWithValue("@IdServicio", servicioAmbulancia.IdServicio);

            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand("DELETE FROM Servicio_Ambulancia WHERE Id_servicioAmbulancia = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);

            await command.ExecuteNonQueryAsync();
        }

        private ServicioAmbulancia MapToServicioAmbulancia(SqlDataReader r)
        {
            var ambulancia = new Ambulancia
            {
                IdAmbulancia = r.GetInt32(r.GetOrdinal("Id_Ambulancia")),
                Nombre = r.GetString(r.GetOrdinal("AmbulanciaNombre")),
                Matricula = r.GetString(r.GetOrdinal("Matricula"))
            };

            var servicio = new Servicio
            {
                IdServicio = r.GetInt32(r.GetOrdinal("Id_Servicio")),
                FechaHora = r.GetDateTime(r.GetOrdinal("fecha_hora")),
                NombreServicio = r.IsDBNull(r.GetOrdinal("nombre_servicio")) ? null : r.GetString(r.GetOrdinal("nombre_servicio"))
            };

            return new ServicioAmbulancia
            {
                IdServicioAmbulancia = r.GetInt32(r.GetOrdinal("Id_servicioAmbulancia")),
                IdAmbulancia = r.GetInt32(r.GetOrdinal("Id_Ambulancia")),
                IdServicio = r.GetInt32(r.GetOrdinal("Id_Servicio")),
                Ambulancia = ambulancia,
                Servicio = servicio
            };
        }
    }
}
