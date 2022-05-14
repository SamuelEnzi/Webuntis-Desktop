using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Webuntis_Desktop.Models
{
    public class SubjectGradesEntry
    {
        public List<double> Grades { get; set; } = new();
        public double Avrage { get; set; }
        public string GradesToTarget { get; set; }
        public string SubjectName { get; set; }
        public Brush AvrageForeground { get; set; } 
        public SubjectGradesEntry(string subjectName, List<double> grades, string gradesToTarget)
        {
            this.Avrage = Math.Round(grades.Average(),2);
            this.SubjectName = subjectName;
            this.GradesToTarget = gradesToTarget;
            this.Grades = grades;
            this.AvrageForeground = GradeToColor(Avrage);
        }

        private Brush GradeToColor(double Grade)
        {
            if (Grade > 6) return Brushes.Green;
            else if (Grade == 6) return Brushes.Orange;
            else return Brushes.Red;
        }
    }
}
