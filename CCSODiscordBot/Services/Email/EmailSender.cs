﻿using System;
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

        public void SendVerifyCode(int code, string recipient)
        {
            var smtpClient = new SmtpClient(_configHandlingService.SMTPAddr)
            {
                Port = (int) _configHandlingService.SMTPPort,
                Credentials = new NetworkCredential(_configHandlingService.SMTPEmail, _configHandlingService.SMTPPassword),
                EnableSsl = true
            };

            smtpClient.Send(_configHandlingService.SMTPEmail, recipient, "CCSO Bot Verification Code", "Hi!\nYour verification code is: " + code);
        }
    }
}