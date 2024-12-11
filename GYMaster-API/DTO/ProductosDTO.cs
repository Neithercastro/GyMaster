namespace GYMaster_API.DTO
{
    public class ProductosDTO
    {
        public string UsuarioGimnasios { get; set; } = null!;

        public string Nombre { get; set; } = null!;

        public string Descripcion { get; set; } = null!;

        public decimal Precio { get; set; }

        public int Stock { get; set; }

        public int IdEstatus { get; set; }
    }
}
