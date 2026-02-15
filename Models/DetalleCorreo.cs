namespace AmbustockBackend.Models
{
    public class DetalleCorreo
    {
        public int IdDetalleCorreo { get; set; }
        public int IdMaterial { get; set; }
        public int IdCorreo { get; set; }
        
        public Materiales Materiales { get; set; }
        public Correo Correo { get; set; }
    }
}