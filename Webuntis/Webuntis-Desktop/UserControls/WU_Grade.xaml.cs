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
    /// Interaktionslogik für WU_Grade.xaml
    /// </summary>
    public partial class WU_Grade : UserControl
    {
        public string GradeContent { get; set; }
        public Brush ForegroundColor { get; set; }

        public WU_Grade(string Content, Brush ForegroundColor)
        {
            InitializeComponent();
            this.GradeContent = Content;
            this.ForegroundColor = ForegroundColor;
            this.DataContext = this;
        }
    }
}
