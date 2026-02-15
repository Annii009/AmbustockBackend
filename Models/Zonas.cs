using System;

namespace AmbustockBackend.Models
{
    public class Zonas
    {
        public int IdZona { get; set; }
        public string NombreZona { get; set; }
        public int IdAmbulancia { get; set; }
        public Ambulancia Ambulancia { get; set; }
    }
}