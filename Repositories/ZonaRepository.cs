using Microsoft.Data.SqlClient;
using AmbustockBackend.Models;

namespace AmbustockBackend.Repositories
{
    public class ZonaRepository : IZonaRepository
    {
        private readonly string _connectionString;

        public ZonaRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("CadenaConexionAmbuStock");
        }

        public async Task<IEnumerable<Zonas>> GetAllAsync()
        {
            var zonas = new List<Zonas>();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand(
                @"SELECT z.ID_zona, z.nombre_zona, z.Id_ambulancia,
                         a.Nombre as AmbulanciaNombre, a.Matricula
                  FROM zonas z
                  INNER JOIN ambulancia a ON z.Id_ambulancia = a.Id_ambulancia",
                connection);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                zonas.Add(MapToZona(reader));
            }
            return zonas;
        }

        public async Task<Zonas?> GetByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand(
                @"SELECT z.ID_zona, z.nombre_zona, z.Id_ambulancia,
                         a.Nombre as AmbulanciaNombre, a.Matricula
                  FROM zonas z
                  INNER JOIN ambulancia a ON z.Id_ambulancia = a.Id_ambulancia
                  WHERE z.ID_zona = @Id",
                connection);
            command.Parameters.AddWithValue("@Id", id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return MapToZona(reader);
            }
            return null;
        }

        public async Task<IEnumerable<Zonas>> GetByAmbulanciaIdAsync(int idAmbulancia)
        {
            var zonas = new List<Zonas>();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand(
                @"SELECT z.ID_zona, z.nombre_zona, z.Id_ambulancia,
                         a.Nombre as AmbulanciaNombre, a.Matricula
                  FROM zonas z
                  INNER JOIN ambulancia a ON z.Id_ambulancia = a.Id_ambulancia
                  WHERE z.Id_ambulancia = @IdAmbulancia",
                connection);
            command.Parameters.AddWithValue("@IdAmbulancia", idAmbulancia);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                zonas.Add(MapToZona(reader));
            }
            return zonas;
        }

        public async Task<Zonas> AddAsync(Zonas zona)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand(
                @"INSERT INTO zonas (nombre_zona, Id_ambulancia)
                  OUTPUT INSERTED.ID_zona
                  VALUES (@NombreZona, @IdAmbulancia)",
                connection);

            command.Parameters.AddWithValue("@NombreZona", zona.NombreZona);
            command.Parameters.AddWithValue("@IdAmbulancia", zona.IdAmbulancia);

            var id = (int)await command.ExecuteScalarAsync();
            zona.IdZona = id;

            return zona;
        }

        public async Task UpdateAsync(Zonas zona)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand(
                @"UPDATE zonas 
                  SET nombre_zona = @NombreZona,
                      Id_ambulancia = @IdAmbulancia
                  WHERE ID_zona = @Id",
                connection);

            command.Parameters.AddWithValue("@Id", zona.IdZona);
            command.Parameters.AddWithValue("@NombreZona", zona.NombreZona);
            command.Parameters.AddWithValue("@IdAmbulancia", zona.IdAmbulancia);

            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand("DELETE FROM zonas WHERE ID_zona = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);

            await command.ExecuteNonQueryAsync();
        }

        private Zonas MapToZona(SqlDataReader r)
        {
            var ambulancia = new Ambulancia
            {
                IdAmbulancia = r.GetInt32(r.GetOrdinal("Id_ambulancia")),
                Nombre = r.GetString(r.GetOrdinal("AmbulanciaNombre")),
                Matricula = r.GetString(r.GetOrdinal("Matricula"))
            };

            return new Zonas
            {
                IdZona = r.GetInt32(r.GetOrdinal("ID_zona")),
                NombreZona = r.GetString(r.GetOrdinal("nombre_zona")),
                IdAmbulancia = r.GetInt32(r.GetOrdinal("Id_ambulancia")),
                Ambulancia = ambulancia
            };
        }
    }
}
