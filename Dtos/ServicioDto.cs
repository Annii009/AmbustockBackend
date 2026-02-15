namespace AmbustockBackend.Dtos
{
    public class ServicioDto
    {
        public int IdServicio { get; set; }
        public DateTime FechaHora { get; set; }
        public string NombreServicio { get; set; }
        public int? IdResponsable { get; set; }
        public string NombreResponsable { get; set; }
    }
}