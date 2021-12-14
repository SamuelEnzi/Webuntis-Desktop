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

namespace Webuntis_Desktop.Views
{
    /// <summary>
    /// Interaktionslogik für UserInterface.xaml
    /// </summary>
    public partial class UserInterface : Window
    {
        public UserInterface()
        {
            InitializeComponent();
        }

        public void getData()
        {
            txtblck_output.Text = "hot funktioniert";
        }



        private void btn_getData_Click(object sender, RoutedEventArgs e)
        {
            if (txtbx_password.Text != "" && txtbx_username.Text != "")
                getData();
        }

        private void txtbx_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                if (txtbx_password.Text != "" && txtbx_username.Text != "")
                    getData();
        }
    }
}
