using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webuntis_API.Models.ClassRegEventsInfo
{
    public class Row
    {
        public int id { get; set; }
        public string elementName { get; set; }
        public string subjectName { get; set; }
        public string creatorName { get; set; }
        public int createDate { get; set; }
        public int createTime { get; set; }
        public object eventReasonName { get; set; }
        public object categoryName { get; set; }
        public string text { get; set; }
        public string elemType { get; set; }
    }

    public class Data
    {
        public List<Row> rows { get; set; }
    }

    public class Root
    {
        public Data data { get; set; }
    }
}
