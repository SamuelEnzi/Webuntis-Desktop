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
using Webuntis_API;
using Webuntis_Desktop.Models;

namespace Webuntis_Desktop.Modules
{
    /// <summary>
    /// Interaktionslogik für Votes.xaml
    /// </summary>
    public partial class Votes : Page, IModule
    {
        WebuntisClient client;
        public Votes() => InitializeComponent();

        public event IModule.OnFinishedLoadingEventHandler OnFinishedLoading;

        public object Display(WebuntisClient client)
        {
            this.client = client;
            return this;
        }

        public void Render()
        {

            Webuntis_API.Models.UserInfo.Root user = client.GetUserInfo();
            Webuntis_API.Models.LessonInfo.Root lessons = user.GetLessons(client);
            List<Webuntis_Desktop.Models.SubjectGradesModel> subjects = new List<SubjectGradesModel>();

            Dispatcher.Invoke(() =>
            {
                UI_votesOutput.Columns.Add(new DataGridTextColumn() { Header = "Fach" });
            });

            lessons.data.lessons.ForEach(x =>
            {
                SubjectGradesModel subject = new SubjectGradesModel(x.subjects, 6);
                subjects.Add(subject);
                x.GetGrades(user, client).data.grades.ForEach(x =>
                {
                    subject.AddMark(x.mark.markDisplayValue);
                });
            });

            for (int i = 0; i < subjects.Max(x => x.Noten.Count) || i < 8; i++)
            {
                Dispatcher.Invoke(() =>
                {
                    UI_votesOutput.Columns.Add(new DataGridTextColumn());
                });
            }

            Dispatcher.Invoke(() =>
            {
                UI_votesOutput.Columns.Add(new DataGridTextColumn() { Header = "Durchschnitt" });
                UI_votesOutput.Columns.Add(new DataGridTextColumn() { Header = "zum erreichen benötigt" });
                UI_votesOutput.Columns.Add(new DataGridTextColumn() { Header = "Zielnote" });
            });
            //UI_votesOutput.ItemsSource = subjects;
            //UI_votesOutput.Items.Add(subjects.Noten);

            subjects.ForEach(x =>
            {
                List<object> subject = new List<object> { x.Fach, x.Durchschnitt, x.reachTarget, x.Zielnote };
                subject.AddRange(x.Noten.Cast<object>());
                Dispatcher.Invoke(() =>
                {
                    UI_votesOutput.Items.Add(subject);
                });
            });

            OnFinishedLoading?.Invoke(this);
        }
    }
}
