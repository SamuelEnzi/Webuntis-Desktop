using Newtonsoft.Json.Linq;
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
using Webuntis_API.Models.TimeTableInfo;



namespace Webuntis_Desktop.Modules
{
    /// <summary>
    /// Interaktionslogik für TimeTable.xaml
    /// </summary>
    public partial class TimeTable : Page, IModule
    {
        public delegate void OnLessionHoverEventHandler(int index);
        public event OnLessionHoverEventHandler? OnLessionHover;

        public delegate void OnLessionHoverEnterEventHandler(int index);
        public event OnLessionHoverEnterEventHandler? OnLessionHoverEnter;

        public delegate void OnLessionHoverExitEventHandler(int index);
        public event OnLessionHoverExitEventHandler? OnLessionHoverExit;

        public event IModule.OnFinishedLoadingEventHandler? OnFinishedLoading;
        WebuntisClient? client;

        private Webuntis_API.Models.LessonInfo.Root? lessonInfo = null;
        private Webuntis_API.Models.UserInfo.Root? userInfo = null;

        private List<SubjectModel> subjects = new List<SubjectModel>();
        private Views.SubjectFollowPopup? SubjectFollowPopup;


        public TimeTable() => InitializeComponent();
        public object Display(WebuntisClient client)
        {
            SubjectFollowPopup = new Views.SubjectFollowPopup();
            SubjectFollowPopup.Show();
            SubjectFollowPopup!.Visibility = Visibility.Hidden;

            this.client = client;
            this.OnLessionHover += OnMouseOverLession;
            this.OnLessionHoverEnter += OnMouseOverLessionEnter;
            this.OnLessionHoverExit += OnMouseOverLessionExit;
            return this;
        }

        private void OnMouseOverLessionExit(int index)
        {
            SubjectFollowPopup!.Visibility = Visibility.Hidden;
        }

        private void OnMouseOverLessionEnter(int index)
        {
            var current = subjects[index];
            SubjectFollowPopup!.Set(current.TeacherName!, current.Subject!, current.StartTime!, current.EndTime!, current.Text! ,current.Color!);
            SubjectFollowPopup!.Visibility = Visibility.Visible;
        }

        private void OnMouseOverLession(int index)
        {
            MoveBottomRightEdgeOfWindowToMousePosition();
        }

        Point GetMousePos() => this.PointToScreen(Mouse.GetPosition(this));

        private void MoveBottomRightEdgeOfWindowToMousePosition()
        {
            var transform = PresentationSource.FromVisual(this).CompositionTarget.TransformFromDevice;
            var mouse = transform.Transform(GetMousePos());
            SubjectFollowPopup!.Left = mouse.X + 10;
            SubjectFollowPopup!.Top = mouse.Y + 10;
        }

        public void Render()
        {
            if (userInfo == null)
                userInfo = client!.GetUserInfo();

            if (lessonInfo == null)
                lessonInfo = userInfo.GetLessons(client);

            string id = userInfo.ToID().ToString();
            var monday = DateTime.Now.StartOfWeek(DayOfWeek.Monday);
            string name = $"{monday.Year}-{monday.Month}-{monday.Day}";

            var timeTableInfo = client!.GetTimeTableInfo(userInfo.ToID(), name);

            var parsed = JObject.Parse(timeTableInfo);
            var token = parsed["data"]?["result"]?["data"]?["elementPeriods"]?[id];

            Webuntis_API.Models.TimeTableInfo.Root[] TimeTableInfo = token!.ToString().Deserialize<Webuntis_API.Models.TimeTableInfo.Root[]>();

            var Days = TimeTableInfo.ToList();
            var GroupedDays = from TimeTable in TimeTableInfo group TimeTable by TimeTable.date;

            int i = 0;
            Redraw();
            foreach (var days in GroupedDays.OrderBy(k => k.Key))
            {
                foreach(var hours in days.OrderBy(d => d.startTime).GroupBy(d=>d.startTime))
                {
                    var Subject = "";
                    var Teacher = "";
                    var Class = "";
                    List<Webuntis_API.Models.LessonInfo.Lesson> lsh = new List<Webuntis_API.Models.LessonInfo.Lesson>();

                    Brush color = Brushes.Orange;

                    foreach (var lession in hours)
                    {
                        lsh = lessonInfo.data.lessons.Where(x => x.id == lession.lessonId).ToList();
                        Subject += lsh[0].subjects + "/";
                        Teacher += lsh[0].teachers + "/";
                        Class += lsh[0].klassen + "/";

                        if (lession.cellState == CellState.EXAM.ToString())
                            color = Brushes.Yellow;
                        else if (lession.cellState == CellState.STANDARD.ToString())
                            color = Brushes.Orange;
                        else if (lession.cellState == CellState.SUBSTITUTION.ToString())
                            color = Brushes.Red;
                    }

                    Subject = Subject.TrimEnd('/');
                    Teacher = Teacher.TrimEnd('/');
                    Class = Class.TrimEnd('/');

                    string text = string.Empty;
                    try
                    {
                        text = hours.First().lessonText;
                    }
                    catch { }

                    var current = new SubjectModel()
                    {
                        Color = color,
                        Text = text,
                        Subject = Subject,
                        TeacherName = Teacher,
                        StartTime = hours.First().startTime.ToString(),
                        EndTime = hours.First().endTime.ToString()
                    };

                    subjects.Add(current);
                    var index = subjects.IndexOf(current);

                    Dispatcher.Invoke(()=> 
                    {
                        var element = GenerateLession(Subject, Teacher, Class, color, index);
                        Insert(element, i);
                    });
                }
                i++;
            }

            OnFinishedLoading?.Invoke(this);
        }

