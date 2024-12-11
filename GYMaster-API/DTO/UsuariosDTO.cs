namespace GYMaster_API.DTO
{
    public class UsuariosDTO
    {
        public string Nombres { get; set; } = null!;

        public string Apellidos { get; set; } = null!;

        public DateTime FechaNacimiento { get; set; }

        public string Sexo { get; set; } = null!;

        public int Estatura { get; set; }

        public int Peso { get; set; }

        public string Telefono { get; set; } = null!;

        public string Correo { get; set; } = null!;

        public string Usuario1 { get; set; } = null!;

        public string Contrasena { get; set; } = null!;

        public string UsuarioGimnasios { get; set; } = null!;

        public DateTime FechaRegistro { get; set; }

        public string CodigoTarjeta { get; set; } = null!;
    }
}
