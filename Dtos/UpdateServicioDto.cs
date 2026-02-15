namespace AmbustockBackend.Dtos
{
    public class UpdateServicioDto
    {
        public DateTime FechaHora { get; set; }
        public string NombreServicio { get; set; }
        public int? IdResponsable { get; set; }
    }
}