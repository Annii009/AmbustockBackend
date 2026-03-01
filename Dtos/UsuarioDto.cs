namespace AmbustockBackend.Dtos
{
    public class UsuarioDto
    {
        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; }
        public string Rol { get; set; }
        public string Email { get; set; }
        public string? Password { get; set; }
    }
}