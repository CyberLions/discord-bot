using System;
using System.Net;
using System.Net.Mail;

namespace CCSODiscordBot.Services.Email
{
    public class EmailSender
    {
        private readonly ConfigHandlingService _configHandlingService;
        public EmailSender(ConfigHandlingService config)
        {
            _configHandlingService = config;
        }

        public void SendVerifyCode(int code, string recipient, string guild, string username)
        {
            // Build the SMTP client:
            var smtpClient = new SmtpClient(_configHandlingService.SMTPAddr)
            {
                Port = (int) _configHandlingService.SMTPPort,
                Credentials = new NetworkCredential(_configHandlingService.SMTPEmail, _configHandlingService.SMTPPassword),
                EnableSsl = true
            };

            // Send email
            smtpClient.Send(_configHandlingService.SMTPEmail, recipient, "CCSO Bot Verification Code",
                "Hi!\n" +
                "Your verification code is: " + code + " for the " + guild + " discord server. Use the /verify command in Discord with the code to validate you account.\n" +
                "This code was requested by " + username
            );


            // Log this event:
            Console.WriteLine("Email sent to " + recipient + " requested by " + username);

            // Clear the SMTP client from RAM:
            smtpClient.Dispose();
        }
    }
}
