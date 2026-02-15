namespace AmbustockBackend.Dtos
{
    public class CajonDto
    {
        public int IdCajon { get; set; }
        public string NombreCajon { get; set; }
        public int IdZona { get; set; }
        public string NombreZona { get; set; }
        public List<MaterialDto> Materiales { get; set; }
    }
}