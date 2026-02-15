namespace AmbustockBackend.Dtos
{
    public class MaterialDto
    {
        public int IdMaterial { get; set; }
        public string NombreProducto { get; set; }
        public int Cantidad { get; set; }
        public int IdZona { get; set; }
        public string NombreZona { get; set; }
        public int? IdCajon { get; set; }
        public string NombreCajon { get; set; }
        public bool Revisado { get; set; }
    }
}