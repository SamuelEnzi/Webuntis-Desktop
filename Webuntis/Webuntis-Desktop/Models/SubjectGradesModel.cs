using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webuntis_Desktop.Models
{
    internal class SubjectGradesModel
    {
        public String Fach { get; set; }
        public List<double> Noten { get; set; } = new List<double>();
        public double Durchschnitt { get => Math.Round(Noten.Sum() / Noten.Count, 2); }
        public double Gerundet { get => Math.Round(Durchschnitt); }
        public double reachTarget { get => Math.Round(Zielnote * Noten.Count - Noten.Sum() + Zielnote, 2); }
        public double Zielnote { get; set; }


        public SubjectGradesModel() { }
        public SubjectGradesModel(string subject, double target)
        {
            this.Fach = subject;
            this.Zielnote = target;
        }

        public void AddMark(double mark)
        {
            Noten.Add(mark);
        }
    }
}
