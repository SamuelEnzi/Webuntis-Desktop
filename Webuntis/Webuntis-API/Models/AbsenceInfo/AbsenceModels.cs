using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webuntis_API.Models.AbsenceInfo
{
    public class Excuse
    {
        public int id { get; set; }
        public string text { get; set; }
        public int excuseDate { get; set; }
        public string excuseStatus { get; set; }
        public bool isExcused { get; set; }
        public int userId { get; set; }
        public string username { get; set; }
    }

    public class Absence
    {
        public int id { get; set; }
        public int startDate { get; set; }
        public int endDate { get; set; }
        public int startTime { get; set; }
        public int endTime { get; set; }
        public object createDate { get; set; }
        public object lastUpdate { get; set; }
        public string createdUser { get; set; }
        public string updatedUser { get; set; }
        public int reasonId { get; set; }
        public string reason { get; set; }
        public string text { get; set; }
        public List<object> interruptions { get; set; }
        public bool canEdit { get; set; }
        public string studentName { get; set; }
        public string excuseStatus { get; set; }
        public bool isExcused { get; set; }
        public Excuse excuse { get; set; }
    }

    public class AbsenceReason
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class Data
    {
        public List<Absence> absences { get; set; }
        public List<AbsenceReason> absenceReasons { get; set; }
        public object excuseStatuses { get; set; }
        public bool showAbsenceReasonChange { get; set; }
        public bool showCreateAbsence { get; set; }
    }

    public class Root
    {
        public Data data { get; set; }
    }

}
