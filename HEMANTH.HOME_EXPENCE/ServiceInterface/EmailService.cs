using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System.Net.Sockets;

namespace HEMANTH.HOME_EXPENSE.ServiceInterface
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private async static Task<bool> IsEmailAddressExist(string email)
        {
            // Get domain from email address
            var domain = email.Split('@')[1];
            try
            {
                var smtpClient = new TcpClient(domain, 587); // Connect to SMTP server on port 25
                var stream = smtpClient.GetStream();
                var reader = new System.IO.StreamReader(stream);
                var writer = new System.IO.StreamWriter(stream);

                // Initiate SMTP conversation
                writer.WriteLine("HELO smtp.test.com");
                writer.Flush();
                var response = reader.ReadLine();

                // Send the VRFY command to check if email exists
                writer.WriteLine($"VRFY {email}");
                writer.Flush();
                response = reader.ReadLine();

                if (response.StartsWith("250"))
                {
                    // Email address exists
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SMTP error: {ex.Message}");
            }
            return false;
        }

        public async Task<bool> SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {

               // bool isExist = await IsEmailAddressExist(toEmail);
                //if(!isExist)
                //{
                //    return false;
                //}
                var emailSettings = _configuration.GetSection("EmailSettings");
                var smtpServer = emailSettings["SmtpServer"];
                var port = int.Parse(emailSettings["Port"]);
                var senderEmail = emailSettings["SenderEmail"];
                var senderPassword = emailSettings["SenderPassword"];

                var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress("ನಮ್ಮ ಮನೆ ಕರ್ಚು", senderEmail));
                emailMessage.To.Add(new MailboxAddress("", toEmail));
                emailMessage.Subject = subject;
                emailMessage.Body = new TextPart("html") { Text = body };

                using var client = new MailKit.Net.Smtp.SmtpClient();

                // Connect and authenticate
                await client.ConnectAsync(smtpServer, port, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(senderEmail, senderPassword);

                // Check if connected
                if (!client.IsConnected)
                {
                    Console.WriteLine("Failed to connect to the SMTP server.");
                    return false;
                }

                // Send email
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);

                // If we reach here, email was sent
                Console.WriteLine($"Email sent successfully to {toEmail}");
                return true;
            }
            catch (SmtpCommandException smtpEx)
            {
                // Handle SMTP-specific errors
                Console.WriteLine($"SMTP Error: {smtpEx.Message} - Code: {smtpEx.StatusCode}");
                return false;
            }
            catch (Exception ex)
            {
                // Handle other general exceptions
                Console.WriteLine($"Error sending email: {ex.Message}");
                return false;
            }
        }
    }
}
