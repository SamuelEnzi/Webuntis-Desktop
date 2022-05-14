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

namespace Webuntis_Desktop.UserControls
{
    /// <summary>
    /// Interaktionslogik für WU_MarkTable.xaml
    /// </summary>
    public partial class WU_MarkTable : UserControl
    {
        public List<Models.SubjectGradesEntry> SubjectGradesEntries { get; set; } = new();
        
        public WU_MarkTable()
        {
            InitializeComponent();
        }

        public void Add(Models.SubjectGradesEntry subjectGradesEntry) =>
            SubjectGradesEntries.Add(subjectGradesEntry);

        public void Set(List<Models.SubjectGradesEntry> SubjectGradesEntries) =>
            this.SubjectGradesEntries = SubjectGradesEntries;
        
        public void Clear() =>
            SubjectGradesEntries.Clear();

        public void Render()
        {
            UI_SubjectContainer.Children.Clear();
            SubjectGradesEntries.ForEach((subject) =>
            {
                var element = new WU_MarkEntry(subject.SubjectName, subject.Avrage.ToString(), subject.GradesToTarget, subject.Grades, subject.AvrageForeground);
                UI_SubjectContainer.Children.Add(element);
            });
        }
    }
}
