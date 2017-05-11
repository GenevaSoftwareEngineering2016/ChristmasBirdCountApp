#load "BirdCount.cs"
#load "FieldFormAnswers.cs"
using System;
using System.Net;
using System.IO;
using System.Text;
using MimeKit;
using MailKit.Net.Smtp;

public static void SendNewFieldReportEmail(FieldFormAnswers fieldFormAnswers, TraceWriter log)
{
    // Set Variables from Form Answers
    string recipientEmail = fieldFormAnswers.RecipientEmail;
    string countType = fieldFormAnswers.CountType;

    // Create and Send the Email Message
    Email emailToSend = new Email();

    // Build Body of Email Message
    StringBuilder emailBodyText = new StringBuilder();
    emailBodyText.AppendLine("A new " + countType + " bird count has been submitted.");
    emailBodyText.AppendLine("\n");

    emailBodyText.AppendLine("Christmas Bird Count Results");
    emailBodyText.AppendLine(fieldFormAnswers.ReportSubmissionTime);
    emailBodyText.AppendLine("Count Circle: " + fieldFormAnswers.CountCircleCode);
    emailBodyText.AppendLine("Count Type: " + fieldFormAnswers.CountType);
    emailBodyText.AppendLine("--------------------------------------\n");
    emailBodyText.AppendLine("Party Members: " + fieldFormAnswers.PartyMembers + "\n");
    emailBodyText.AppendLine("Number in Party: " + fieldFormAnswers.PartySize + "\n");

    emailBodyText.AppendLine("\n");
    emailBodyText.AppendLine("Team Leader: " + fieldFormAnswers.TeamLeaderName);
    emailBodyText.AppendLine("Phone: " + fieldFormAnswers.TeamLeaderPhone);
    emailBodyText.AppendLine("Email: " + fieldFormAnswers.TeamLeaderEmail);
    emailBodyText.AppendLine(fieldFormAnswers.Street);
    emailBodyText.AppendLine(fieldFormAnswers.City + ", " + fieldFormAnswers.State + " " + fieldFormAnswers.Zip + "\n");
    emailBodyText.AppendLine("\n");

    emailBodyText.AppendLine("Start Time 1: " + fieldFormAnswers.StartTime1);
    emailBodyText.AppendLine("End Time 1: " + fieldFormAnswers.EndTime1 + "\n");
    emailBodyText.AppendLine("Start Time 2: " + fieldFormAnswers.StartTime2);
    emailBodyText.AppendLine("End Time 2: " + fieldFormAnswers.EndTime2 + "\n");
    emailBodyText.AppendLine("Hours Driven: " + fieldFormAnswers.HoursDriven);
    emailBodyText.AppendLine("Miles Driven: " + fieldFormAnswers.MilesDriven + "\n");
    emailBodyText.AppendLine("Hours Walked: " + fieldFormAnswers.HoursWalked);
    emailBodyText.AppendLine("Miles Walked: " + fieldFormAnswers.MilesWalked + "\n");
    emailBodyText.AppendLine("Hours Owling: " + fieldFormAnswers.HoursOwling + "\n\n");

    emailBodyText.AppendLine("Total Number of Bird Species Seen: " + fieldFormAnswers.TotalBirdSpeciesSeen);
    emailBodyText.AppendLine("Total Number of All Birds Seen: " + fieldFormAnswers.TotalBirdsSeen + "\n");
    emailBodyText.AppendLine("Notes: " + fieldFormAnswers.OptionalNotes);


    emailToSend.CreateEmailMessage(recipientEmail, "New Christmas Bird Count Results", emailBodyText);
    
    // Send the Email - We Are Adding an Attachment
    bool emailSent = emailToSend.SendEmail(log, false, "");

    if (emailSent)
    {
        log.Info("Email Sent!");
    }
    else
    {
        log.Error("Unable to send email!");
    }
}

// CREATE AN EMAIL OBJECT TO SEND
public class Email
{
    public MimeMessage EmailMessage { get; set; }

