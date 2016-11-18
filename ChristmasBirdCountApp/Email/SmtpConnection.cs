// (c) 2016 Geneva College Senior Software Project Team
using System;
using Android.Content;
using MailKit.Net.Smtp;

namespace ChristmasBirdCountApp.Email
{
    public class SmtpConnection
    {
        private string _emailAddress;
        private string _emailPassword;
        private string _sharedSecret;

        public SmtpClient Client { get; set; }

        public void CreateSmtpConnection(Context appContext)
        {
            // Set Up SMTP Client
            Client = new SmtpClient();

            // Decrypt Email Password
            try
            {
                // Get Email Resources
                _emailPassword = appContext.GetString(Resource.String.EmailPassword);
                _sharedSecret = appContext.GetString(Resource.String.SharedSecret);

                // Decrypt Password
                _emailPassword = Decryptor.DecryptStringAES(_emailPassword, _sharedSecret);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("ERROR! " + ex.ToString());
            }

            try
            {
                Client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                // Connect to SMTP Server
                Client.Connect("smtp.gmail.com", 465, true);

                // Not Using OAuth2 Token
                Client.AuthenticationMechanisms.Remove("XOAUTH2");

                // SMTP Server Requires Authentication
                // 1) Get the Email Address
                _emailAddress = appContext.GetString(Resource.String.EmailAddress);
                // 2) Authenticate
                Client.Authenticate(_emailAddress, _emailPassword);
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
