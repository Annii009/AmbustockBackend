namespace AmbustockBackend.Models
{
    public class Reposicion
    {
        public int IdReposicion { get; set; }
        public int IdCorreo { get; set; }
        public string NombreMaterial { get; set; }
        public int? Cantidad { get; set; }
        public string Comentarios { get; set; }
        public byte[] FotoEvidencia { get; set; }
        
        public Correo Correo { get; set; }
    }
}