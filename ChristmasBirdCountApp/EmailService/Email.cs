// (c) 2016 Geneva College Senior Software Project Team
// With help (code) from: Sven-Michael Stübe @ http://stackoverflow.com/questions/39678310/add-email-body-text-and-send-email-from-xamarin-android-app/39681516#39681516
using System;
using System.IO;
using System.Text;
using MimeKit;

namespace EmailService
{
    public class Email
    {
        public MimeMessage EmailMessage { get; set; }

        public void CreateEmailMessage(string toAddress, string subjectText, string bodyText)
        {
            // Following Code Adapted From Morten Godrim Jensen @ http://stackoverflow.com/questions/30255789/how-to-send-a-mail-in-xamarin-using-system-net-mail-smtpclient
            // Following Code Adapted From https://github.com/jstedfast/MailKit
            // Build the Body of the Email
            var emailBody = new StringBuilder();
            emailBody.AppendLine(bodyText);

            // Set Up Email Parameters
            EmailMessage = new MimeMessage();
            EmailMessage.From.Add(new MailboxAddress("Christmas Bird Count App Log", "gc.seniorsoftwareproject@gmail.com"));
            EmailMessage.To.Add(new MailboxAddress("Compiler", toAddress));     // The "Compiler" is the recipient of the email.
            EmailMessage.Subject = subjectText;
            EmailMessage.Body = new TextPart("plain") { Text = emailBody.ToString() };
        }

        public void AddAttachmentToEmail(string filepath)
        {
            // Code and Comments Borrowed from http://www.mimekit.net/docs/html/CreatingMessages.htm#CreateMessageWithAttachments
            // Create an attachment for the file located at path
            var attachment = new MimePart("file", "csv")
            {
                ContentObject = new ContentObject(File.OpenRead(filepath), ContentEncoding.Default),
                ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                ContentTransferEncoding = ContentEncoding.Base64,
                FileName = Path.GetFileName(filepath)
            };

            // Create the multipart/mixed container to hold the message text and the file attachment
            var multipart = new Multipart("mixed");
            multipart.Add(EmailMessage.Body);
            multipart.Add(attachment);

            // Set the multipart/mixed as the message body
            EmailMessage.Body = multipart;
        }

        public bool SendEmail()
        {
            // Connect to SMTP Email Server
            SmtpConnection smtpConnection = new SmtpConnection();

            int connectionAttempts = 0;

            do
            {
                smtpConnection.CreateSmtpConnection();
                connectionAttempts++;
            } while (smtpConnection.Client == null && connectionAttempts <= 5);  // May make 5 attempts to connect to SMTP Server

            if (smtpConnection.Client == null)
            {
                return false;   // The email failed to send becuase a connection to the SMTP server could not be established.
            }

            // Attempt to Send Email
            try
            {
                // Send Email
                smtpConnection.Client.Send(EmailMessage);

                // Close the SMTP Client/Server Connection
                smtpConnection.CloseSmtpConnection();

                return true;    // The email was sent.
            }
            catch (Exception)
            {
                return false;   // The email failed to send.
            }
        }
    }
}
