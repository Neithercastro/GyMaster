using GYMaster_API.DTO;

namespace GYMaster_API.Services
{
    public interface IEmailService
    {
        void SendConfirmationEmail(string correo, string url, CredencialesDTO Object);
        void SendRecoveryEmail(string correo, CredencialesDTO Object);
        void SendReceiptEmail(string correo, string[] productos, CredencialesDTO Object);
    }
}
