namespace AmbustockBackend.Models
{
    public class Revision
    {
        public int Id_revision { get; set; }
        public int Id_ambulancia { get; set; }
        public int Id_servicio { get; set; }
        public string Nombre_Responsable { get; set; }
        public DateTime Fecha_Revision { get; set; }
        public int Total_Materiales { get; set; }
        public int Materiales_Revisados { get; set; }
        public string Estado { get; set; }
    }
}