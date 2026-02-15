namespace AmbustockBackend.Models
{
    public class Usuarios
    {
        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; }
        public string Rol { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int? IdResponsable { get; set; }
        public int? IdCorreo { get; set; }
        
        public Responsable Responsable { get; set; }
        public Correo Correo { get; set; }
    }
}