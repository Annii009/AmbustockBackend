namespace AmbustockBackend.Dtos
{
    public class GuardarRevisionDto
    {
        public int IdAmbulancia { get; set; }
        public int IdServicio { get; set; }
        public string NombreResponsable { get; set; }
        public DateTime FechaRevision { get; set; }
        public List<ZonaGuardarDto> Zonas { get; set; }
    }
}
