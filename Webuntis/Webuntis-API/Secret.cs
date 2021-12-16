using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Security;

namespace Webuntis_API
{
    /// <summary>
    /// User key pair
    /// </summary>
    public class Secret
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string SchoolName { get; set; } = "lbs-brixen";
        public Secret(string Username, string Password)
        {
            this.Username = Username;
            this.Password = Password;
        }

        /// <summary>
        /// Encrypt and return class
        /// </summary>
        /// <returns></returns>
        public string GetProtectedSecret() => Protect(this.Serialize());

        public static Secret LoadSecret(string encryptedData)
        {
            try
            {
                return Unprotect(encryptedData).Deserialize<Secret>();
            }
            catch(Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"protect: {e.Message}");
                Console.ResetColor();
                return null;
            }
        }

        internal protected static string Protect(string input)
        {
            try
            {
                byte[] encryptedData = ProtectedData.Protect(Encoding.Unicode.GetBytes(input), null, DataProtectionScope.CurrentUser);
                return Convert.ToBase64String(encryptedData);
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"protect: {e.Message}");
                Console.ResetColor();
                return null;
            }
        }

        internal protected static string Unprotect(string encryptedData)
        {
            try
            {
                byte[] decryptedData = ProtectedData.Unprotect(Convert.FromBase64String(encryptedData), null, DataProtectionScope.CurrentUser);
                return  Encoding.Unicode.GetString(decryptedData);
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"unprotect: {e}");
                Console.ResetColor();
                return null;
            }
        }
    }
}
