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
    /// Interaktionslogik für WU_MarkEntry.xaml
    /// </summary>
    public partial class WU_MarkEntry : UserControl
    {
        public string SubjectName { get; set; }
        public string Avrege { get; set; }
        public string GradesToTarget { get; set; }
        public List<double> Grades { get; set; } = new();
        public Brush AvrageForeground { get; set; }

        public WU_MarkEntry(string SubjectName, string Avrege, string GradesToTarget, List<double> Grades, Brush AvrageForeground)
        {
            InitializeComponent();
            this.SubjectName =  SubjectName;
            this.Avrege = Avrege;
            this.GradesToTarget = GradesToTarget;
            this.Grades = Grades;
            this.AvrageForeground = AvrageForeground;

            this.Grades.ForEach((grade) =>
            {
                var element = new WU_Grade(grade.ToString(), GradeColor(grade));
                UI_GradesContainer.Children.Add(element);
            });

            this.DataContext = this;
        }

        private Brush GradeColor(double grade)
        {
            if (grade > 6) return Brushes.Green;
            else if (grade == 6) return Brushes.Orange;
            else return Brushes.Red;
        }
    }
}
