namespace SpaCitasAPI.DTOs
{
    public class CitaDTO
    {
        public int Id { get; set; }
        public string PacienteNombre { get; set; }
        public string ServicioNombre { get; set; }
        public string TerapeutaNombre { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan Hora { get; set; }
        public int DuracionMinutos { get; set; }
        public string Estado { get; set; }
        public string TiempoRestante { get; set; }
    }
}
