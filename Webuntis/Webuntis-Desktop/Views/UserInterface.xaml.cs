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

namespace Webuntis_Desktop.Views
{
    /// <summary>
    /// Interaktionslogik für UserInterface.xaml
    /// </summary>
    public partial class UserInterface : Window
    {
        WebuntisClient client;

        public UserInterface(WebuntisClient client)
        {
            InitializeComponent();

            this.client = client;

            var userData = client.GetUserInfo();

            UI_UserName.Content = userData.user.person.displayName;
            UI_UserStatus.Content = userData.user.roles.Aggregate((x, y) => x + ", " + y);

            UI_ProfilePicture.Source = Helpers.WebWorker.ImageFromUrl(userData.user.person.imageUrl);


            UI_ModuleListView.Items.Add(new Module("Übersicht", "Ressources/Uebersicht.png", new Overview()));
            UI_ModuleListView.Items.Add(new Module("Mein Stundenplan" , "Ressources/Stundenplan.png"));
            UI_ModuleListView.Items.Add(new Module("Abwesenheiten", "Ressources/Abwesenheiten.png"));
            UI_ModuleListView.Items.Add(new Module("Noten", "Ressources/Noten.png", new Votes()));
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

            var selectedModule = (Module)UI_ModuleListView.SelectedItem;

            if (selectedModule.module == null)
            {
                UI_ModuleFrame.Content = null;
                return;
            }

            UI_ModuleFrame.Content = (Page)selectedModule.module.Display(client);
        }
    }


}
