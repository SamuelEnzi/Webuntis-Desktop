using System.Net;
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
    }
}
