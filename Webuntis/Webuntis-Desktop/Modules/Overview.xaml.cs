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

namespace Webuntis_Desktop.Modules
{
    /// <summary>
    /// Interaktionslogik für Overview.xaml
    /// </summary>
    public partial class Overview : Page, IModule
    {
        WebuntisClient client;
        public Overview() => InitializeComponent();

        public event IModule.OnFinishedLoadingEventHandler OnFinishedLoading;


        private Webuntis_API.Models.UserInfo.Root? userInfo = null;
        private Webuntis_API.Models.ClassRegEventsInfo.Root? classRegInfo = null;
        private List<ClassBookEntry>? classBookEntry = null;

        private Webuntis_API.Models.AbsenceInfo.Root? absenceInfo = null;
        private List<AbsenceEntry>? absenceEntry = null;

        private Webuntis_API.Models.GradeListInfo.Root? gradeInfo = null;
        private List<GradeEntry>? gradeEntry = null;

        public object Display(WebuntisClient client)
        {
           
            this.client = client;
            return this;
        }

        public void Render()
        {
            if(userInfo == null)
                userInfo = client.GetUserInfo();
          
            if(classRegInfo == null)
                classRegInfo = userInfo.GetClassRegEvents(client);

            if(classBookEntry == null)
            {
                classBookEntry = new List<ClassBookEntry>();
                classRegInfo.data.rows.ForEach((x) => classBookEntry.Add(new Modules.ClassBookEntry(x.creatorName, x.text)));
            }

            Dispatcher.Invoke(() => 
            {
                UI_ClassBookEntry.DataContent.ItemsSource = classBookEntry;
            });

            if (absenceInfo == null)
            {
                absenceInfo = userInfo.GetAbsences(client, Webuntis_API.Models.AbsenceInfo.Status.All);
                absenceEntry = new List<AbsenceEntry>();

                absenceInfo.data.absences.ForEach((x) =>
                {
                    absenceEntry.Add(new AbsenceEntry(x.startDate.ParseDate(), x.endDate.ParseDate(),x.excuse.excuseStatus));
                });
            }

            Dispatcher.Invoke(() =>
            {
                UI_OpenAbsences.DataContent.ItemsSource = absenceEntry;
            });

            if(gradeInfo == null)
            {
                DateTime start = DateTime.Now.AddDays(-32);
                DateTime end = DateTime.Now;

                gradeInfo = userInfo.GetGaradeList(client, start.DateTimeToString(), end.DateTimeToString());
                gradeEntry = new List<GradeEntry>();

                gradeInfo.data.ForEach((x) =>
                {
                    gradeEntry.Add(new GradeEntry(x.grade.date, x.subject, x.grade.examType.name, x.grade.exam != null ? x.grade.exam.name : "", x.grade.mark.markDisplayValue.ToString()));
                });

                gradeEntry = (from entry in gradeEntry orderby entry.date descending select entry).ToList();
            }

            Dispatcher.Invoke(() => 
            {
                UI_Grades.DataContent.ItemsSource = gradeEntry;
            });

            OnFinishedLoading?.Invoke(this); 
        }
    }

    public class GradeEntry
    {
        public string Subject { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Grade { get; set; }

        public int date;
        public GradeEntry(int date, string Subject, string Type, string Name, string Grade)
        {
            this.date = date;
            this.Subject = $"{Subject}"; 
            this.Type = $"    {Type}"; 
            this.Name = $"    {Name}"; 
            this.Grade = $"    {Grade}"; 
        }
    }


    public class AbsenceEntry
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Status { get; set; }

        public AbsenceEntry(string From, string To, string Status)
        {
            this.From = $"vom: {From}   ";
            this.To = $" zu: {To}   ";
            this.Status = $" status: {Status}    ";
        }
    }

    public class ClassBookEntry
    {
        public string Teacher { get; set; }
        public string Content { get; set; }
        public ClassBookEntry(string Teacher, string Content)
        {
            this.Teacher = Teacher;
            this.Content = Content;
        }
}}
