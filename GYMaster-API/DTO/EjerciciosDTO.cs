namespace GYMaster_API.DTO
{
    public class EjerciciosDTO
    {
        public int IdEjercicios { get; set; }

        public string Ejercicio { get; set; } = null!;

        public string ZonaTrabajo { get; set; } = null!;

        public string Repeticiones { get; set; } = null!;

        public string Series { get; set; } = null!;

        public string NombreMusculo { get; set; } = null!;
    }
}
