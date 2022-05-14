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
        public string Date { get; set; }
        public string Description { get; set; }
        public WU_Grade(Models.GradeModel Content, Brush ForegroundColor)
        {
            InitializeComponent();
            this.GradeContent = Content.Grade.ToString();
            this.ForegroundColor = ForegroundColor;
            this.Date = Content.Date;
            this.Description = Content.Description;

            UI_HoverPanel.ToolTip = $"[{Date}] {Description}";
            this.DataContext = this;
        }
    }
}
