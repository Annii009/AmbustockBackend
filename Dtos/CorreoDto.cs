namespace AmbustockBackend.Dtos
{
    public class CorreoDto
    {
        public int IdCorreo { get; set; }
        public DateTime? FechaAlerta { get; set; }
        public string TipoProblema { get; set; }
        public int? IdMaterial { get; set; }
        public string NombreMaterial { get; set; }
        public int? IdUsuario { get; set; }
        public string NombreUsuario { get; set; }
        public int? IdReposicion { get; set; }
    }
}