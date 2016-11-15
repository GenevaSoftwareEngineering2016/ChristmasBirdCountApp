// (c) 2016 Geneva College Senior Software Project Team
using System;
using System.Reflection;
using System.Resources;
using MailKit.Net.Smtp;

namespace EmailService
{
    public class SmtpConnection
    {
        private string _encryptedPassword;
        private string _sharedSecret;
        private string _emailPassword;

        public SmtpClient Client { get; set; }

        public void CreateSmtpConnection()
        {
            // Set Up SMTP Client
            Client = new SmtpClient();

            // Decrypt Email Password

            // 1) Get Encrypted Password
            Assembly assembly = this.GetType().Assembly;
            ResourceManager resourceManager = new ResourceManager("Resources.EmailStrings", assembly);
            _encryptedPassword = resourceManager.GetString("EmailPassword");
            _sharedSecret = resourceManager.GetString("SharedSecret");

            // 2) Decrypt Password
            _emailPassword = Decryptor.DecryptStringAES(_encryptedPassword, _sharedSecret);

            try
            {
                Client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                // Connect to SMTP Server
                Client.Connect("smtp.gmail.com", 465, true);

                // Not Using OAuth2 Token
                Client.AuthenticationMechanisms.Remove("XOAUTH2");

                // SMTP Server Requires Authentication
                Client.Authenticate("gc.seniorsoftwareproject@gmail.com", _emailPassword);
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
