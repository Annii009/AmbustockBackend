namespace AmbustockBackend.Models
{
    public class ServicioAmbulancia
    {
        public int IdServicioAmbulancia { get; set; }
        public int IdAmbulancia { get; set; }
        public int IdServicio { get; set; }
        
        public Ambulancia Ambulancia { get; set; }
        public Servicio Servicio { get; set; }
    }
}