using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Microsoft.Extensions.Options;
using AmbustockBackend.Models;
using AmbustockBackend.Repositories;

namespace AmbustockBackend.Service
{
    public interface IEmailService
    {
        Task EnviarCorreoReposicionCompletadaAsync(Reposicion reposicion);
    }

    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;
        private readonly IUsuarioRepository _usuariosRepository;
        private readonly ILogger<EmailService> _logger;

        public EmailService(
            IOptions<EmailSettings> emailSettings,
            IUsuarioRepository usuariosRepository,
            ILogger<EmailService> logger)
        {
            _emailSettings = emailSettings.Value;
            _usuariosRepository = usuariosRepository;
            _logger = logger;
        }

        public async Task EnviarCorreoReposicionCompletadaAsync(Reposicion reposicion)
        {
            try
            {
                var admins = await _usuariosRepository.GetByRolAsync("Admin");
                var adminEmails = admins
                    .Where(u => !string.IsNullOrEmpty(u.Email))
                    .Select(u => u.Email)
                    .ToList();

                if (!adminEmails.Any())
                {
                    _logger.LogWarning("No se encontraron administradores con email configurado");
                    return;
                }

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.SenderEmail));
                
                foreach (var email in adminEmails)
                {
                    message.To.Add(MailboxAddress.Parse(email));
                }

                message.Subject = $"✅ Reposición Completada - {reposicion.NombreMaterial}";

                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = GenerarHtmlCorreo(reposicion)
                };

                message.Body = bodyBuilder.ToMessageBody();

                using var client = new SmtpClient();
                await client.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.SmtpPort, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_emailSettings.SenderEmail, _emailSettings.SenderPassword);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);

                _logger.LogInformation($"Correo de reposición enviado exitosamente. ID: {reposicion.IdReposicion}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al enviar correo de reposición. ID: {reposicion.IdReposicion}");
            }
        }

        private string GenerarHtmlCorreo(Reposicion reposicion)
        {
            return $@"
<!DOCTYPE html>
<html>
<head>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
        .header {{ background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; padding: 30px; text-align: center; border-radius: 10px 10px 0 0; }}
        .content {{ background: #f9f9f9; padding: 30px; border-radius: 0 0 10px 10px; }}
        .info-box {{ background: white; padding: 20px; margin: 15px 0; border-radius: 8px; box-shadow: 0 2px 4px rgba(0,0,0,0.1); }}
        .label {{ font-weight: bold; color: #667eea; }}
        .value {{ color: #333; margin-bottom: 10px; }}
        .footer {{ text-align: center; margin-top: 20px; color: #666; font-size: 12px; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>Reposición Completada</h1>
            <p>AmbuStock - Sistema de Gestión</p>
        </div>
        <div class='content'>
            <div class='info-box'>
                <p class='label'>Material:</p>
                <p class='value'>{reposicion.NombreMaterial ?? "N/A"}</p>
                
                <p class='label'>Cantidad:</p>
                <p class='value'>{reposicion.Cantidad ?? 0} unidades</p>
                
                <p class='label'>Fecha:</p>
                <p class='value'>{DateTime.Now:dd/MM/yyyy HH:mm}</p>
            </div>

            {(!string.IsNullOrEmpty(reposicion.Comentarios) ? $@"
            <div class='info-box'>
                <p class='label'>Comentarios:</p>
                <p class='value'>{reposicion.Comentarios}</p>
            </div>" : "")}

            <p style='text-align: center; margin-top: 20px;'>
                Por favor, revisa el sistema para más detalles.
            </p>
        </div>
        <div class='footer'>
            <p>Este es un correo automático, por favor no responder.</p>
            <p>AmbuStock &copy; {DateTime.Now.Year}</p>
        </div>
    </div>
</body>
</html>";
        }
    }
}
