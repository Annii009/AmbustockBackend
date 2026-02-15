using Microsoft.Data.SqlClient;
using AmbustockBackend.Models;

namespace AmbustockBackend.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly string _connectionString;

        public UsuarioRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("CadenaConexionAmbuStock");
        }

        public async Task<IEnumerable<Usuarios>> GetAllAsync()
        {
            var usuarios = new List<Usuarios>();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand(
                @"SELECT Id_usuario, Nombre_Usuario, Rol, email, Password, 
                         Id_responsable, Id_Correo
                  FROM usuarios",
                connection);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                usuarios.Add(MapToUsuario(reader));
            }
            return usuarios;
        }

        public async Task<Usuarios?> GetByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand(
                @"SELECT Id_usuario, Nombre_Usuario, Rol, email, Password, 
                         Id_responsable, Id_Correo
                  FROM usuarios
                  WHERE Id_usuario = @Id",
                connection);
            command.Parameters.AddWithValue("@Id", id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return MapToUsuario(reader);
            }
            return null;
        }

        public async Task<Usuarios?> GetByEmailAsync(string email)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand(
                @"SELECT Id_usuario, Nombre_Usuario, Rol, email, Password, 
                         Id_responsable, Id_Correo
                  FROM usuarios
                  WHERE email = @Email",
                connection);
            command.Parameters.AddWithValue("@Email", email);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return MapToUsuario(reader);
            }
            return null;
        }

        public async Task<Usuarios?> GetByEmailAndPasswordAsync(string email, string password)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand(
                @"SELECT Id_usuario, Nombre_Usuario, Rol, email, Password, 
                         Id_responsable, Id_Correo
                  FROM usuarios
                  WHERE email = @Email AND Password = @Password",
                connection);
            command.Parameters.AddWithValue("@Email", email);
            command.Parameters.AddWithValue("@Password", password);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return MapToUsuario(reader);
            }
            return null;
        }

        public async Task<Usuarios> AddAsync(Usuarios usuario)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand(
                @"INSERT INTO usuarios (Nombre_Usuario, Rol, email, Password, Id_responsable, Id_Correo)
                  OUTPUT INSERTED.Id_usuario
                  VALUES (@NombreUsuario, @Rol, @Email, @Password, @IdResponsable, @IdCorreo)",
                connection);

            command.Parameters.AddWithValue("@NombreUsuario", usuario.NombreUsuario);
            command.Parameters.AddWithValue("@Rol", (object?)usuario.Rol ?? DBNull.Value);
            command.Parameters.AddWithValue("@Email", (object?)usuario.Email ?? DBNull.Value);
            command.Parameters.AddWithValue("@Password", usuario.Password);
            command.Parameters.AddWithValue("@IdResponsable", (object?)usuario.IdResponsable ?? DBNull.Value);
            command.Parameters.AddWithValue("@IdCorreo", (object?)usuario.IdCorreo ?? DBNull.Value);

            var id = (int)await command.ExecuteScalarAsync();
            usuario.IdUsuario = id;

            return usuario;
        }

        public async Task UpdateAsync(Usuarios usuario)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand(
                @"UPDATE usuarios 
                  SET Nombre_Usuario = @NombreUsuario,
                      Rol = @Rol,
                      email = @Email,
                      Password = @Password,
                      Id_responsable = @IdResponsable,
                      Id_Correo = @IdCorreo
                  WHERE Id_usuario = @Id",
                connection);

            command.Parameters.AddWithValue("@Id", usuario.IdUsuario);
            command.Parameters.AddWithValue("@NombreUsuario", usuario.NombreUsuario);
            command.Parameters.AddWithValue("@Rol", (object?)usuario.Rol ?? DBNull.Value);
            command.Parameters.AddWithValue("@Email", (object?)usuario.Email ?? DBNull.Value);
            command.Parameters.AddWithValue("@Password", usuario.Password);
            command.Parameters.AddWithValue("@IdResponsable", (object?)usuario.IdResponsable ?? DBNull.Value);
            command.Parameters.AddWithValue("@IdCorreo", (object?)usuario.IdCorreo ?? DBNull.Value);

            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand("DELETE FROM usuarios WHERE Id_usuario = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);

            await command.ExecuteNonQueryAsync();
        }


        public async Task<IEnumerable<Usuarios>> GetByRolAsync(string rol)
        {
            var usuarios = new List<Usuarios>();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand(
                @"SELECT Id_usuario, Nombre_Usuario, Rol, Email, Password, Id_Responsable, Id_Correo
          FROM usuarios
          WHERE Rol = @Rol",
                connection);
            command.Parameters.AddWithValue("@Rol", rol);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                usuarios.Add(new Usuarios
                {
                    IdUsuario = reader.GetInt32(reader.GetOrdinal("Id_usuario")),
                    NombreUsuario = reader.GetString(reader.GetOrdinal("Nombre_Usuario")),
                    Rol = reader.IsDBNull(reader.GetOrdinal("Rol")) ? null : reader.GetString(reader.GetOrdinal("Rol")),
                    Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString(reader.GetOrdinal("Email")),
                    Password = reader.IsDBNull(reader.GetOrdinal("Password")) ? null : reader.GetString(reader.GetOrdinal("Password")),
                    IdResponsable = reader.IsDBNull(reader.GetOrdinal("Id_Responsable")) ? null : reader.GetInt32(reader.GetOrdinal("Id_Responsable")),
                    IdCorreo = reader.IsDBNull(reader.GetOrdinal("Id_Correo")) ? null : reader.GetInt32(reader.GetOrdinal("Id_Correo"))
                });
            }
            return usuarios;
        }


        private Usuarios MapToUsuario(SqlDataReader r)
        {
            return new Usuarios
            {
                IdUsuario = r.GetInt32(r.GetOrdinal("Id_usuario")),
                NombreUsuario = r.GetString(r.GetOrdinal("Nombre_Usuario")),
                Rol = r.IsDBNull(r.GetOrdinal("Rol")) ? null : r.GetString(r.GetOrdinal("Rol")),
                Email = r.IsDBNull(r.GetOrdinal("email")) ? null : r.GetString(r.GetOrdinal("email")),
                Password = r.GetString(r.GetOrdinal("Password")),
                IdResponsable = r.IsDBNull(r.GetOrdinal("Id_responsable")) ? null : r.GetInt32(r.GetOrdinal("Id_responsable")),
                IdCorreo = r.IsDBNull(r.GetOrdinal("Id_Correo")) ? null : r.GetInt32(r.GetOrdinal("Id_Correo"))
            };
        }
    }
}
