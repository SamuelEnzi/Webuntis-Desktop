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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Webuntis_Desktop.UserControls
{
    /// <summary>
    /// Interaktionslogik für WU_PasswordBox.xaml
    /// </summary>
    public partial class WU_PasswordBox : UserControl
    {
        public string? Placeholder { get; set; }

        public string? Password { get => Input.Password; set => Input.Password = value; }

        public WU_PasswordBox()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void OnLostFocus(object sender, KeyboardFocusChangedEventArgs e) => Input.Tag = Input.Password == "" ? Placeholder : "";
        private void OnFocus(object sender, KeyboardFocusChangedEventArgs e) => Input.Tag = "";
     
    }
}
