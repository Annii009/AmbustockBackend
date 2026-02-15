namespace AmbustockBackend.Dtos
{

    public class CajonRevisionDto
    {
        public int IdCajon { get; set; }
        public string NombreCajon { get; set; }
        public List<MaterialRevisionDto> Materiales { get; set; }
    }
}