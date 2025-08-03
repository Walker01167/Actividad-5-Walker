using System.ComponentModel.DataAnnotations;

namespace SpaCitasAPI.Models
{
    public class Terapeuta
    {
        public int TerapeutaId { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        public string Especialidad { get; set; }

        public ICollection<Cita> Citas { get; set; } = new List<Cita>();

    }
}
