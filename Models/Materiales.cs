namespace AmbustockBackend.Models
{
    public class Materiales
    {
        public int IdMaterial { get; set; }
        public string NombreProducto { get; set; }
        public int Cantidad { get; set; }
        public int IdZona { get; set; }
        public Zonas Zona { get; set; }
        public int? IdCajon { get; set; }
        public Cajones Cajon { get; set; }
    }
}