namespace GYMaster_API.DTO
{
    public class SuscripcionesDTO
    {
        public string UsuarioUsuarios { get; set; } = null!;

        public string UsuarioGimnasios { get; set; } = null!;

        public string TipoMembresia { get; set; } = null!;

        public DateTime FechaRenovacion { get; set; }
    }
}
