using Microsoft.Data.SqlClient;
using AmbustockBackend.Models;

namespace AmbustockBackend.Repositories
{
    public class RevisionRepository : IRevisionRepository
    {
        private readonly string _connectionString;

        public RevisionRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("CadenaConexionAmbuStock");
        }

        public async Task<IEnumerable<Revision>> GetAllAsync()
        {
            var revisiones = new List<Revision>();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand(
                "SELECT * FROM Revisiones ORDER BY Fecha_Revision DESC",
                connection);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                revisiones.Add(MapToRevision(reader));
            }
            return revisiones;
        }

        public async Task<Revision> GetByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand(
                "SELECT * FROM Revisiones WHERE Id_revision = @Id",
                connection);
            command.Parameters.AddWithValue("@Id", id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return MapToRevision(reader);
            }
            return null;
        }

        public async Task<Revision> AddAsync(Revision revision)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand(
                @"INSERT INTO Revisiones 
                    (Id_ambulancia, Id_servicio, Nombre_Responsable, Fecha_Revision, Total_Materiales, Materiales_Revisados, Estado)
                  OUTPUT INSERTED.Id_revision
                  VALUES (@IdAmbulancia, @IdServicio, @NombreResponsable, @FechaRevision, @TotalMateriales, @MaterialesRevisados, @Estado)",
                connection);

            command.Parameters.AddWithValue("@IdAmbulancia", revision.Id_ambulancia);
            command.Parameters.AddWithValue("@IdServicio", revision.Id_servicio);
            command.Parameters.AddWithValue("@NombreResponsable", revision.Nombre_Responsable);
            command.Parameters.AddWithValue("@FechaRevision", revision.Fecha_Revision);
            command.Parameters.AddWithValue("@TotalMateriales", revision.Total_Materiales);
            command.Parameters.AddWithValue("@MaterialesRevisados", revision.Materiales_Revisados);
            command.Parameters.AddWithValue("@Estado", revision.Estado);

            var id = (int)await command.ExecuteScalarAsync();
            revision.Id_revision = id;

            return revision;
        }

        private Revision MapToRevision(SqlDataReader r)
        {
            return new Revision
            {
                Id_revision = r.GetInt32(r.GetOrdinal("Id_revision")),
                Id_ambulancia = r.GetInt32(r.GetOrdinal("Id_ambulancia")),
                Id_servicio = r.GetInt32(r.GetOrdinal("Id_servicio")),
                Nombre_Responsable = r.GetString(r.GetOrdinal("Nombre_Responsable")),
                Fecha_Revision = r.GetDateTime(r.GetOrdinal("Fecha_Revision")),
                Total_Materiales = r.GetInt32(r.GetOrdinal("Total_Materiales")),
                Materiales_Revisados = r.GetInt32(r.GetOrdinal("Materiales_Revisados")),
                Estado = r.GetString(r.GetOrdinal("Estado"))
            };
        }
    }
}
