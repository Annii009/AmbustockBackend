namespace AmbustockBackend.Dtos
{
    public class ZonaGuardarDto
    {
        public string NombreZona { get; set; }
        public List<MaterialGuardarDto> Materiales { get; set; }
        public List<CajonGuardarDto> Cajones { get; set; }
    }
}