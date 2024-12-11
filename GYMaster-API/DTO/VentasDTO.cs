namespace GYMaster_API.DTO
{
    public class VentasDTO
    {
        public string UsuarioGimnasios { get; set; } = null!;

        public string Concepto { get; set; } = null!;

        public decimal Total { get; set; }

        public DateTime Fecha { get; set; }

        public TimeSpan Hora { get; set; }
    }
}
