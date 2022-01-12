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
    /// Interaktionslogik für SubjectFollowPopup.xaml
    /// </summary>
    public partial class SubjectFollowPopup : Window
    {
        public SubjectFollowPopup()
        {
            InitializeComponent();
        }

        public void Set(string Teacher, string Subject, string StartTime, string EndTime ,string Text, Brush Color) 
        {
            Dispatcher.Invoke(() =>
            {
                this.UI_Background.Fill = Color;
               
                this.UI_Subject.Content = Subject;
                this.UI_Teacher.Content = Teacher;
                this.UI_StartTime.Content = $"von {ToTime(StartTime)}Uhr";   
                this.UI_EndTime.Content = $"bis {ToTime(EndTime)}Uhr";
                this.UI_Text.Text = Text;

                if(Text != String.Empty)
                    this.UI_Text.Visibility = Visibility.Visible;
                else
                    this.UI_Text.Visibility = Visibility.Collapsed;

            });
        }

        private string ToTime(string time) => time.Insert(time.Length - 2, ":");

    }
}
