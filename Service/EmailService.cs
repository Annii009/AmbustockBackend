using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using AmbustockBackend.Models;
using AmbustockBackend.Data;

namespace AmbustockBackend.Service
{
    public class EmailService
    {
        private readonly EmailSettings _emailSettings;
        private readonly AmbustockContext _context;
        private readonly ILogger<EmailService> _logger;

        public EmailService(
            IOptions<EmailSettings> emailSettings,
            AmbustockContext context,
            ILogger<EmailService> logger)
        {
            _emailSettings = emailSettings.Value;
            _context = context;
            _logger = logger;
        }

        public async Task EnviarCorreoReposicionAsync(int idCorreo, Reposicion reposicion, List<string>? fotosBase64 = null)
        {
            try
            {
                var correo = await _context.Correos
                    .Include(c => c.Usuarios)
                    .Include(c => c.Materiales)
                    .FirstOrDefaultAsync(c => c.IdCorreo == idCorreo);

                if (correo?.Usuarios == null || string.IsNullOrEmpty(correo.Usuarios.Email))
                {
                    _logger.LogWarning($"No se encontró usuario o email para IdCorreo: {idCorreo}");
                    return;
                }

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.SenderEmail));
                message.To.Add(MailboxAddress.Parse(correo.Usuarios.Email));
                message.Subject = $"Nueva reposición registrada - {reposicion.NombreMaterial}";

                var fotoCids = new List<string>();
                if (fotosBase64 != null)
                {
                    for (int i = 0; i < fotosBase64.Count; i++)
                        fotoCids.Add($"foto{i}@ambustock");
                }

                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = GenerarHtml(correo, reposicion, fotoCids)
                };

                if (fotosBase64 != null)
                {
                    for (int i = 0; i < fotosBase64.Count; i++)
                    {
                        var bytes = Convert.FromBase64String(fotosBase64[i]);
                        var imagen = bodyBuilder.LinkedResources.Add(
                            $"foto{i}.jpg",
                            bytes,
                            new MimeKit.ContentType("image", "jpeg")
                        );
                        imagen.ContentId = fotoCids[i];
                        imagen.ContentDisposition = new MimeKit.ContentDisposition(MimeKit.ContentDisposition.Inline);
                    }
                }

                message.Body = bodyBuilder.ToMessageBody();

                using var client = new SmtpClient();
                await client.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.SmtpPort, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_emailSettings.SenderEmail, _emailSettings.SenderPassword);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);

                _logger.LogInformation($"Email enviado a {correo.Usuarios.Email} — IdCorreo: {idCorreo}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al enviar email — IdCorreo: {idCorreo}");
            }
        }


        private string GenerarHtml(Correo correo, Reposicion reposicion, List<string> fotoCids)
        {
            var fotosHtml = "";
            if (fotoCids.Count > 0)
            {
                var imgs = string.Join("", fotoCids.Select(cid => $@"
            <img src='cid:{cid}' 
                 style='max-width:100%;border-radius:8px;margin:8px 0;display:block;' />"));

                fotosHtml = $@"
            <div class='info-box'>
                <p class='label'>Fotos de evidencia ({fotoCids.Count}):</p>
                {imgs}
            </div>";
            }

            return $@"
            <!DOCTYPE html>
            <html>
            <head>
                <style>
                    body {{ font-family: Arial, sans-serif; color: #333; }}
                    .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                    .header {{ background: linear-gradient(135deg, #c0392b, #8e1a11); color: white; padding: 30px; text-align: center; border-radius: 10px 10px 0 0; }}
                    .content {{ background: #f9f9f9; padding: 30px; border-radius: 0 0 10px 10px; }}
                    .info-box {{ background: white; padding: 20px; margin: 15px 0; border-radius: 8px; box-shadow: 0 2px 4px rgba(0,0,0,0.1); }}
                    .label {{ font-weight: bold; color: #c0392b; margin-bottom: 4px; }}
                    .footer {{ text-align: center; margin-top: 20px; color: #666; font-size: 12px; }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <div class='header'>
                        <h1>Nueva Reposición Registrada</h1>
                        <p>AmbuStock - Sistema de Gestión</p>
                    </div>
                    <div class='content'>
                        <div class='info-box'>
                            <p class='label'>Responsable:</p>
                            <p>{correo.Usuarios?.NombreUsuario ?? "N/A"}</p>

                            <p class='label'>Material:</p>
                            <p>{reposicion.NombreMaterial ?? "N/A"}</p>

                            <p class='label'>Cantidad:</p>
                            <p>{reposicion.Cantidad ?? 0} unidades</p>

                            <p class='label'>Tipo de problema:</p>
                            <p>{correo.TipoProblema ?? "N/A"}</p>

                            <p class='label'>Fecha:</p>
                            <p>{DateTime.Now:dd/MM/yyyy HH:mm}</p>

                            {(string.IsNullOrEmpty(reposicion.Comentarios) ? "" : $@"
                            <p class='label'>Comentarios:</p>
                            <p>{reposicion.Comentarios}</p>")}
                        </div>
                        {fotosHtml}
                        <p style='text-align:center;margin-top:20px;'>
                            Accede al sistema para gestionar la reposición.
                        </p>
                    </div>
                    <div class='footer'>
                        <p>Correo automático — no responder.</p>
                        <p>AmbuStock &copy; {DateTime.Now.Year}</p>
                    </div>
                </div>
            </body>
            </html>";
        }
    }
}
