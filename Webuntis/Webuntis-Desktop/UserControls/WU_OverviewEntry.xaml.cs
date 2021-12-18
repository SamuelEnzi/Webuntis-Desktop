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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Webuntis_Desktop.UserControls
{
    /// <summary>
    /// Interaktionslogik für WU_OverviewEntry.xaml
    /// </summary>
  
    public partial class WU_OverviewEntry : UserControl
    {
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public ImageSource IconImageSource { get; set; }
        public DataGrid DataContent { get => this.PDataContent; set => this.PDataContent = value; }
        public int MaxCellWidth { get; set; } = 250;
        public WU_OverviewEntry()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void AutoGen(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            e.Column.MaxWidth = MaxCellWidth;
            DataGridBoundColumn column = e.Column as DataGridBoundColumn;
            if (column != null)
            {
                column.Binding = new Binding(e.PropertyName);

                Style elementStyle = new Style(typeof(TextBlock));
                elementStyle.Setters.Add(new Setter(TextBlock.TextWrappingProperty, TextWrapping.WrapWithOverflow));
                elementStyle.Setters.Add(new Setter(TextBlock.MarginProperty, new Thickness(0,0,0, 10)));
                column.ElementStyle = elementStyle;
            }
        }
    }
}
