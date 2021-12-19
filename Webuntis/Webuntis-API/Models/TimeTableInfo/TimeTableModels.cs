using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webuntis_API.Models.TimeTableInfo
{
    public class VideoCall
    {
        public object videoCallUrl { get; set; }
        public bool active { get; set; }
    }

    public class Element
    {
        public int type { get; set; }
        public int id { get; set; }
        public int orgId { get; set; }
        public bool missing { get; set; }
        public string state { get; set; }
    }

    public class Is
    {
        public bool standard { get; set; }
        public bool @event { get; set; }
    }

    public class Root
    {
        public int id { get; set; }
        public int lessonId { get; set; }
        public int lessonNumber { get; set; }
        public string lessonCode { get; set; }
        public string lessonText { get; set; }
        public string periodText { get; set; }
        public bool hasPeriodText { get; set; }
        public string periodInfo { get; set; }
        public List<object> periodAttachments { get; set; }
        public string staffText { get; set; }
        public List<object> staffAttachments { get; set; }
        public VideoCall videoCall { get; set; }
        public string substText { get; set; }
        public int date { get; set; }
        public int startTime { get; set; }
        public int endTime { get; set; }
        public List<Element> elements { get; set; }
        public bool hasInfo { get; set; }
        public int code { get; set; }
        public string cellState { get; set; }
        public int priority { get; set; }
        public Is @is { get; set; }
        public int roomCapacity { get; set; }
        public int studentCount { get; set; }
    }
}
