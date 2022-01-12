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
using Webuntis_Desktop.Models;
using Webuntis_Desktop.Modules;
using Webuntis_API;
using System.Threading;

namespace Webuntis_Desktop.Views
{
    /// <summary>
    /// Interaktionslogik für UserInterface.xaml
    /// </summary>
    public partial class UserInterface : Window
    {
        public delegate void OnLogoutEventHandler();
        public event OnLogoutEventHandler? OnLogout;

        public delegate void OnReLoginEventHandler();
        public event OnReLoginEventHandler? OnRelogin;

        WebuntisClient? client;

        public UserInterface(WebuntisClient client)
        {
            InitializeComponent();

            this.client = client;

            var userData = client.GetUserInfo();

            UI_UserName.Content = userData.ToName();
            UI_UserStatus.Content = userData.user.roles.Aggregate((x, y) => x + ", " + y);

            UI_ProfilePicture.Source = Helpers.WebWorker.ImageFromUrl(userData.user.person.imageUrl);

            UI_ModuleListView.Items.Add(new Module("Übersicht", "Ressources/Uebersicht.png", new Overview()));
            UI_ModuleListView.Items.Add(new Module("Mein Stundenplan", "Ressources/Stundenplan.png", new TimeTable()));
            UI_ModuleListView.Items.Add(new Module("Abwesenheiten", "Ressources/Abwesenheiten.png", new Absences()));
            UI_ModuleListView.Items.Add(new Module("Noten", "Ressources/Noten.png", new Votes()));

            UI_ModuleListView.SelectedIndex = 0;
        }

        private void OnCloseClicked(object sender, MouseButtonEventArgs e) => Environment.Exit(0);

        private void OnWindowDrag(object sender, MouseEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch { }
        }

        private void OnModuleSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (UI_ModuleListView.SelectedItem == null) return;
            bool success = client!.HeartBeat();

            if (!success)
            {
                OnRelogin?.Invoke();
                return;
                
            }


            UI_ProcessBar.Visibility = Visibility.Visible;
            UI_ModuleListView.IsEnabled = false;

            var selectedModule = (Module)UI_ModuleListView.SelectedItem;

            if (selectedModule.module == null)
            {
                UI_ModuleFrame.Content = null;
                return;
            }

            try
            {
                selectedModule.module.OnFinishedLoading += OnFinishedLoading;
                UI_ModuleFrame.Content = (Page)selectedModule.module.Display(client);
                new Thread(() => 
                {
                    try
                    {
                        selectedModule.module.Render();
                    }
                    catch { }
                }).Start();
            }
            catch { }

        }

        private void OnFinishedLoading(object sender)
        {
            Dispatcher.Invoke(() =>
            {
                var selectedModule = (Module)UI_ModuleListView.SelectedItem;

                selectedModule.module!.OnFinishedLoading -= OnFinishedLoading;
                UI_ProcessBar.Visibility = Visibility.Hidden;
                UI_ModuleListView.IsEnabled = true;
            });
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.WindowState = WindowState.Normal;
            }

        }

        private void LogoutButtonClicked(object sender, MouseButtonEventArgs e) => OnLogout?.Invoke();
 
    }


}
