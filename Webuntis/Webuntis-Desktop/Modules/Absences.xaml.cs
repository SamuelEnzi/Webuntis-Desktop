using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace Webuntis_Desktop.Modules
{
    /// <summary>
    /// Interaktionslogik für Absences.xaml
    /// </summary>
    public partial class Absences : Page, IModule
    {
        public event IModule.OnFinishedLoadingEventHandler? OnFinishedLoading;
        WebuntisClient? client;

        private Webuntis_API.Models.AbsenceInfo.Root? absenceInfo = null;
        private Webuntis_API.Models.UserInfo.Root? userInfo = null;

        private bool RenderFinished = false;
        public Absences()
        {
            InitializeComponent();
        }


        public object Display(WebuntisClient client)
        {
            this.client = client;
            return this;
        }

        public void Render()
        {
            if (userInfo == null)
                userInfo = client!.GetUserInfo();

            var res = LoadInfo();

            Dispatcher.Invoke(() =>
            {
                UI_AbsencesListView.ItemsSource = res;
            });

            OnFinishedLoading?.Invoke(this);
            RenderFinished = true;
        }

        public object? GetSelectedValue()
        {
            object? selectedValue = null;
            Dispatcher.Invoke(() => selectedValue = UI_TypeSelection.SelectedValue);
            return selectedValue;
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!RenderFinished) return;

            new Thread(() =>
            {
                var res = LoadInfo();
                Dispatcher.Invoke(() => 
                {
                    UI_AbsencesListView.ItemsSource = res;
                });
            }).Start();
        }

        private List<AbsenceModel> LoadInfo()
        {
            List<AbsenceModel> result = new List<AbsenceModel>();
            Webuntis_API.Models.AbsenceInfo.Status selected = Webuntis_API.Models.AbsenceInfo.Status.All;
            Brush background = Brushes.Green;

            Dispatcher.Invoke(() =>
            {
                selected = ToStatus(((ComboBoxItem)GetSelectedValue()!).Content?.ToString()!);
            });

            absenceInfo = userInfo.GetAbsences(client, selected);
            absenceInfo.data.absences.Reverse();
            absenceInfo.data.absences.ForEach(absence =>
            {
                background = StateToColor(absence.isExcused);
                var model = new AbsenceModel($"vom {absence.startDate.ParseDate()} ({absence.startTime.ToString().ToTimeString()})", $"bis {absence.endDate.ParseDate()} ({absence.endTime.ToString().ToTimeString()})", absence.excuseStatus, background, Brushes.White);
                result.Add(model);
            });
            return result;
        }

        private Webuntis_API.Models.AbsenceInfo.Status ToStatus(string status)
        {
            Webuntis_API.Models.AbsenceInfo.Status selected = Webuntis_API.Models.AbsenceInfo.Status.All;

            if (status == Webuntis_API.Models.AbsenceInfo.Status.All.ToString())
                selected = Webuntis_API.Models.AbsenceInfo.Status.All;

            else if (status == Webuntis_API.Models.AbsenceInfo.Status.excused.ToString())
                selected = Webuntis_API.Models.AbsenceInfo.Status.excused;

            else if (status == Webuntis_API.Models.AbsenceInfo.Status.notexcused.ToString())
                selected = Webuntis_API.Models.AbsenceInfo.Status.notexcused;

            else if (status == Webuntis_API.Models.AbsenceInfo.Status.open.ToString())
                selected = Webuntis_API.Models.AbsenceInfo.Status.open;
            else
                selected = Webuntis_API.Models.AbsenceInfo.Status.All;

            return selected;
        }

        private Brush StateToColor(bool state)
        {
            if (state)
                return Brushes.Green;
            return Brushes.Red;
        }

    }

    public class AbsenceModel 
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string AbsenceStatusText { get; set; }
        public Brush BackgroundColor { get; set; }
        public Brush ForgroundColor { get; set; }

        public AbsenceModel(string FromDate, string ToDate, string AbsenceStatusText, Brush BackgroundColor, Brush ForgroundColor)
        {
            this.FromDate = FromDate;
            this.ToDate = ToDate;
            this.AbsenceStatusText = AbsenceStatusText;
            this.BackgroundColor = BackgroundColor;
            this.ForgroundColor = ForgroundColor;
        }
    }


}
