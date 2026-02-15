namespace AmbustockBackend.Dtos
{
    public class RevisionAmbulanciaDto
    {
        public int IdAmbulancia { get; set; }
        public string NombreAmbulancia { get; set; }
        public string Matricula { get; set; }
        public List<ZonaRevisionDto> Zonas { get; set; }
    }

}