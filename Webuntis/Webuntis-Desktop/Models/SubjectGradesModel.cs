using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webuntis_Desktop.Models
{
    internal class SubjectGradesModel
    {
        public string? Subject { get; set; }
        public List<GradeModel> Grades { get; set; } = new List<GradeModel>();
        public SubjectGradesModel() { }
        public SubjectGradesModel(string subject) =>
             this.Subject = subject;

        //public void AddMark(double mark) =>Noten.Add(mark);
        public void AddMark(GradeModel model) =>
            Grades.Add(model);

    }

    public class GradeModel
    {
        public double Grade { get; set; }
        public string Date { get; set; }
        public string Description { get; set; }

        public GradeModel(double Grade, string Date, string Description)
        {
            this.Grade = Grade;
            this.Date = Date;
            this.Description = Description;
        }


    }
}
