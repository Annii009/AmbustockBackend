namespace AmbustockBackend.Models
{
    public class Correo
    {
        public int IdCorreo { get; set; }
        public DateTime? FechaAlerta { get; set; }
        public string TipoProblema { get; set; }
        public int? IdMaterial { get; set; }
        public int? IdUsuario { get; set; }
        public int? IdReposicion { get; set; }
        
        public Materiales Materiales { get; set; }
        public Usuarios Usuarios { get; set; }
        public Reposicion Reposicion { get; set; }
    }
}