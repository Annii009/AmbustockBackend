namespace AmbustockBackend.Dtos
{
    public class ReposicionDto
    {
        public int IdReposicion { get; set; }
        public int IdCorreo { get; set; }
        public string NombreMaterial { get; set; }
        public int? Cantidad { get; set; }
        public string Comentarios { get; set; }
        public string FotoEvidenciaBase64 { get; set; }
        public List<ZonaDto> Zonas { get; set; }
    }
}