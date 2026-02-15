namespace AmbustockBackend.Dtos
{
    public class CreateServicioDto
    {
        public DateTime FechaHora { get; set; }
        public string NombreServicio { get; set; }
        public int? IdResponsable { get; set; }
    }
}