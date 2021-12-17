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
    /// Interaktionslogik für WU_TextBox.xaml
    /// </summary>
    public partial class WU_TextBox : UserControl
    {
        public string? Placeholder { get; set; }


        public WU_TextBox()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void OnKeyboardFocused(object sender, DependencyPropertyChangedEventArgs e) => Input.Tag = "";
        private void OnLostFocus(object sender, KeyboardFocusChangedEventArgs e) => Input.Tag = Input.Text == "" ? Placeholder : "";
    }
}
