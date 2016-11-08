// (c) 2016 Geneva College Senior Software Project Team
using System;
using MailKit.Net.Smtp;

namespace EmailService
{
    public class SmtpConnection
    {
        public SmtpClient Client { get; set; }

        public void CreateSmtpConnection()
        {
            // Set Up SMTP Client
            Client = new SmtpClient();

            try
            {
                Client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                // Connect to SMTP Server
                Client.Connect("smtp.gmail.com", 465, true);

                // Not Using OAuth2 Token
                Client.AuthenticationMechanisms.Remove("XOAUTH2");

                // SMTP Server Requires Authentication
                Client.Authenticate("gc.seniorsoftwareproject@gmail.com", "----------");
            }
            catch (Exception)
            {
                Client = null;
            }
        }

        public void CloseSmtpConnection()
        {
            Client.Disconnect(true);
        }
    }
}
