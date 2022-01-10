using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Webuntis_API;

namespace Webuntis_Desktop.Views
{
    /// <summary>
    /// Interaktionslogik für Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Secret? userSecret = null;
        public WebuntisClient? client = null;

        private Webuntis_API.Models.LoginInfo.Root? LastLoginInfo = null;

        private string invalidCreds = "Invalid user name and/or password";
        private string userBlocked = "Account blocked";

        public Login(Webuntis_API.Models.LoginInfo.Root? LastLoginInfo)
        {
            InitializeComponent();
            this.LastLoginInfo = LastLoginInfo;

            UI_StatusLable.Content = LastLoginInfo?.loginError != null ? LastLoginInfo.loginError : "";        
            UI_StatusLable.Visibility = Visibility.Visible;
        }

        private void OnCloseClicked(object sender, MouseButtonEventArgs e) => Environment.Exit(0);

        private void OnLoginClicked(object sender, RoutedEventArgs e)
        {
            userSecret = new Secret(this.UI_UsernameInput.Input.Text, this.UI_PasswordInput.Input.Password);

            client = new WebuntisClient(userSecret);

            if (!client.TryOpen())
            {
                UI_StatusLable.Content = invalidCreds;
                if (client.UserBlocked)
                    UI_StatusLable.Content = userBlocked;
                    
                UI_StatusLable.Visibility = Visibility.Visible;
                client.Dispose();
                client = null;
                return;
            }



            this.Close();
        }
    }
}
