using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Configuration;
using BitFlipping.UComments.Web;

namespace BitFlipping.UComments.Core.Services
{
    public class SendService
    {
        public SendService()
        {

        }

        public async Task SendEmail(MailMessage mail)
        {
            using(var smtpClient = new SmtpClient())
            {
                await smtpClient.SendMailAsync(mail);
            }
        }

        /// <summary>
        /// Sends email using default umbraco notification email
        /// </summary>
        /// <param name="to"></param>
        /// <param name="view"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task SendEmail(string to, string view, object model)
        {
            string html = ViewHelper.RenderPartialView(view, model);
            string from = UmbracoConfig.For.UmbracoSettings().Content.NotificationEmailAddress;
            using (var message = new MailMessage(from, to))
            {
                await this.SendEmail(message);
            }
        }
    }
}
