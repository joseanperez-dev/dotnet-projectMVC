using System.Security.Cryptography;
using System.Text;
using System.Net.Mail;


namespace projectMVC.Helpers;


/*
*   This class contains utility static methods used throughout the application.
*   It provides helper methods for password hashing, token generation, and email sending.
*/
class Utils
{
    /*
    *   Returns a formatted string including the specified word.
    *
    *   @param word The word to include in the string.
    *   @return A string stating "El término es {word}".
    */
    public static string staticMethod(string word)
    {
        return $"El término es {word}";
    }

    /*
    *   Creates a SHA256 hash of the given key string, useful for storing passwords securely.
    *
    *   @param key The input string to hash.
    *   @return The hexadecimal string representation of the hashed key.
    */
    public static string CreatePassword(string key)
    {
        StringBuilder sb = new StringBuilder();
        using (SHA256 hash = SHA256Managed.Create())
        {
            Encoding enc = Encoding.UTF8;
            byte[] result = hash.ComputeHash(enc.GetBytes(key));
            foreach (byte b in result) sb.Append(b.ToString("x2"));
        }
        return sb.ToString();
    }

    /*
    *   Generates a unique token string combining a new GUID and the current Unix timestamp.
    *
    *   @return A string token in the format "GUID_unixTimestamp".
    */
    public static string GenerateToken()
    {
        Guid myuuid = Guid.NewGuid();
        return $"{myuuid.ToString()}_{new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds()}";
    }

    /*
    *   Sends an email with the specified recipient, subject, and HTML content via SMTP.
    *
    *   @param email The recipient's email address.
    *   @param subject The subject line of the email.
    *   @param content The HTML content body of the email.
    */
    public static void SendEmail(string email, string subject, string content)
    {
        MailMessage mail = new MailMessage();
        mail.To.Add(email);
        mail.From = new MailAddress("mailtest@prueba.com");
        mail.Subject = subject;
        string body = content;
        mail.IsBodyHtml = true;
        SmtpClient smtp = new SmtpClient();
        smtp.Host = "sandbox.smtp.mailtrap.io";
        smtp.Port = 587;
        smtp.UseDefaultCredentials = false;
        smtp.Credentials = new System.Net.NetworkCredential("302dc222226142", "211fb72ecfd258");
        smtp.EnableSsl = true;
        smtp.Send(mail);
    }
}
