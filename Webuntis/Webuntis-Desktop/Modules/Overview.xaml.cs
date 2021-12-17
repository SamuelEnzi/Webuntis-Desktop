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
using Webuntis_API;

namespace Webuntis_Desktop.Modules
{
    /// <summary>
    /// Interaktionslogik für Overview.xaml
    /// </summary>
    public partial class Overview : Page, IModule
    {
        WebuntisClient client;
        public Overview() => InitializeComponent();
        public object Display(WebuntisClient client)
        {
            this.client = client;
            this.Lable.Content = $"Overview:: {client.Secret.Username}";
            return this;
        }
    }
}
