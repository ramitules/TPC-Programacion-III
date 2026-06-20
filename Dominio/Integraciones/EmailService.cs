using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class EmailService
    {
        private MailMessage email;
        private SmtpClient server;

        public EmailService()
        {
            server = new SmtpClient();
            server.Credentials = new NetworkCredential("1e6d2fd38c183f", "1fff5ef2499edb");
            server.EnableSsl = true;
            server.Port = 2525;
            server.Host = "sandbox.smtp.mailtrap.io";
        }

        public void armarCorreo(string destinatario, string asunto, string cuerpo)
        {
            email = new MailMessage();
            email.From = new MailAddress("tpcnoresponder@gmail.com");
            email.To.Add(destinatario);
            email.Subject = asunto;
            email.IsBodyHtml = true;
            //email.Body = cuerpo;
            email.Body = "<html><body>" + cuerpo + "</body></html>";
        }

        public void enviarCorreo()
        {
            try
            {
                server.Send(email);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
