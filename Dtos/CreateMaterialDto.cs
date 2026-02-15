namespace AmbustockBackend.Dtos
{
    public class CreateMaterialDto
    {
        public string NombreProducto { get; set; }
        public int Cantidad { get; set; }
        public int IdZona { get; set; }
        public int? IdCajon { get; set; }
    }
}