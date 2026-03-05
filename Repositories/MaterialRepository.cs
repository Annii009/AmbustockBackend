using Microsoft.Data.SqlClient;
using AmbustockBackend.Models;

namespace AmbustockBackend.Repositories
{
    public class MaterialRepository : IMaterialRepository
    {
        private readonly string _connectionString;

        public MaterialRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("CadenaConexionAmbuStock");
        }

        public async Task<IEnumerable<Materiales>> GetAllAsync()
        {
            var materiales = new List<Materiales>();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand(
                @"SELECT m.Id_material, m.nombre_Producto, m.cantidad, m.Id_zona, m.Id_cajon,
                         z.nombre_zona, c.Nombre_cajon, m.foto_url, m.foto_public_id
                  FROM materiales m
                  INNER JOIN zonas z ON m.Id_zona = z.ID_zona
                  LEFT JOIN cajones c ON m.Id_cajon = c.Id_cajon",
                connection);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
                materiales.Add(MapToMaterial(reader));

            return materiales;
        }

        public async Task<Materiales?> GetByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand(
                @"SELECT m.Id_material, m.nombre_Producto, m.cantidad, m.Id_zona, m.Id_cajon,
                         z.nombre_zona, c.Nombre_cajon, m.foto_url, m.foto_public_id
                  FROM materiales m
                  INNER JOIN zonas z ON m.Id_zona = z.ID_zona
                  LEFT JOIN cajones c ON m.Id_cajon = c.Id_cajon
                  WHERE m.Id_material = @Id",
                connection);
            command.Parameters.AddWithValue("@Id", id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
                return MapToMaterial(reader);

            return null;
        }

        public async Task<IEnumerable<Materiales>> GetByZonaIdAsync(int idZona)
        {
            var materiales = new List<Materiales>();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand(
                @"SELECT m.Id_material, m.nombre_Producto, m.cantidad, m.Id_zona, m.Id_cajon,
                         z.nombre_zona, c.Nombre_cajon, m.foto_url, m.foto_public_id
                  FROM materiales m
                  INNER JOIN zonas z ON m.Id_zona = z.ID_zona
                  LEFT JOIN cajones c ON m.Id_cajon = c.Id_cajon
                  WHERE m.Id_zona = @IdZona",
                connection);
            command.Parameters.AddWithValue("@IdZona", idZona);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
                materiales.Add(MapToMaterial(reader));

            return materiales;
        }

        public async Task<IEnumerable<Materiales>> GetByCajonIdAsync(int idCajon)
        {
            var materiales = new List<Materiales>();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand(
                @"SELECT m.Id_material, m.nombre_Producto, m.cantidad, m.Id_zona, m.Id_cajon,
                         z.nombre_zona, c.Nombre_cajon, m.foto_url, m.foto_public_id
                  FROM materiales m
                  INNER JOIN zonas z ON m.Id_zona = z.ID_zona
                  LEFT JOIN cajones c ON m.Id_cajon = c.Id_cajon
                  WHERE m.Id_cajon = @IdCajon",
                connection);
            command.Parameters.AddWithValue("@IdCajon", idCajon);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
                materiales.Add(MapToMaterial(reader));

            return materiales;
        }

        public async Task<IEnumerable<Materiales>> GetByCantidadBajaAsync(int cantidadMinima)
        {
            var materiales = new List<Materiales>();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand(
                @"SELECT m.Id_material, m.nombre_Producto, m.cantidad, m.Id_zona, m.Id_cajon,
                         z.nombre_zona, c.Nombre_cajon, m.foto_url, m.foto_public_id
                  FROM materiales m
                  INNER JOIN zonas z ON m.Id_zona = z.ID_zona
                  LEFT JOIN cajones c ON m.Id_cajon = c.Id_cajon
                  WHERE m.cantidad <= @CantidadMinima",
                connection);
            command.Parameters.AddWithValue("@CantidadMinima", cantidadMinima);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
                materiales.Add(MapToMaterial(reader));

            return materiales;
        }

        public async Task<Materiales> AddAsync(Materiales material)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand(
                @"INSERT INTO materiales (nombre_Producto, cantidad, Id_zona, Id_cajon, foto_url, foto_public_id)
                  OUTPUT INSERTED.Id_material
                  VALUES (@NombreProducto, @Cantidad, @IdZona, @IdCajon, @FotoUrl, @FotoPublicId)",
                connection);

            command.Parameters.AddWithValue("@NombreProducto", material.NombreProducto);
            command.Parameters.AddWithValue("@Cantidad", material.Cantidad);
            command.Parameters.AddWithValue("@IdZona", material.IdZona);
            command.Parameters.AddWithValue("@IdCajon", (object?)material.IdCajon ?? DBNull.Value);
            command.Parameters.AddWithValue("@FotoUrl", (object?)material.FotoUrl ?? DBNull.Value);
            command.Parameters.AddWithValue("@FotoPublicId", (object?)material.FotoPublicId ?? DBNull.Value);

            var id = (int)await command.ExecuteScalarAsync();
            material.IdMaterial = id;

