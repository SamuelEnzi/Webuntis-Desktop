using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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
using Webuntis_Desktop.Models;

namespace Webuntis_Desktop.Modules
{
    /// <summary>
    /// Interaktionslogik für Votes.xaml
    /// </summary>
    public partial class Votes : Page, IModule
    {
        private int targetMark = 6;

        public event IModule.OnFinishedLoadingEventHandler? OnFinishedLoading;
        WebuntisClient? client;
        public Votes() => InitializeComponent();


        public object Display(WebuntisClient client)
        {
            this.client = client;
            return this;
        }

        public void Render()
        {
            Webuntis_API.Models.UserInfo.Root user = client!.GetUserInfo();
            Webuntis_API.Models.LessonInfo.Root lessons = user.GetLessons(client);
            List<Webuntis_Desktop.Models.SubjectGradesModel> subjects = new List<SubjectGradesModel>();

            lessons.data.lessons.ForEach(x =>
            {
                SubjectGradesModel subject = new SubjectGradesModel(x.subjects, 6);
                subjects.Add(subject);
                x.GetGrades(user, client).data.grades.ForEach(x => subject.AddMark(x.mark.markDisplayValue));
            });

            List<SubjectGradesEntry> entries = new List<SubjectGradesEntry>();
            double avr = 0;
            subjects.Where(x => x.Noten.Count > 0).ToList().ForEach(x =>
            {
                string delimiter = "; ";
                string markRequired = string.Join(delimiter, MarksToTarget(x.Noten.Where(x => x > 3).ToList(), targetMark)).Trim().Trim(';');
                var entry = CreateSubjectEntry(x.Fach != null ? x.Fach : "", x.Noten.Where(x => x > 3).ToList(), markRequired);
                avr += entry.Grades.Average();
                entries.Add(entry);
            });

            avr /= subjects.Where(x => x.Noten.Count > 0).ToList().Count();
            avr = Math.Round(avr,2);
            Dispatcher.Invoke(() => UI_AvrLable.Content = $"Notendurchschnitt: {avr}");
            Dispatcher.Invoke(() => 
            {
                UI_MarkTable.Set(entries);
                UI_MarkTable.Render();
            });
            OnFinishedLoading?.Invoke(this);
        }

        public Models.SubjectGradesEntry CreateSubjectEntry(string SubjectName, List<double> Grades, string GradesToTarget) =>
            new Models.SubjectGradesEntry(SubjectName, Grades, GradesToTarget);

        /// <summary>
        /// created by ISAAC
        /// </summary>
        /// <param name="marks"></param>
        /// <param name="targetMark"></param>
        /// <returns></returns>
        public IEnumerable<double?> MarksToTarget(List<double> marks, double targetMark)
        {
            do
            {
                var target = Math.Clamp((targetMark * (marks.Count + 1) - marks.Sum()), 4, 10);

                if (target > 4)
                    target = Math.Round(target * 4, 0) / 4;

                yield return target;
                marks.Add(target);
            } while (marks.Average() != 6.0 && marks.Count < 10);
        }

        private void UI_TargetVoteInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (UI_TargetVoteInput.Text.Length <= 0 || UI_TargetVoteInput.Text == " ") return;
            try 
            {
                int input = int.Parse(UI_TargetVoteInput.Text);
                if (input == targetMark || input <= 3) return;
                targetMark = input;

                SharedEvents.RegisterLoadModule(this);
            }
            catch { }
        }

        private void Grid_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^4-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }   
}
