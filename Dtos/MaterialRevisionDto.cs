namespace AmbustockBackend.Dtos
{

    public class MaterialRevisionDto
    {
        public int IdMaterial { get; set; }
        public string NombreProducto { get; set; }
        public int Cantidad { get; set; }
        public bool Revisado { get; set; }
    }
}