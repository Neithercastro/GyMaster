using GYMaster_API.DTO;
using MailKit.Security;
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;

namespace GYMaster_API.Services
{
    public class EmailService : IEmailService
    {

        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void SendConfirmationEmail(string toEmail, string URL, CredencialesDTO Objeto)
        {
            // Configuración del servidor SMTP
            using var smtp = new SmtpClient();
            smtp.Connect(
                _configuration.GetSection("Email:Host").Value,
                Convert.ToInt32(_configuration.GetSection("Email:Port").Value),
                SecureSocketOptions.StartTls
            );

            // Autenticación con nuestra cuenta de correo
            smtp.Authenticate(
                _configuration.GetSection("Email:UserName").Value,
                _configuration.GetSection("Email:PassWord").Value
            );

            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_configuration.GetSection("Email:UserName").Value));
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = "GYMaster: Confirmación de correo";
            email.Body = new TextPart(TextFormat.Html)
            {
                Text = $"Hola {Objeto.Usuario}, bienvenido a GYMaster." +
                $"<br />Para continuar debe confirmar su correo electrónico entrando al siguiente enlace:" +
                $"<br />{URL}"
            };

            smtp.Send(email);
            smtp.Disconnect(true);
        }

        public void SendRecoveryEmail(string toEmail, CredencialesDTO Objeto)
        {
            // Configuración del servidor SMTP
            using var smtp = new SmtpClient();
            smtp.Connect(
                _configuration.GetSection("Email:Host").Value,
                Convert.ToInt32(_configuration.GetSection("Email:Port").Value),
                SecureSocketOptions.StartTls
            );

            // Autenticación con nuestra cuenta de correo
            smtp.Authenticate(
                _configuration.GetSection("Email:UserName").Value,
                _configuration.GetSection("Email:PassWord").Value
            );

            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_configuration.GetSection("Email:UserName").Value));
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = "GYMaster: Recuperación de contraseña";
            email.Body = new TextPart(TextFormat.Html)
            {
                Text = $"Hola {Objeto.Usuario}, su contraseña es: {Objeto.Contrasena}"
            };

            smtp.Send(email);
            smtp.Disconnect(true);
        }

        public void SendReceiptEmail(string toEmail, string[] productos, CredencialesDTO Objeto)
        {
            // Configuración del servidor SMTP
            using var smtp = new SmtpClient();
            smtp.Connect(
                _configuration.GetSection("Email:Host").Value,
                Convert.ToInt32(_configuration.GetSection("Email:Port").Value),
                SecureSocketOptions.StartTls
            );

            // Autenticación con nuestra cuenta de correo
            smtp.Authenticate(
                _configuration.GetSection("Email:UserName").Value,
                _configuration.GetSection("Email:PassWord").Value
            );

            string Contenido = "";
            string[] Divisiones;

            foreach(string Producto in productos)
            {
                Divisiones = Producto.Split(',');
                Contenido += Divisiones[0] + ":<br />&nbsp;&nbsp;&nbsp;&nbsp;Cantidad: " + Divisiones[1] + "<br />&nbsp;&nbsp;&nbsp;&nbsp;Importe: $" + Divisiones[2] + "<br />";
            }

            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_configuration.GetSection("Email:UserName").Value));
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = "GYMaster: Recibo de su compra";
            email.Body = new TextPart(TextFormat.Html)
            {
                Text = $"Hola {Objeto.Usuario}, su pedido ha sido realizado con éxito." +
                $" Puede recoger sus productos en el local de su Gimnasio." +
                $"<br />Aquí están los detalles de su pedido:<br /><br />{Contenido}"
            };

            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}
