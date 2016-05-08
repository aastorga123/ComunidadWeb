using MiComunidad.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MiComunidad.Controllers
{
    public class EnviarCorreoController : Controller
    {
        public static Boolean EnviarCorreo(String asunto, String mensaje, System.Collections.Specialized.StringCollection lstDestinatarios)
        {
            MailAddressCollection destinatarios = new MailAddressCollection();

            foreach (string destinatario in lstDestinatarios)
                destinatarios.Add(new MailAddress(destinatario));

            return EnviarCorreo(destinatarios, asunto, mensaje);
        }

        public static Boolean EnviarCorreo(MailAddressCollection destinatarios, String asunto, String mensaje)
        {
            bool response = false;
            try
            {
                MailMessage mail = new MailMessage();

                //string destinatarios_finales = System.Configuration.ConfigurationManager.AppSettings["CorreoProblemaPromover"];
                String server = System.Configuration.ConfigurationManager.AppSettings["ServerSMTP"];
                Int32 puerto = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["PuertoSMTP"]);
                String from = System.Configuration.ConfigurationManager.AppSettings["FromSMTP"];
                mail.From = new MailAddress(from);

                foreach (MailAddress correo in destinatarios)
                    mail.To.Add(correo);

                mail.Body = mensaje;
                mail.IsBodyHtml = true;
                mail.Subject = asunto;
                

                SmtpClient smtpServidor = new SmtpClient(server, puerto);

                smtpServidor.Send(mail);
                response = true;
            }
            catch (Exception e)
            {
                //Global.Log.Error("EnviarCorreo: Error al enviar email, Excepcion: " + e.Message);
                response = false;
            }
            return response;
        }

        public static Boolean EnviarCorreoRegistroUsuario(String asunto, String mensaje, Usuario usuario, string carpetaarchivo)
        {
            bool response = false;
            try
            {
                MailMessage mail = new MailMessage();

                //string destinatarios_finales = System.Configuration.ConfigurationManager.AppSettings["CorreoProblemaPromover"];
                String server = System.Configuration.ConfigurationManager.AppSettings["ServerSMTP"];
                Int32 puerto = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["PuertoSMTP"]);
                String from = System.Configuration.ConfigurationManager.AppSettings["FromSMTP"];
                mail.From = new MailAddress(from);

                mail.To.Add(usuario.EmailUsuario);

                mail.Body = mensaje;
                mail.IsBodyHtml = true;
                mail.Subject = asunto;

                AlternateView htmlView =
                AlternateView.CreateAlternateViewFromString(mail.Body,
                             Encoding.UTF8,
                             MediaTypeNames.Text.Html);


                LinkedResource img =
                new LinkedResource(@carpetaarchivo,
                         MediaTypeNames.Image.Jpeg);
                img.ContentId = "imagen";
                htmlView.LinkedResources.Add(img);
                mail.AlternateViews.Add(htmlView);


                using (SmtpClient smtpServidor = new SmtpClient(server, puerto))
                {
                    smtpServidor.UseDefaultCredentials = false;
                    smtpServidor.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtpServidor.EnableSsl = true;
                    smtpServidor.Credentials = new NetworkCredential(from, "mokona123");
                    smtpServidor.Timeout = 1000000000;
                    smtpServidor.ServicePoint.MaxIdleTime = 1;
                    smtpServidor.Send(mail);
                }

                mail.Dispose();
                
                response = true;
            }
            catch (Exception e)
            {
                //Global.Log.Error("EnviarCorreo: Error al enviar email, Excepcion: " + e.Message);
                response = false;
            }
            return response;
        }
    }
}