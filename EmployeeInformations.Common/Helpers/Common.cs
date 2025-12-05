using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;

namespace EmployeeInformations.Common
{
    public class Common
    {
        public static string GeneratePassword()
        {
            string chars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var length = 10;
            StringBuilder sb = new StringBuilder();
            Random rnd = new Random();

            for (int i = 0; i < length; i++)
            {
                int index = rnd.Next(chars.Length);
                sb.Append(chars[index]);
            }

            return Convert.ToString(sb);
        }

        public static void WriteServerErrorLog(string Message)
        {
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\SeverLog.txt", true);
                sw.WriteLine(DateTime.Now.ToString() + ": " + Message);
                sw.Flush();
                sw.Close();
            }
            catch
            {
            }
        }

        public static string Encrypt(string value)
        {
            string EncryptionKey = "abc123";
            byte[] clearBytes = Encoding.Unicode.GetBytes(value);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    value = Convert.ToBase64String(ms.ToArray());
                }
            }
            return value;
        }

        public static string Decrypt(string value)
        {
            string EncryptionKey = "abc123";
            value = value.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(value);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    value = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return value;
        }

        public static string sha256_hash(string value)
        {
            StringBuilder sb = new StringBuilder();
            using SHA256 hash = SHA256.Create();
            {
                Encoding encoder = Encoding.UTF8;
                byte[] result = hash.ComputeHash(encoder.GetBytes(value));
                foreach (byte b in result)
                    sb.Append(b.ToString("X2"));
            }
            return Convert.ToString(sb);
        }

        public static string SendQuoteAndConsultantEmail(IConfiguration _config, string body, string toEmail, string subject, string displayName)
        {
            try
            {
                var fromEmail = Convert.ToString(_config.GetSection("WebSiteEmailSettings").GetSection("FromEmail").Value);
                var password = Convert.ToString(_config.GetSection("WebSiteEmailSettings").GetSection("Password").Value);
                var host = Convert.ToString(_config.GetSection("WebSiteEmailSettings").GetSection("Host").Value);
                var port = Convert.ToInt32(_config.GetSection("WebSiteEmailSettings").GetSection("Port").Value);

                var fromAddress = new MailAddress(fromEmail, displayName);
                var toAddress = new MailAddress(toEmail);

                var smtp = new SmtpClient
                {
                    Host = host,
                    Port = port,
                    //EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, password)
                };
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                })
                {
                    smtp.Send(message);
                }
            }
            catch (Exception)
            {

            }
            return string.Empty;
        }

        public static string GetEmployeeSortName(string firstName, string lastName)
        {
            var firstLetterFirstName = string.IsNullOrEmpty(firstName) ? "" : firstName.Substring(0, 1);
            var firstLetterLastName = string.IsNullOrEmpty(lastName) ? "" : lastName.Substring(0, 1);
            return (firstLetterFirstName + firstLetterLastName);
        }

        public static string GetClassNameForGrid(int input)
        {
            string className = "";
            switch (input)
            {
                case 1:
                    className = "bg-success";
                    break;
                case 2:
                    className = "bg-warning";
                    break;
                case 3:
                    className = "bg-info";
                    break;
                case 4:
                    className = "bg-blue";
                    break;
                case 5:
                    className = "bg-pink";
                    break;
                case 6:
                    className = "bg-danger";
                    break;
            }
            return className;
        }

        public static string GetClassNameForProjectDashboard(int input)
        {
            string className = "";
            switch (input)
            {
                case 1:
                    className = "#816bff";
                    break;
                case 2:
                    className = "#13c9f2";
                    break;
                case 3:
                    className = "#ff82b7";
                    break;
                case 4:
                    className = "#559bfb";
                    break;
                case 5:
                    className = "#abd67f";
                    break;
                    //case 6:
                    //    className = "bg-danger";
                    //    break;
            }
            return className;
        }

        public static string GetClassNameForDepartmentDashboard(int input)
        {
            string className = "";
            switch (input)
            {
                case 1:
                    className = "purple";
                    break;
                case 2:
                    className = "orange";
                    break;
                case 3:
                    className = "info";
                    break;
                case 4:
                    className = "blue";
                    break;
                    //case 5:
                    //    className = "pink";
                    //    break;
                    //case 6:
                    //    className = "bg-danger";
                    //    break;
            }
            return className;
        }

        public static string GetClassNameForLeaveDashboard(int input)
        {
            string className = "";
            switch (input)
            {
                case 1:
                    className = "bg-info";
                    break;
                case 2:
                    className = "bg-pink";
                    break;
                case 3:
                    className = "bg-success";
                    break;
                case 4:
                    className = "bg-blue";
                    break;
                case 5:
                    className = "bg-purple";
                    break;
                    //case 6:
                    //    className = "bg-danger";
                    //    break;
            }
            return className;
        }

    }
}
