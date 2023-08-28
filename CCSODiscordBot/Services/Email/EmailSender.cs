using System.Net;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

namespace CCSODiscordBot.Services.Email
{
    public class EmailSender
    {
        private readonly ConfigHandlingService _configHandlingService;
        public EmailSender(ConfigHandlingService config)
        {
            _configHandlingService = config;
        }
        /// <summary>
        /// Send a verification code via email
        /// </summary>
        /// <param name="code"></param>
        /// <param name="recipient"></param>
        /// <param name="guild"></param>
        /// <param name="username"></param>
        public void SendVerifyCode(int code, string recipient, string guild, string username)
        {
            string emailBody = "Hi,\n" +
                    "Your verification code is " + code + " for the " + guild + " Discord server.\n" +
                    "Use the /verify slash command in Discord with the code to validate you account. You can use this command in any channel that you can send messages in.\n" +
                    "NOTE: if you are on a mobile device, be sure to tap on the /verify popup while typing the /verify command. Your message should be responded to immediately by the bot.\n" +
                    "This code was requested by " + username;

            var message = new MimeMessage();
            message.From.Add(MailboxAddress.Parse(_configHandlingService.SMTPEmail));
            message.To.Add(MailboxAddress.Parse(recipient));
            message.Subject = "CCSO Bot Verification Code";
            message.Body = new TextPart(TextFormat.Plain) { Text = emailBody };

            SmtpClient smtp = new SmtpClient();
            try
            {
                smtp.Connect(_configHandlingService.SMTPAddr, (int) _configHandlingService.SMTPPort, SecureSocketOptions.StartTlsWhenAvailable);
                smtp.Authenticate(_configHandlingService.SMTPUser, _configHandlingService.SMTPPassword);
                smtp.Send(message);
            }
            catch (AuthenticationException e)
            {
                Console.WriteLine("Auth error when sending email. Config details:");
                Console.WriteLine("SMTP User: " + _configHandlingService.SMTPUser);
                Console.WriteLine("SMTP Pass: " + _configHandlingService.SMTPPassword);
                Console.WriteLine("SMTP Email: " + _configHandlingService.SMTPEmail);
                Console.WriteLine("SMTP Server: " + _configHandlingService.SMTPAddr);
                Console.WriteLine("SMTP Port: " + (int) _configHandlingService.SMTPPort);
                Console.WriteLine("Error msg: " + e.Message);
                throw;
            }
            catch
            {
                throw;
            }
            finally
            {
                // Always disconnect
                smtp.Disconnect(true);
            }

            // Log this event:
            Console.WriteLine("Email sent to " + recipient + " requested by " + username);
        }
    }
}
