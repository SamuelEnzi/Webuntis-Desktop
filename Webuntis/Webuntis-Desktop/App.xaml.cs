using System;
using System.Configuration;
using System.IO;
using System.Windows;
using Webuntis_API;

namespace Webuntis_Desktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        private Secret? UserSecret = null;
        private WebuntisClient? WebuntisClient = null;

        protected override void OnStartup(StartupEventArgs e)
        {
            while (true)
            {
                var filename = ConfigurationManager.AppSettings["SecretFileName"]?.ToString() != null ? ConfigurationManager.AppSettings["SecretFileName"]?.ToString() : "usData.sha";

                if (File.Exists(filename!))
                    UserSecret = Secret.LoadSecret(File.ReadAllText(filename!));

                if (UserSecret != null)
                {
                    WebuntisClient = new WebuntisClient(UserSecret);
                    if (!WebuntisClient!.TryOpen())
                    {
                        WebuntisClient.Dispose();
                        WebuntisClient = null;
                    }
                }

                if (UserSecret == null)
                {
                    Views.Login login = new Views.Login();
                    login.ShowDialog();

                    UserSecret = login.userSecret;
                    WebuntisClient = login.client;

                    File.WriteAllText(filename!, UserSecret!.GetProtectedSecret());
                }

                if (UserSecret == null)
                {
                    MessageBox.Show("no secret");
                    Environment.Exit(0);
                }

                try
                {
                    Views.UserInterface? ui = new Views.UserInterface(WebuntisClient!);

                    ui.OnLogout += () =>
                    {
                        ui.Close();
                        ui = null;
                        UserSecret = null;
                        WebuntisClient = null;
                        File.Delete(filename!);
                    };
                   
                    ui.ShowDialog();
                }
                catch(Exception ex)
                {
                    UserSecret = null;
                    WebuntisClient = null;
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
