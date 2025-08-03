using System.ComponentModel.DataAnnotations;

namespace SpaCitasAPI.Models
{
    public class Servicio
    {
        public int ServicioId { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        public decimal Precio { get; set; }

        [Range(1, 480)]
        public int DuracionEnMinutos { get; set; }

        public ICollection<Cita> Citas { get; set; } = new List<Cita>();
    }
}
