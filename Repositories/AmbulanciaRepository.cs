
using AmbustockBackend.Models;
using Microsoft.Data.SqlClient;
using AmbustockBackend.Repositories;


namespace AmbustockBackend.Repositories
{
    public class AmbulanciaRepository : IAmbulanciaRepository
    {
        private readonly string _connectionString;

        public AmbulanciaRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("CadenaConexionAmbuStock");
        }

        public async Task<IEnumerable<Ambulancia>> GetAllAsync()
        {
            var ambulancias = new List<Ambulancia>();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand("SELECT Id_ambulancia, Nombre, Matricula FROM ambulancia", connection);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                ambulancias.Add(MapToAmbulancia(reader));
            }
            return ambulancias;
        }

        public async Task<Ambulancia?> GetByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand(
                "SELECT Id_ambulancia, Nombre, Matricula FROM ambulancia WHERE Id_ambulancia = @Id",
                connection);
            command.Parameters.AddWithValue("@Id", id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return MapToAmbulancia(reader);
            }
            return null;
        }

        public async Task<Ambulancia?> GetByMatriculaAsync(string matricula)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand(
                "SELECT Id_ambulancia, Nombre, Matricula FROM ambulancia WHERE Matricula = @Matricula",
                connection);
            command.Parameters.AddWithValue("@Matricula", matricula);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return MapToAmbulancia(reader);
            }
            return null;
        }

        public async Task<Ambulancia> AddAsync(Ambulancia ambulancia)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand(
                @"INSERT INTO ambulancia (Nombre, Matricula)
                  OUTPUT INSERTED.Id_ambulancia
                  VALUES (@Nombre, @Matricula)",
                connection);

            command.Parameters.AddWithValue("@Nombre", ambulancia.Nombre);
            command.Parameters.AddWithValue("@Matricula", ambulancia.Matricula);

            var id = (int)await command.ExecuteScalarAsync();
            ambulancia.IdAmbulancia = id;

            return ambulancia;
        }

        public async Task UpdateAsync(Ambulancia ambulancia)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand(
                @"UPDATE ambulancia 
                  SET Nombre = @Nombre, Matricula = @Matricula
                  WHERE Id_ambulancia = @Id",
                connection);

            command.Parameters.AddWithValue("@Id", ambulancia.IdAmbulancia);
            command.Parameters.AddWithValue("@Nombre", ambulancia.Nombre);
            command.Parameters.AddWithValue("@Matricula", ambulancia.Matricula);

            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand("DELETE FROM ambulancia WHERE Id_ambulancia = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);

            await command.ExecuteNonQueryAsync();
        }

        private Ambulancia MapToAmbulancia(SqlDataReader r)
        {
            return new Ambulancia
            {
                IdAmbulancia = r.GetInt32(r.GetOrdinal("Id_ambulancia")),
                Nombre = r.GetString(r.GetOrdinal("Nombre")),
                Matricula = r.GetString(r.GetOrdinal("Matricula"))
            };
        }
    }
}
