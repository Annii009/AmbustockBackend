namespace AmbustockBackend.Models
{
    public class Servicio
    {
        public int IdServicio { get; set; }
        public DateTime FechaHora { get; set; }
        public string NombreServicio { get; set; }
        public int? IdResponsable { get; set; }
        public Responsable Responsable { get; set; }
    }
}