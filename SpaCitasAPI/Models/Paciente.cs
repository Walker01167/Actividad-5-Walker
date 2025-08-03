using System.ComponentModel.DataAnnotations;

namespace SpaCitasAPI.Models
{
    public class Paciente
    {
        public int PacienteId { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [EmailAddress]
        public string Correo { get; set; }

        [Phone]
        public string Telefono { get; set; }

        public ICollection<Cita>? Citas { get; set; }
    }
}
