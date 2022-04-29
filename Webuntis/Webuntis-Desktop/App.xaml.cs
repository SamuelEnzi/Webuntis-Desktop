using System;
using System.Configuration;
using System.IO;
using System.Windows;
using Webuntis_API;
using UpdateManager_Core;
using System.Threading;

namespace Webuntis_Desktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        private Secret? UserSecret = null;
        private WebuntisClient? WebuntisClient = null;
        private Webuntis_API.Models.LoginInfo.Root? LastLoginInfo = null;

        protected override void OnStartup(StartupEventArgs e)
        {
            CheckForUpdateAsync();
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
                        LastLoginInfo = WebuntisClient.LoginInfo;
                        UserSecret = null;
                        WebuntisClient = null;
                        File.Delete(filename!);
                        continue;
                    }
                }

                if (UserSecret == null)
                {
                    Views.Login login = new Views.Login(LastLoginInfo);
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

                    ui.OnRelogin += () =>
                    {
                        ui.Close();
                        ui = null;
                        UserSecret = null;
                        WebuntisClient = null;
                    };

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
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        public void CheckForUpdateAsync()
        {
            new Thread(() =>
            {
                try
                {
                    Directory.Delete("temp", true);
                }
                catch { }
                var executablepath = $"{AppDomain.CurrentDomain.BaseDirectory}Webuntis-Desktop.exe";
                var startCommand = $"start \"\" \"{executablepath}\"";
                UpdateManager updateManager = new UpdateManager("ManamanaTheReal499", "Webuntis-Desktop");
                var response = updateManager.GetFirtPackageAsync().Result;
                if (response == null) return;

                var isSame = updateManager.CurrentVersionMeta?.CheckVersion(response!);

                if (isSame == true) return;

                var res = MessageBox.Show($"Es ist ein Update verfügbar. Möchten Sie es herunterladen?\nDrücken Sie 'Nein' um diese Verion zu überspringen.\n\nPatch notes:\n{response.body}", "Update", MessageBoxButton.YesNo);

                if (res == MessageBoxResult.No) 
                {
                    updateManager.CurrentVersionMeta!.Patch(response!);
                    return;
                };

                if (res != MessageBoxResult.Yes)
                    return;

                updateManager.DownloadFile(response!, (path) =>
                {
                    updateManager.CurrentVersionMeta!.Patch(response!);
                    updateManager.Install(startCommand);
                });
            }).Start();
        }
    }
}
