using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webuntis_API.Models.LessonInfo
{
    public class Lesson
    {
        public string klassen { get; set; }
        public string teachers { get; set; }
        public string subjects { get; set; }
        public string text { get; set; }
        public int id { get; set; }
    }

    public class Data
    {
        public List<Lesson> lessons { get; set; }
    }

    public class Root
    {
        public Data data { get; set; }
    }

}
