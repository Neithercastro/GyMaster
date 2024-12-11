using GYMaster_API.DTO;
using GYMaster_API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace GYMaster_API.Services
{
    public class ObtenerUser : IUserRepository
    {

        private readonly GymasterContext _context;

        public ObtenerUser(GymasterContext context)
        {
            _context = context;
        }

        


        public void ConfigureServices(IServiceCollection services)
        {
            

            // Registrar la interfaz y su implementación
            services.AddScoped<IUserRepository, ObtenerUser>();

            
        }

        public UsuariosDTO obtenerUsuarios(string email)
        {
            throw new NotImplementedException();
        }
    }
}
