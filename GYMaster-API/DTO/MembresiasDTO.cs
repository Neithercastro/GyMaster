namespace GYMaster_API.DTO
{
    public class MembresiasDTO
    {
        public string UsuarioGimnasios { get; set; } = null!;

        public string Tipo { get; set; } = null!;

        public decimal Precio { get; set; }

        public int Periodo { get; set; }

        public int IdEstatus { get; set; }
    }
}
