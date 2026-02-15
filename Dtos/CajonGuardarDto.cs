namespace AmbustockBackend.Dtos
{
    public class CajonGuardarDto
    {
        public string NombreCajon { get; set; }
        public List<MaterialGuardarDto> Materiales { get; set; }
    }
}