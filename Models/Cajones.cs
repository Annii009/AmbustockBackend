namespace AmbustockBackend.Models
{
    public class Cajones
    {
        public int IdCajon { get; set; }
        public string NombreCajon { get; set; }
        public int IdZona { get; set; }
        public Zonas Zona { get; set; }
    }
}