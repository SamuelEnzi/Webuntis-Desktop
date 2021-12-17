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
    /// Interaktionslogik für Votes.xaml
    /// </summary>
    public partial class Votes : Page, IModule
    {
        WebuntisClient client;
        public Votes() => InitializeComponent();
        public object Display(WebuntisClient client)
        {
            this.client = client;
            this.Lable.Content = $"Votes:: {client.Secret.Username}";
            return this;
        }
    }
}
