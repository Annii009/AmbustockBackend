namespace AmbustockBackend.Dtos
{
    public class ServicioAmbulanciaDto
    {
        public int IdServicioAmbulancia { get; set; }
        public int IdAmbulancia { get; set; }
        public string NombreAmbulancia { get; set; }
        public string Matricula { get; set; }
        public int IdServicio { get; set; }
        public DateTime FechaHoraServicio { get; set; }
        public string NombreServicio { get; set; }
    }
}