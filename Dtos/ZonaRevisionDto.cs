namespace AmbustockBackend.Dtos
{
    

    public class ZonaRevisionDto
    {
        public int IdZona { get; set; }
        public string NombreZona { get; set; }
        public List<CajonRevisionDto> Cajones { get; set; }
        public List<MaterialRevisionDto> Materiales { get; set; }
    }
}