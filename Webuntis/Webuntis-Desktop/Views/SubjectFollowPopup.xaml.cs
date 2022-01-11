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
                this.UI_Background.Opacity = 0.7;
                this.UI_Subject.Content = Subject;
                this.UI_Teacher.Content = Teacher;
                this.UI_StartTime.Content = ToTime(StartTime);   
                this.UI_EndTime.Content = ToTime(EndTime);
                this.UI_Text.Text = Text;
            });
        }

        private string ToTime(string time) => time.Insert(time.Length - 2, ":");

    }
}
