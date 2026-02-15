namespace AmbustockBackend.Dtos
{
    public class ResponsableDto
    {
        public int IdResponsable { get; set; }
        public string NombreResponsable { get; set; }
        public DateTime? FechaServicio { get; set; }
        public int? IdServicio { get; set; }
        public int? IdUsuario { get; set; }
        public int? IdReposicion { get; set; }
    }
}