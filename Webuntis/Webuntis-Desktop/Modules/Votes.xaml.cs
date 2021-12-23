using System;
using System.Collections.Generic;
using System.Data;
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

            DataTable data = new DataTable();
            data.Columns.Add("Fach");

            lessons.data.lessons.ForEach(x =>
            {
                SubjectGradesModel subject = new SubjectGradesModel(x.subjects, 6);
                subjects.Add(subject);
                x.GetGrades(user, client).data.grades.ForEach(x => subject.AddMark(x.mark.markDisplayValue));
            });

            int columnCount = Math.Max(subjects.Max(x => x.Noten.Count), 8);
            for (int i = 0; i < columnCount; i++)
            {
                DataColumn column = new DataColumn();
                column.ColumnName = i.ToString();
                data.Columns.Add(column);
            }

            data.Columns.Add("Durchschnitt");
            data.Columns.Add("Gerundet");

            subjects.Where(x => x.Noten.Count > 0).ToList().ForEach(x =>
            {
                List<object> subject = new List<object> { x.Fach! };
                subject.AddRange(x.Noten.Where(x=> x > 3).ToList().Cast<object>());
                if (subject.Count < columnCount + 1)
                    subject.AddRange(new string[columnCount - (subject.Count - 1)]);
                subject.Add(x.Durchschnitt);
                subject.Add(x.Gerundet);

                DataRow dr = data.NewRow();
                dr.ItemArray = subject.ToArray();

                data.Rows.Add(dr);
            });

            Dispatcher.Invoke(() => UI_votesOutput.ItemsSource = data.DefaultView);
            OnFinishedLoading?.Invoke(this);
        }
    }
}
