using System;
using System.Net;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace Webuntis_API
{
    public static class Extensions
    {
        public static string GetSetCookieContent(this string header, string parameter)
        {
            /*
               JSESSIONID=7BBB22736D60445934AE71511C6F77FC; Path=/WebUntis; HttpOnly, schoolname="_bGJzLWJyaXhlbg=="; 
               Version=1; Max-Age=1209600; Expires=Tue, 28-Dec-2021 15:09:00 GMT
            */
            if (header == null) return null;

            string trigger = $"{parameter}=";
            string[] lines = header.Split(';');

            string prestring = "";

            foreach (string line in lines)
                if (line.Contains(trigger))
                    prestring = line.Substring(line.IndexOf(trigger) + trigger.Length).Trim();

            if (prestring.StartsWith("\""))
                prestring = prestring.Substring(1); ;

            if (prestring.EndsWith("\""))
                prestring = prestring.Substring(0, prestring.Length - 1);

            return prestring.Trim();
        }

        public static string FindParameter(this string header, string parameter)
        {
            string[] words = header.Split('\n');
            var trigger = $"{parameter}";
            foreach (string word in words)
                if (word.Contains(trigger))
                    return word.Substring(word.IndexOf(trigger) + trigger.Length).Trim();
            return null;
        }

        public static string UrlEncode(this string url) => WebUtility.UrlEncode(url);

        public static bool IsSucceeded(this string headers) => headers.Contains("SUCCESS");

        public static T Deserialize<T>(this string json) => JsonConvert.DeserializeObject<T>(json);

        public static string Serialize(this object o) => JsonConvert.SerializeObject(o);

        public static Models.GradeInfo.Root GetGrades(this Models.LessonInfo.Lesson lession, Models.UserInfo.Root userinfo, WebuntisClient c) => c.GetGrades(userinfo.ToID(), lession.id);

        public static Models.LessonInfo.Root GetLessons(this Models.UserInfo.Root userinfo, WebuntisClient c) => c.GetLessons(userinfo.ToID(), userinfo.currentSchoolYear.id);

        public static Models.AbsenceInfo.Root GetAbsences(this Models.UserInfo.Root userInfo, WebuntisClient c, Models.AbsenceInfo.Status status) => c.GetAbsences(userInfo.ToID(), userInfo.currentSchoolYear.dateRange.start.DateToShorDate(), userInfo.currentSchoolYear.dateRange.end.DateToShorDate(), status);
        
        public static Models.ClassRegEventsInfo.Root GetClassRegEvents(this Models.UserInfo.Root userInfo, WebuntisClient c) => c.GetClassRegEvents(userInfo.ToID(), userInfo.currentSchoolYear.dateRange.start, userInfo.currentSchoolYear.dateRange.end);
        
        public static Models.GradeListInfo.Root GetGaradeList(this Models.UserInfo.Root userInfo, WebuntisClient c, string StartDate, string EndDate) => c.GetGradeList(userInfo.ToID(), StartDate, EndDate);

        public static string DateTimeToString(this DateTime time) => time.ToString("yyyyMMdd");
        
        public static string TimeDiff(this int startDate, int endDate)
        {
            if (startDate.ToString() == null || endDate.ToString() == null) return "";

            var start = startDate.ToString().ParseTimeTuple();
            var end = endDate.ToString().ParseTimeTuple();

            var startmin = start.Item2 + (start.Item1 * 60);
            var endmin = end.Item2 + (end.Item1 * 60);

            var diffMin = endmin - startmin;

            var hours = (int)(diffMin/60);
            var min = (double)((double)((double)(diffMin/60.0) - (double)hours) * 60.0);

            return $"{hours}h {min}m";
        }
        
        public static Tuple<int, int> ParseTimeTuple (this string timeString)
        {
            if(timeString.Length == 4)
            {
                var hours = timeString.Substring(0, 2);
                var minutes = timeString.Substring(2);

                return new Tuple<int, int> (int.Parse(hours), int.Parse(minutes));
            }

            if (timeString.Length == 3)
            {
                var hours = timeString.Substring(0, 1);
                var minutes = timeString.Substring(1);

                return new Tuple<int, int>(int.Parse(hours), int.Parse(minutes));
            }
            return null;
        }

        public static string DateToShorDate(this string date) => Regex.Replace(date, "[-]", string.Empty);

        public static string ParseDate(this string ShortDate)
        {
            string year = ShortDate.Substring(0, 4);
            string month = ShortDate.Substring(4, 2);
            string day = ShortDate.Substring(6, 2);

            return $"{year}-{month}-{day}";
        }

        public static string ParseDate(this int ShortDateInt)
        {
            string ShortDate = ShortDateInt.ToString();
            string year = ShortDate.Substring(0, 4);
            string month = ShortDate.Substring(4, 2);
            string day = ShortDate.Substring(6, 2);

            return $"{year}-{month}-{day}";
        }

        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }
    
        public static int ToID(this Models.UserInfo.Root userInfo)
        {
            if(userInfo.user.person.id != -1)
                return userInfo.user.person.id;

            if (userInfo.user.students.Count > 0)
                return userInfo.user.students[0].id;

            return -1;
        }

        public static string ToName(this Models.UserInfo.Root userInfo)
        {
            if (userInfo.user.person.displayName.Trim() != "")
                return userInfo.user.person.displayName;

            if (userInfo.user.students.Count > 0)
                return userInfo.user.students[0].displayName;

            return "";
        }
    }
}