            return material;
        }

        public async Task UpdateAsync(Materiales material)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand(
                @"UPDATE materiales 
                  SET nombre_Producto = @NombreProducto, 
                      cantidad        = @Cantidad, 
                      Id_zona         = @IdZona, 
                      Id_cajon        = @IdCajon,
                      foto_url        = @FotoUrl,
                      foto_public_id  = @FotoPublicId
                  WHERE Id_material = @Id",
                connection);

            command.Parameters.AddWithValue("@Id", material.IdMaterial);
            command.Parameters.AddWithValue("@NombreProducto", material.NombreProducto);
            command.Parameters.AddWithValue("@Cantidad", material.Cantidad);
            command.Parameters.AddWithValue("@IdZona", material.IdZona);
            command.Parameters.AddWithValue("@IdCajon", (object?)material.IdCajon ?? DBNull.Value);
            command.Parameters.AddWithValue("@FotoUrl", (object?)material.FotoUrl ?? DBNull.Value);
            command.Parameters.AddWithValue("@FotoPublicId", (object?)material.FotoPublicId ?? DBNull.Value);

            await command.ExecuteNonQueryAsync();
        }

        public async Task UpdateCantidadAsync(int idMaterial, int nuevaCantidad)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand(
                "UPDATE materiales SET cantidad = @Cantidad WHERE Id_material = @Id",
                connection);

            command.Parameters.AddWithValue("@Id", idMaterial);
            command.Parameters.AddWithValue("@Cantidad", nuevaCantidad);

            await command.ExecuteNonQueryAsync();
        }

        // Actualiza únicamente la foto de un material
        public async Task UpdateFotoAsync(int idMaterial, string fotoUrl, string fotoPublicId)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand(
                @"UPDATE materiales 
                  SET foto_url = @FotoUrl, foto_public_id = @FotoPublicId
                  WHERE Id_material = @Id",
                connection);

            command.Parameters.AddWithValue("@Id", idMaterial);
            command.Parameters.AddWithValue("@FotoUrl", fotoUrl);
            command.Parameters.AddWithValue("@FotoPublicId", fotoPublicId);

            await command.ExecuteNonQueryAsync();
        }

        // Devuelve el foto_public_id de un material (para poder borrarlo de Cloudinary)
        public async Task<string?> GetFotoPublicIdAsync(int idMaterial)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand(
                "SELECT foto_public_id FROM materiales WHERE Id_material = @Id",
                connection);
            command.Parameters.AddWithValue("@Id", idMaterial);

            var result = await command.ExecuteScalarAsync();
            return result == DBNull.Value ? null : result?.ToString();
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand("DELETE FROM materiales WHERE Id_material = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);

            await command.ExecuteNonQueryAsync();
        }

        public async Task<IEnumerable<Materiales>> GetByZonaSinCajonAsync(int idZona)
        {
            var materiales = new List<Materiales>();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand(
                @"SELECT m.Id_material, m.nombre_Producto, m.cantidad, m.Id_zona, m.Id_cajon,
                         z.nombre_zona, m.foto_url, m.foto_public_id
                  FROM materiales m
                  INNER JOIN zonas z ON m.Id_zona = z.ID_zona
                  WHERE m.Id_zona = @IdZona AND m.Id_cajon IS NULL",
                connection);
            command.Parameters.AddWithValue("@IdZona", idZona);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                materiales.Add(new Materiales
                {
                    IdMaterial    = reader.GetInt32(reader.GetOrdinal("Id_material")),
                    NombreProducto = reader.GetString(reader.GetOrdinal("nombre_Producto")),
                    Cantidad      = reader.GetInt32(reader.GetOrdinal("cantidad")),
                    IdZona        = reader.GetInt32(reader.GetOrdinal("Id_zona")),
                    IdCajon       = null,
                    Zona          = new Zonas
                    {
                        IdZona     = reader.GetInt32(reader.GetOrdinal("Id_zona")),
                        NombreZona = reader.GetString(reader.GetOrdinal("nombre_zona"))
                    },
                    Cajon         = null,
                    FotoUrl       = reader.IsDBNull(reader.GetOrdinal("foto_url")) ? null : reader.GetString(reader.GetOrdinal("foto_url")),
                    FotoPublicId  = reader.IsDBNull(reader.GetOrdinal("foto_public_id")) ? null : reader.GetString(reader.GetOrdinal("foto_public_id"))
                });
            }

            return materiales;
        }


        private Materiales MapToMaterial(SqlDataReader r)
        {
            Cajones? cajon = null;
            if (!r.IsDBNull(r.GetOrdinal("Id_cajon")))
            {
                cajon = new Cajones
                {
                    IdCajon     = r.GetInt32(r.GetOrdinal("Id_cajon")),
                    NombreCajon = r.GetString(r.GetOrdinal("Nombre_cajon"))
                };
            }

            return new Materiales
            {
                IdMaterial     = r.GetInt32(r.GetOrdinal("Id_material")),
                NombreProducto = r.GetString(r.GetOrdinal("nombre_Producto")),
                Cantidad       = r.GetInt32(r.GetOrdinal("cantidad")),
                IdZona         = r.GetInt32(r.GetOrdinal("Id_zona")),
                IdCajon        = r.IsDBNull(r.GetOrdinal("Id_cajon")) ? null : r.GetInt32(r.GetOrdinal("Id_cajon")),
                Zona = new Zonas
                {
                    IdZona     = r.GetInt32(r.GetOrdinal("Id_zona")),
                    NombreZona = r.GetString(r.GetOrdinal("nombre_zona"))
                },
                Cajon         = cajon,
                FotoUrl       = r.IsDBNull(r.GetOrdinal("foto_url")) ? null : r.GetString(r.GetOrdinal("foto_url")),
                FotoPublicId  = r.IsDBNull(r.GetOrdinal("foto_public_id")) ? null : r.GetString(r.GetOrdinal("foto_public_id"))
            };
        }
    }
}