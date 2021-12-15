﻿using System;
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

        public static Models.GradeInfo.Root GetGrades(this Models.LessionInfo.Lesson lession, Models.UserInfo.Root userinfo, WebuntisClient c) => c.GetGrades(userinfo.user.person.id, lession.id);

        public static Models.LessionInfo.Root GetLessions(this Models.UserInfo.Root userinfo, WebuntisClient c) => c.GetLessions(userinfo.user.person.id, userinfo.currentSchoolYear.id);

        public static Models.AbsenceInfo.Root GetAbsences(this Models.UserInfo.Root userInfo, WebuntisClient c) => c.GetAbsences(userInfo.user.person.id, userInfo.currentSchoolYear.dateRange.start.DateToShorDate(), userInfo.currentSchoolYear.dateRange.end.DateToShorDate());
        
        public static string TimeDiff(this int startDate, int endDate)
        {
            if (startDate.ToString() == null || endDate.ToString() == null) return "";

            var start = startDate.ToString().ParseTimeTuple();
            var end = endDate.ToString().ParseTimeTuple();

            var diffHours = end.Item1 - start.Item1;
            var diffMinutes = end.Item2 - start.Item2;

            return $"{diffHours}h {diffMinutes}m";
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

        /// <summary>
        /// 20220616
        /// </summary>
        public static string DateToShorDate(this string date) => Regex.Replace(date, "[-]", string.Empty);
    }
}
