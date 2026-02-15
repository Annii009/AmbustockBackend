using Microsoft.Data.SqlClient;
using AmbustockBackend.Models;

namespace AmbustockBackend.Repositories
{
    public class CorreoRepository : ICorreoRepository
    {
        private readonly string _connectionString;

        public CorreoRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("CadenaConexionAmbuStock");
        }

        public async Task<IEnumerable<Correo>> GetAllAsync()
        {
            var correos = new List<Correo>();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand(
                @"SELECT c.Id_Correo, c.fecha_alerta, c.tipo_problema, 
                         c.Id_material, c.Id_usuario, c.Id_reposicion,
                         m.nombre_Producto, u.Nombre_Usuario
                  FROM correo c
                  LEFT JOIN materiales m ON c.Id_material = m.Id_material
                  LEFT JOIN usuarios u ON c.Id_usuario = u.Id_usuario",
                connection);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                correos.Add(MapToCorreo(reader));
            }
            return correos;
        }

        public async Task<Correo?> GetByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand(
                @"SELECT c.Id_Correo, c.fecha_alerta, c.tipo_problema, 
                         c.Id_material, c.Id_usuario, c.Id_reposicion,
                         m.nombre_Producto, u.Nombre_Usuario
                  FROM correo c
                  LEFT JOIN materiales m ON c.Id_material = m.Id_material
                  LEFT JOIN usuarios u ON c.Id_usuario = u.Id_usuario
                  WHERE c.Id_Correo = @Id",
                connection);
            command.Parameters.AddWithValue("@Id", id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return MapToCorreo(reader);
            }
            return null;
        }

        public async Task<IEnumerable<Correo>> GetByUsuarioIdAsync(int idUsuario)
        {
            var correos = new List<Correo>();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand(
                @"SELECT c.Id_Correo, c.fecha_alerta, c.tipo_problema, 
                         c.Id_material, c.Id_usuario, c.Id_reposicion,
                         m.nombre_Producto, u.Nombre_Usuario
                  FROM correo c
                  LEFT JOIN materiales m ON c.Id_material = m.Id_material
                  LEFT JOIN usuarios u ON c.Id_usuario = u.Id_usuario
                  WHERE c.Id_usuario = @IdUsuario",
                connection);
            command.Parameters.AddWithValue("@IdUsuario", idUsuario);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                correos.Add(MapToCorreo(reader));
            }
            return correos;
        }

        public async Task<IEnumerable<Correo>> GetByMaterialIdAsync(int idMaterial)
        {
            var correos = new List<Correo>();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand(
                @"SELECT c.Id_Correo, c.fecha_alerta, c.tipo_problema, 
                         c.Id_material, c.Id_usuario, c.Id_reposicion,
                         m.nombre_Producto, u.Nombre_Usuario
                  FROM correo c
                  LEFT JOIN materiales m ON c.Id_material = m.Id_material
                  LEFT JOIN usuarios u ON c.Id_usuario = u.Id_usuario
                  WHERE c.Id_material = @IdMaterial",
                connection);
            command.Parameters.AddWithValue("@IdMaterial", idMaterial);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                correos.Add(MapToCorreo(reader));
            }
            return correos;
        }

        public async Task<Correo> AddAsync(Correo correo)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand(
                @"INSERT INTO correo (fecha_alerta, tipo_problema, Id_material, Id_usuario, Id_reposicion)
                  OUTPUT INSERTED.Id_Correo
                  VALUES (@FechaAlerta, @TipoProblema, @IdMaterial, @IdUsuario, @IdReposicion)",
                connection);

            command.Parameters.AddWithValue("@FechaAlerta", (object)correo.FechaAlerta ?? DBNull.Value);
            command.Parameters.AddWithValue("@TipoProblema", (object)correo.TipoProblema ?? DBNull.Value);
            command.Parameters.AddWithValue("@IdMaterial", (object)correo.IdMaterial ?? DBNull.Value);
            command.Parameters.AddWithValue("@IdUsuario", (object)correo.IdUsuario ?? DBNull.Value);
            command.Parameters.AddWithValue("@IdReposicion", (object)correo.IdReposicion ?? DBNull.Value);

            var id = (int)await command.ExecuteScalarAsync();
            correo.IdCorreo = id;

            return correo;
        }

        public async Task UpdateAsync(Correo correo)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand(
                @"UPDATE correo 
                  SET fecha_alerta = @FechaAlerta,
                      tipo_problema = @TipoProblema,
                      Id_material = @IdMaterial,
                      Id_usuario = @IdUsuario,
                      Id_reposicion = @IdReposicion
                  WHERE Id_Correo = @Id",
                connection);

            command.Parameters.AddWithValue("@Id", correo.IdCorreo);
            command.Parameters.AddWithValue("@FechaAlerta", (object)correo.FechaAlerta ?? DBNull.Value);
            command.Parameters.AddWithValue("@TipoProblema", (object)correo.TipoProblema ?? DBNull.Value);
            command.Parameters.AddWithValue("@IdMaterial", (object)correo.IdMaterial ?? DBNull.Value);
            command.Parameters.AddWithValue("@IdUsuario", (object)correo.IdUsuario ?? DBNull.Value);
            command.Parameters.AddWithValue("@IdReposicion", (object)correo.IdReposicion ?? DBNull.Value);

            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand("DELETE FROM correo WHERE Id_Correo = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);

            await command.ExecuteNonQueryAsync();
        }

        private Correo MapToCorreo(SqlDataReader r)
        {
            Materiales material = null;
            if (!r.IsDBNull(r.GetOrdinal("Id_material")))
            {
                material = new Materiales
                {
                    IdMaterial = r.GetInt32(r.GetOrdinal("Id_material")),
                    NombreProducto = r.GetString(r.GetOrdinal("nombre_Producto"))
                };
            }

            Usuarios usuario = null;
            if (!r.IsDBNull(r.GetOrdinal("Id_usuario")))
            {
                usuario = new Usuarios
                {
                    IdUsuario = r.GetInt32(r.GetOrdinal("Id_usuario")),
                    NombreUsuario = r.GetString(r.GetOrdinal("Nombre_Usuario"))
                };
            }

            return new Correo
            {
                IdCorreo = r.GetInt32(r.GetOrdinal("Id_Correo")),
                FechaAlerta = r.IsDBNull(r.GetOrdinal("fecha_alerta")) ? null : r.GetDateTime(r.GetOrdinal("fecha_alerta")),
                TipoProblema = r.IsDBNull(r.GetOrdinal("tipo_problema")) ? null : r.GetString(r.GetOrdinal("tipo_problema")),
                IdMaterial = r.IsDBNull(r.GetOrdinal("Id_material")) ? null : r.GetInt32(r.GetOrdinal("Id_material")),
                IdUsuario = r.IsDBNull(r.GetOrdinal("Id_usuario")) ? null : r.GetInt32(r.GetOrdinal("Id_usuario")),
                IdReposicion = r.IsDBNull(r.GetOrdinal("Id_reposicion")) ? null : r.GetInt32(r.GetOrdinal("Id_reposicion")),
                Materiales= material,
                Usuarios = usuario
            };
        }
    }
}
