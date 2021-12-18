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

            Dispatcher.Invoke(() => {
                UI_ClassBookEntry.DataContent.ItemsSource = classBookEntry;
            });

            OnFinishedLoading?.Invoke(this); 
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