        private void Insert(UIElement element, int day)
        {
            if(day == 0)
                Monday.Children.Add(element);
            
            else if (day == 1)
                Tuesday.Children.Add(element);
            
            else if (day == 2)
                Wednesday.Children.Add(element);
            
            else if (day == 3)
                Thursday.Children.Add(element);
            
            else if (day == 4)
                Friday.Children.Add(element);
        }

        private void Redraw()
        {
            subjects.Clear();
            Dispatcher.Invoke(() =>
            {
                Monday.Children.Clear();
                Tuesday.Children.Clear();
                Wednesday.Children.Clear();
                Thursday.Children.Clear();
                Friday.Children.Clear();

                Monday.Children.Add(GenerateHeader("MO", Brushes.White));
                Tuesday.Children.Add(GenerateHeader("DI", Brushes.White));
                Wednesday.Children.Add(GenerateHeader("MI", Brushes.White));
                Thursday.Children.Add(GenerateHeader("DO", Brushes.White));
                Friday.Children.Add(GenerateHeader("FR", Brushes.White));
            });
        }

        public UIElement GenerateHeader(string text, Brush backgroundColor)
        {
            Grid Container = new Grid();
            Container.Margin = new Thickness(5);
            Container.Height = 30;

            Label Title = new Label();
            Title.Content = text;
            Title.FontSize = 15;
            Title.HorizontalAlignment = HorizontalAlignment.Center;
            Title.VerticalAlignment = VerticalAlignment.Center;

            Rectangle background = new Rectangle();
            background.Fill = backgroundColor;

            Container.Children.Add(background);
            Container.Children.Add(Title);

            return Container;
        }

        public UIElement GenerateLession( string Subject, string TeacherName, string ClassName ,Brush backgroundColor, int hoverIndex = -1)
        {
            Grid Background = new Grid();
            Background.Margin = new Thickness(5, 5, 5, 0);

            Grid Container = new Grid();
            Container.Margin = new Thickness(0, 5, 5, 5);
            Container.Height = 55;

            Container.RowDefinitions.Add(new RowDefinition());
            Container.RowDefinitions.Add(new RowDefinition());
            
            Grid horizontalGrid = new Grid();
            horizontalGrid.ColumnDefinitions.Add(new ColumnDefinition());
            horizontalGrid.ColumnDefinitions.Add(new ColumnDefinition());

            Label ClassNameLable = new Label();
            ClassNameLable.Content = ClassName;
            ClassNameLable.FontSize = 12;
            ClassNameLable.HorizontalAlignment = HorizontalAlignment.Center;
            horizontalGrid.Children.Add(ClassNameLable);
            Grid.SetColumn(ClassNameLable, 0);

            Label TeacherNameLable = new Label();
            TeacherNameLable.Content = TeacherName;
            TeacherNameLable.FontSize = 12;
            TeacherNameLable.HorizontalAlignment = HorizontalAlignment.Center;
            horizontalGrid.Children.Add(TeacherNameLable);
            Grid.SetColumn(ClassNameLable, 1);

            Container.Children.Add(horizontalGrid);
            Grid.SetRow(horizontalGrid, 0);
            
            Label SubjectLable = new Label();
            SubjectLable.Content = Subject;
            SubjectLable.FontSize = 15;
            SubjectLable.HorizontalAlignment = HorizontalAlignment.Center;
            Container.Children.Add(SubjectLable);
            Grid.SetRow(SubjectLable, 1);

            Rectangle background = new Rectangle();
            background.Fill = backgroundColor;
            background.Opacity = 0.6;
            Background.Children.Add(background);

            Background.Children.Add(Container);

            Rectangle hoverRect = new Rectangle();
            hoverRect.Fill = Brushes.Transparent;
            Background.Children.Add(hoverRect);

            hoverRect.MouseMove += (v, x) =>
            {
                OnLessionHover?.Invoke(hoverIndex);
            };

            hoverRect.MouseLeave += (v, x) =>
            {
                OnLessionHoverExit?.Invoke(hoverIndex);
            };

            hoverRect.MouseEnter += (v, y) =>
            {
                OnLessionHoverEnter?.Invoke(hoverIndex);
            };


            return Background;
        }

        sealed private class SubjectModel
        {
            public string? Subject { get; set; }
            public string? StartTime { get; set; }
            public string? EndTime { get; set; }
            public string? TeacherName { get; set; }
            public string? Text { get; set; }
            public Brush? Color { get; set; }
            public override string ToString() => $"{Subject}, {TeacherName}, {StartTime}, {EndTime}";
        }
    }
}