    public void CreateEmailMessage(string toAddress, string subjectText, StringBuilder emailBody)
    {
        // Following Code Adapted From Morten Godrim Jensen @ http://stackoverflow.com/questions/30255789/how-to-send-a-mail-in-xamarin-using-system-net-mail-smtpclient
        // Following Code Adapted From https://github.com/jstedfast/MailKit

        // Set Up Email Parameters
        EmailMessage = new MimeMessage();
        EmailMessage.From.Add(new MailboxAddress("Christmas Bird Count App Log", "gc.seniorsoftwareproject@gmail.com"));
        EmailMessage.To.Add(new MailboxAddress("Compiler", toAddress));     // The "Compiler" is the recipient of the email.
        EmailMessage.Subject = subjectText;
        EmailMessage.Body = new TextPart("plain") { Text = emailBody.ToString() };
    }

    public bool SendEmail(TraceWriter log, bool addAttachment = false, string attachmentFilePath = "")
    {
        // Connect to SMTP Email Server
        SmtpConnection smtpConnection = new SmtpConnection();

        int connectionAttempts = 0;

        do
        {
            smtpConnection.CreateSmtpConnection(log);
            connectionAttempts++;
        } while (smtpConnection.Client == null && connectionAttempts <= 5);  // May make 5 attempts to connect to SMTP Server

        if (smtpConnection.Client == null)
        {
            log.Error("SMTP Client Error");
            return false;   // The email failed to send becuase a connection to the SMTP server could not be established.
        }

        if (addAttachment)
        {
            // Code and Comments Borrowed from http://www.mimekit.net/docs/html/CreatingMessages.htm#CreateMessageWithAttachments
            try
            {
                // Create an attachment for the file located at path,
                // Add that attachment to the email,
                // And send the email
                using (FileStream fileOpener = File.OpenRead(attachmentFilePath))
                {
                    var attachment = new MimePart("file", "csv")
                    {
                        ContentObject = new ContentObject(fileOpener, ContentEncoding.Default),
                        ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                        ContentTransferEncoding = ContentEncoding.Base64,
                        FileName = Path.GetFileName(attachmentFilePath)
                    };

                    // Create the multipart/mixed container to hold the message text and the file attachment
                    var multipart = new Multipart("mixed");
                    multipart.Add(EmailMessage.Body);
                    multipart.Add(attachment);

                    // Set the multipart/mixed as the message body
                    EmailMessage.Body = multipart;

                    // Send Email
                    smtpConnection.Client.Send(EmailMessage);
                }

                // Close the SMTP Client/Server Connection
                smtpConnection.CloseSmtpConnection();

                return true;    // The email was sent.
            }
            catch (Exception ex)
            {
                log.Error("Error sending email: " + ex.ToString());
                return false;   // The email failed to send.
            }
        }
        else
        {
            // Attempt to Send Email
            try
            {
                // Send Email
                smtpConnection.Client.Send(EmailMessage);

                // Close the SMTP Client/Server Connection
                smtpConnection.CloseSmtpConnection();

                return true;    // The email was sent.
            }
            catch (Exception ex)
            {
                log.Error("Error sending email: " + ex.ToString());
                return false;   // The email failed to send.
            }
        }
    }
}

// CREATE A CONNECTION TO THE GMAIL ACCOUNT VIA WHICH EMAILS WILL BE SENT
public class SmtpConnection
{
    private string _emailAddress;
    private string _emailPassword;

    public SmtpClient Client { get; set; }

    public void CreateSmtpConnection(TraceWriter log)
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
            // 1) Get the Email Address
            _emailAddress = "gc.seniorsoftwareproject@gmail.com";
            _emailPassword = "M0b1l3F1rst(-)";
            // 2) Authenticate
            Client.Authenticate(_emailAddress, _emailPassword);
        }
        catch (Exception ex)
        {
            log.Error("SMTP Connection Error: " + ex.ToString());
            Client = null;
        }
    }

    public void CloseSmtpConnection()
    {
        Client.Disconnect(true);
    }
}