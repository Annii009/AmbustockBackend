namespace AmbustockBackend.Dtos
{
    public class ZonaDto
    {
        public int IdZona { get; set; }
        public string NombreZona { get; set; }
        public int IdAmbulancia { get; set; }
        public string AmbulanciaNombre { get; set; }
        public List<CajonDto> Cajones { get; set; }
        public List<MaterialDto> Materiales { get; set; }
    }
}