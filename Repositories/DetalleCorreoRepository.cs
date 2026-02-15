using Microsoft.Data.SqlClient;
using AmbustockBackend.Models;

namespace AmbustockBackend.Repositories
{
    public class DetalleCorreoRepository : IDetalleCorreoRepository
    {
        private readonly string _connectionString;

        public DetalleCorreoRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("CadenaConexionAmbuStock");
        }

        public async Task<IEnumerable<DetalleCorreo>> GetAllAsync()
        {
            var detalles = new List<DetalleCorreo>();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand(
                @"SELECT dc.Id_detalleCorreo, dc.Id_material, dc.Id_correo,
                         m.nombre_Producto, m.cantidad,
                         c.fecha_alerta, c.tipo_problema
                  FROM Detalle_Correo dc
                  INNER JOIN materiales m ON dc.Id_material = m.Id_material
                  INNER JOIN correo c ON dc.Id_correo = c.Id_Correo",
                connection);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                detalles.Add(MapToDetalleCorreo(reader));
            }
            return detalles;
        }

        public async Task<DetalleCorreo?> GetByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand(
                @"SELECT dc.Id_detalleCorreo, dc.Id_material, dc.Id_correo,
                         m.nombre_Producto, m.cantidad,
                         c.fecha_alerta, c.tipo_problema
                  FROM Detalle_Correo dc
                  INNER JOIN materiales m ON dc.Id_material = m.Id_material
                  INNER JOIN correo c ON dc.Id_correo = c.Id_Correo
                  WHERE dc.Id_detalleCorreo = @Id",
                connection);
            command.Parameters.AddWithValue("@Id", id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return MapToDetalleCorreo(reader);
            }
            return null;
        }

        public async Task<IEnumerable<DetalleCorreo>> GetByCorreoIdAsync(int idCorreo)
        {
            var detalles = new List<DetalleCorreo>();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand(
                @"SELECT dc.Id_detalleCorreo, dc.Id_material, dc.Id_correo,
                         m.nombre_Producto, m.cantidad,
                         c.fecha_alerta, c.tipo_problema
                  FROM Detalle_Correo dc
                  INNER JOIN materiales m ON dc.Id_material = m.Id_material
                  INNER JOIN correo c ON dc.Id_correo = c.Id_Correo
                  WHERE dc.Id_correo = @IdCorreo",
                connection);
            command.Parameters.AddWithValue("@IdCorreo", idCorreo);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                detalles.Add(MapToDetalleCorreo(reader));
            }
            return detalles;
        }

        public async Task<IEnumerable<DetalleCorreo>> GetByMaterialIdAsync(int idMaterial)
        {
            var detalles = new List<DetalleCorreo>();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand(
                @"SELECT dc.Id_detalleCorreo, dc.Id_material, dc.Id_correo,
                         m.nombre_Producto, m.cantidad,
                         c.fecha_alerta, c.tipo_problema
                  FROM Detalle_Correo dc
                  INNER JOIN materiales m ON dc.Id_material = m.Id_material
                  INNER JOIN correo c ON dc.Id_correo = c.Id_Correo
                  WHERE dc.Id_material = @IdMaterial",
                connection);
            command.Parameters.AddWithValue("@IdMaterial", idMaterial);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                detalles.Add(MapToDetalleCorreo(reader));
            }
            return detalles;
        }

        public async Task<DetalleCorreo> AddAsync(DetalleCorreo detalleCorreo)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand(
                @"INSERT INTO Detalle_Correo (Id_material, Id_correo)
                  OUTPUT INSERTED.Id_detalleCorreo
                  VALUES (@IdMaterial, @IdCorreo)",
                connection);

            command.Parameters.AddWithValue("@IdMaterial", detalleCorreo.IdMaterial);
            command.Parameters.AddWithValue("@IdCorreo", detalleCorreo.IdCorreo);

            var id = (int)await command.ExecuteScalarAsync();
            detalleCorreo.IdDetalleCorreo = id;

            return detalleCorreo;
        }

        public async Task UpdateAsync(DetalleCorreo detalleCorreo)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand(
                @"UPDATE Detalle_Correo 
                  SET Id_material = @IdMaterial,
                      Id_correo = @IdCorreo
                  WHERE Id_detalleCorreo = @Id",
                connection);

            command.Parameters.AddWithValue("@Id", detalleCorreo.IdDetalleCorreo);
            command.Parameters.AddWithValue("@IdMaterial", detalleCorreo.IdMaterial);
            command.Parameters.AddWithValue("@IdCorreo", detalleCorreo.IdCorreo);

            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand("DELETE FROM Detalle_Correo WHERE Id_detalleCorreo = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);

            await command.ExecuteNonQueryAsync();
        }

        private DetalleCorreo MapToDetalleCorreo(SqlDataReader r)
        {
            var material = new Materiales
            {
                IdMaterial = r.GetInt32(r.GetOrdinal("Id_material")),
                NombreProducto = r.GetString(r.GetOrdinal("nombre_Producto")),
                Cantidad = r.GetInt32(r.GetOrdinal("cantidad"))
            };

            var correo = new Correo
            {
                IdCorreo = r.GetInt32(r.GetOrdinal("Id_correo")),
                FechaAlerta = r.IsDBNull(r.GetOrdinal("fecha_alerta")) ? null : r.GetDateTime(r.GetOrdinal("fecha_alerta")),
                TipoProblema = r.IsDBNull(r.GetOrdinal("tipo_problema")) ? null : r.GetString(r.GetOrdinal("tipo_problema"))
            };

            return new DetalleCorreo
            {
                IdDetalleCorreo = r.GetInt32(r.GetOrdinal("Id_detalleCorreo")),
                IdMaterial = r.GetInt32(r.GetOrdinal("Id_material")),
                IdCorreo = r.GetInt32(r.GetOrdinal("Id_correo")),
                Materiales = material,
                Correo = correo
            };
        }
    }
}
