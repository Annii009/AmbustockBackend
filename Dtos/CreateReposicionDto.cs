namespace AmbustockBackend.Dtos
{
    public class CreateReposicionDto
    {
        public int IdCorreo { get; set; }
        public string NombreMaterial { get; set; }
        public int? Cantidad { get; set; }
        public string Comentarios { get; set; }
        public string FotoEvidenciaBase64 { get; set; }
    }
}