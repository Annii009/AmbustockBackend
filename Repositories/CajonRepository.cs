using Microsoft.Data.SqlClient;
using AmbustockBackend.Models;

namespace AmbustockBackend.Repositories
{
    public class CajonRepository : ICajonRepository
    {
        private readonly string _connectionString;

        public CajonRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("CadenaConexionAmbuStock");
        }

        public async Task<IEnumerable<Cajones>> GetAllAsync()
        {
            var cajones = new List<Cajones>();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand(
                @"SELECT c.Id_cajon, c.Nombre_cajon, c.Id_zona,
                         z.nombre_zona
                  FROM cajones c
                  INNER JOIN zonas z ON c.Id_zona = z.ID_zona",
                connection);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                cajones.Add(MapToCajon(reader));
            }
            return cajones;
        }

        public async Task<Cajones?> GetByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand(
                @"SELECT c.Id_cajon, c.Nombre_cajon, c.Id_zona,
                         z.nombre_zona
                  FROM cajones c
                  INNER JOIN zonas z ON c.Id_zona = z.ID_zona
                  WHERE c.Id_cajon = @Id",
                connection);
            command.Parameters.AddWithValue("@Id", id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return MapToCajon(reader);
            }
            return null;
        }

        public async Task<IEnumerable<Cajones>> GetByZonaIdAsync(int idZona)
        {
            var cajones = new List<Cajones>();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand(
                @"SELECT c.Id_cajon, c.Nombre_cajon, c.Id_zona,
                         z.nombre_zona
                  FROM cajones c
                  INNER JOIN zonas z ON c.Id_zona = z.ID_zona
                  WHERE c.Id_zona = @IdZona",
                connection);
            command.Parameters.AddWithValue("@IdZona", idZona);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                cajones.Add(MapToCajon(reader));
            }
            return cajones;
        }

        public async Task<Cajones> AddAsync(Cajones cajon)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand(
                @"INSERT INTO cajones (Nombre_cajon, Id_zona)
                  OUTPUT INSERTED.Id_cajon
                  VALUES (@NombreCajon, @IdZona)",
                connection);

            command.Parameters.AddWithValue("@NombreCajon", cajon.NombreCajon);
            command.Parameters.AddWithValue("@IdZona", cajon.IdZona);

            var id = (int)await command.ExecuteScalarAsync();
            cajon.IdCajon = id;

            return cajon;
        }

        public async Task UpdateAsync(Cajones cajon)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand(
                @"UPDATE cajones 
                  SET Nombre_cajon = @NombreCajon,
                      Id_zona = @IdZona
                  WHERE Id_cajon = @Id",
                connection);

            command.Parameters.AddWithValue("@Id", cajon.IdCajon);
            command.Parameters.AddWithValue("@NombreCajon", cajon.NombreCajon);
            command.Parameters.AddWithValue("@IdZona", cajon.IdZona);

            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand("DELETE FROM cajones WHERE Id_cajon = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);

            await command.ExecuteNonQueryAsync();
        }

        private Cajones MapToCajon(SqlDataReader r)
        {
            var zona = new Zonas
            {
                IdZona = r.GetInt32(r.GetOrdinal("Id_zona")),
                NombreZona = r.GetString(r.GetOrdinal("nombre_zona"))
            };

            return new Cajones
            {
                IdCajon = r.GetInt32(r.GetOrdinal("Id_cajon")),
                NombreCajon = r.GetString(r.GetOrdinal("Nombre_cajon")),
                IdZona = r.GetInt32(r.GetOrdinal("Id_zona")),
                Zona = zona
            };
        }
    }
}
