using BookStoreWebApp.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace BookStoreWebApp.Service
{
    public class EmailService : IEmailService
    {
        private const string templatePath = @"EmailTemplate/{0}.html";
        private readonly SMTPConfigModel _smtpconfig;

        public async Task SendTestEmail(UserEmailOptions userEmailOptions)
        {
            userEmailOptions.Subject = UpdatePlaceholder("Hello {{UserName}}, This is Test Email",userEmailOptions.PlaceHolder);
            userEmailOptions.Body = UpdatePlaceholder(GetEmailBody("TestEmail"), userEmailOptions.PlaceHolder);

            await sendEmail(userEmailOptions);
        }
        public async Task SendEmailConfirmation(UserEmailOptions userEmailOptions)
        {
            userEmailOptions.Subject = UpdatePlaceholder("Hello {{UserName}}, Please Confirm Your Email Id",userEmailOptions.PlaceHolder);
            userEmailOptions.Body = UpdatePlaceholder(GetEmailBody("EmailConfirmation"), userEmailOptions.PlaceHolder);

            await sendEmail(userEmailOptions);
        }

        public EmailService(IOptions<SMTPConfigModel> smtpconfig)
        {
            _smtpconfig = smtpconfig.Value;
        }
        private async Task sendEmail(UserEmailOptions userEmailOptions)
        {
            MailMessage mail = new MailMessage
            {
                Subject = userEmailOptions.Subject,
                Body = userEmailOptions.Body,
                From = new MailAddress(_smtpconfig.SenderAddress, _smtpconfig.SenderDisplayName),
                IsBodyHtml = _smtpconfig.IsBodyHTML
            };
            foreach (var toEmail in userEmailOptions.ToEmail)
            {
                mail.To.Add(toEmail);
            }

            NetworkCredential networkCredential = new NetworkCredential(_smtpconfig.UserName, _smtpconfig.Password);

            SmtpClient smtpClient = new SmtpClient
            {
                Host = _smtpconfig.Host,
                Port = _smtpconfig.Port,
                EnableSsl = _smtpconfig.EnableSSL,
                UseDefaultCredentials = _smtpconfig.UseDefaultCredentials,
                Credentials = networkCredential
            };
            mail.BodyEncoding = System.Text.Encoding.Default;

            await smtpClient.SendMailAsync(mail);
        }
        private string GetEmailBody(string Template)
        {
            var body = File.ReadAllText(string.Format(templatePath, Template));
            return body;
        }

        private string UpdatePlaceholder(string text , List<KeyValuePair<string,string>> keyValuePairs) 
        {
            if (!String.IsNullOrEmpty(text) && keyValuePairs != null)
            {
                foreach(var placeholder in keyValuePairs)
                {
                    if (text.Contains(placeholder.Key))
                    {
                        text = text.Replace(placeholder.Key, placeholder.Value);
                    }
                }
            }

            return text;
        }
    }
}
