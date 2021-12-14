using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
