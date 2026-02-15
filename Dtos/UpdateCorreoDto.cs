namespace AmbustockBackend.Dtos
{
    public class UpdateCorreoDto
    {
        public DateTime? FechaAlerta { get; set; }
        public string TipoProblema { get; set; }
        public int? IdMaterial { get; set; }
        public int? IdUsuario { get; set; }
        public int? IdReposicion { get; set; }
    }
}