namespace AmbustockBackend.Dtos
{
    public class DetalleCorreoDto
    {
        public int IdDetalleCorreo { get; set; }
        public int IdMaterial { get; set; }
        public string NombreMaterial { get; set; }
        public int CantidadMaterial { get; set; }
        public int IdCorreo { get; set; }
        public string TipoProblema { get; set; }
    }
}