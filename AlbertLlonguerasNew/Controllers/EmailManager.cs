using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Umbraco.Core.Logging;

namespace AlbertLlonguerasNew.Controllers
{
    public class EmailManager
    {
        public void SendEmail(string nom, string correu, string link, string linkName, string match)
        {
            var dataConf = this.GetDataSendMail();
            if (!dataConf.Any()) return;
            var port = Int32.Parse(dataConf["port"]);
            var host = dataConf["host"];
            var to = correu;
            var pass = dataConf["passTo"];

            var sb = new StringBuilder();
            sb.AppendFormat("<p>{0} enrecorda't de fer la porra pel partit {1}</p>", nom, match);
            sb.AppendFormat("<p>Porra disponible a: <a href=\"{0}\">{1}</a></p>", link, linkName);

            var smtp = new SmtpClient(host, port) { EnableSsl = true };
            var credentials = new NetworkCredential(to, pass);

            var message = new MailMessage
            {
                From = new MailAddress(dataConf["mailTo"]),
                To = { new MailAddress(to) },
                Subject = "Nova porra " + match,
                Body = sb.ToString(),
                IsBodyHtml = true
            };

            try
            {
                smtp.EnableSsl = true;
                smtp.Credentials = credentials;
                smtp.Send(message);
            }
            catch (SmtpException smtpEx)
            {
            }
        }

        private Dictionary<string, string> GetDataSendMail()
        {
            var dataConf = new Dictionary<string, string>();

            var mailTo = ConfigurationManager.AppSettings["sendMail:mail"];
            var passTo = ConfigurationManager.AppSettings["sendMail:password"];
            var host = ConfigurationManager.AppSettings["sendMail:host"];
            var port = ConfigurationManager.AppSettings["sendMail:port"];

            LogHelper.Info(MethodBase.GetCurrentMethod().DeclaringType, string.Format("SMTP configuration: {0}-{1}", host, port));

            if (string.IsNullOrEmpty(mailTo) || string.IsNullOrEmpty(passTo) || string.IsNullOrEmpty(host) ||
                string.IsNullOrEmpty(port)) return dataConf;
            dataConf["mailTo"] = mailTo;
            dataConf["passTo"] = passTo;
            dataConf["host"] = host;
            dataConf["port"] = port;

            return dataConf;
        }

    }
}
