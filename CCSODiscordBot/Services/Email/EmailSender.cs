using System;
using System.Net;
using System.Net.Mail;

namespace CCSODiscordBot.Services.Email
{
    public class EmailSender
    {
        private ConfigHandlingService _configHandlingService;
        public EmailSender(ConfigHandlingService config)
        {
            _configHandlingService = config;
        }

        public async Task SendVerifyCode(int code, string email)
        {
            var smtpClient = new SmtpClient(_configHandlingService.SMTPAddr)
            {
                Port = (int) _configHandlingService.SMTPPort,
                Credentials = new NetworkCredential(_configHandlingService.SMTPEmail, _configHandlingService.SMTPPassword),
                EnableSsl = true
            };
        }
    }
}

