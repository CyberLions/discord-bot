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
            using (var smtpClient = new SmtpClient(_configHandlingService.SMTPAddr)
            {
                Port = (int)_configHandlingService.SMTPPort,
                Credentials = new NetworkCredential(_configHandlingService.SMTPEmail, _configHandlingService.SMTPPassword),
                EnableSsl = true
            })
            // Using the SMTP client, execute the following. Then dispose of the object from RAM.
            { 

                // Send email
                smtpClient.Send(_configHandlingService.SMTPEmail, recipient, "CCSO Bot Verification Code",
                    "Hi,\n" +
                    "Your verification code is " + code + " for the " + guild + " Discord server.\n" +
                    "Use the /verify command in Discord with the code to validate you account. You can use this command in any channel that you can send messages in.\n" +
                    "This code was requested by " + username
                );
            }

            // Log this event:
            Console.WriteLine("Email sent to " + recipient + " requested by " + username);
        }
    }
}
