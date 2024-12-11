using GYMaster_API.DTO;



namespace GYMaster_API.Services
{
    public interface IUserRepository
    {
        UsuariosDTO obtenerUsuarios(string email);
        

    }
}
