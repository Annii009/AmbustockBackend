namespace AmbustockBackend.Dtos
{
    public class RevisionDetalleDto
    {
        public int IdRevision { get; set; }
        public string NombreAmbulancia { get; set; }
        public string Matricula { get; set; }
        public string NombreResponsable { get; set; }
        public DateTime FechaRevision { get; set; }
        public string Estado { get; set; }
        public int TotalMateriales { get; set; }
        public int MaterialesRevisados { get; set; }
        public List<ZonaRevisionDto> Zonas { get; set; }
    }
}
