namespace GYMaster_API.DTO
{
    public class CarritosdetalleDTO
    {
        public string UsuarioUsuarios { get; set; } = null!;

        public string UsuarioGimnasios { get; set; } = null!;

        public string NombreProductos { get; set; } = null!;

        public int Cantidad { get; set; }

        public decimal Subtotal { get; set; }
    }
}
