using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Webuntis_API
{
    public class WebuntisClient : IDisposable
    {
        public delegate void OnSessionTimeoutEventHandler(Exception e);
        public event OnSessionTimeoutEventHandler OnSessionTimeout;

        public delegate void OnHeartBeatEventHandler(bool success);
        public event OnHeartBeatEventHandler OnHeartBeat;

        private WebuntisBase webuntisBase = new WebuntisBase();

        public WebClient client = new WebClient();
        public SessionInfo sessionInfo { get; private set; } = new SessionInfo();
        public Secret Secret { get; private set; }

        public WebuntisClient(Secret secret)
        {
            this.Secret = secret;

            client.Headers.Add("user-agent", $"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/96.0.4664.45 Safari/537.36");
            client.Headers.Add("Sec-Ch-Ua-Platform", $"\"Windows\"");
            client.Headers.Add("Origin", $"https://mese.webuntis.com");
            client.Headers.Add("Sec-Fetch-Site", $"same-origin");
            client.Headers.Add("Sec-Fetch-Mode", $"cors");
            client.Headers.Add("Sec-Fetch-Dest", $"empty");
            client.Headers.Add("Referer", $"https://mese.webuntis.com/WebUntis/?school=lbs-brixen");
            client.Headers.Add("Accept-Language", $"de-DE,de;q=0.9,en-US;q=0.8,en;q=0.7");
            client.Headers.Add("X-Csrf-Token", $"7a6436ad-7b50-4796-bfb4-20fb77e4dc34");
            client.Headers.Add("Accept", $"application/json");
            client.Headers.Add("Host", $"mese.webuntis.com");
            client.Headers.Add("Content-Type", $"application/x-www-form-urlencoded");
        }


        public bool TryOpen()
        {
            try
            {
                return Open();
            }
            catch
            {
                return false;
            }
        }

        
        /// <summary>
        /// Open a Webuntis session
        /// </summary>
        /// <returns></returns>
        /// <exception cref="WebuntisLoginException"></exception>
        public bool Open()
        {
            var rqurl = webuntisBase.GetSessionEndpoint(this.Secret);
            client.DownloadString(rqurl);
            var temp = client.ResponseHeaders["Set-Cookie"];

            this.sessionInfo.JSessionID = temp.GetSetCookieContent("JSESSIONID");
            this.sessionInfo.Schoolname = temp.GetSetCookieContent("schoolname");

            if (this.sessionInfo.JSessionID == null || this.sessionInfo.Schoolname == null) 
                return false;

            AddHeader("Cookie", this.sessionInfo.ToString());

            var traceURL = webuntisBase.GetTraceEndpoint();
            client.DownloadString(traceURL);

            var headers = client.ResponseHeaders["Set-Cookie"];
            this.sessionInfo.TraceID = headers.ToString().FindParameter("traceId").Substring(1).Trim();

            if (this.sessionInfo.TraceID == null)
                return false;

            AddHeader("Cookie", this.sessionInfo.ToString());

            string data = $"school=lbs-brixen&j_username={this.Secret.Username}&j_password={this.Secret.Password}&token=";
            var secEndpoint = webuntisBase.GetSecurityEndpoint();
            
            client.Headers.Add("Accept", $"application/json");
            client.Headers.Add("Content-Type", $"application/x-www-form-urlencoded");

            var res = client.UploadString(secEndpoint, data);

            var rheaders = client.ResponseHeaders["Set-Cookie"];
            this.sessionInfo.Auth = rheaders.GetSetCookieContent("auth");

            if (!res.IsSucceeded())
                throw new WebuntisLoginException();

            var TokenURL = webuntisBase.GetNewTokenEndpoint();
            this.sessionInfo.Token = client.DownloadString(TokenURL);

            client.Headers.Add("Authorization", $"Bearer {this.sessionInfo.Token.Trim()}");
            client.Headers.Add("Accept", $"application/json, text/plain, */*");

            client.Headers.Add("Referer", $"https://mese.webuntis.com/WebUntis/?school=lbs-brixen");
            client.Headers.Add("Host", $"mese.webuntis.com");

            return true;
        }
        public void Dispose()
        {
            sessionInfo = null;
            Secret = null;
            webuntisBase = null;
            client.Dispose();
        }
        
        /// <summary>
        /// get userinfo
        /// </summary>
        /// <returns></returns>
        public Models.UserInfo.Root GetUserInfo()
        {
            try
            {
                var requestURl = webuntisBase.GetUserInfoEndpoint();
                var userInfo = client.DownloadString(requestURl);

                return userInfo.Deserialize<Models.UserInfo.Root>();
            }
            catch (WebException Exception) 
            {
                OnSessionTimeout?.Invoke(Exception);
                return null;
            }
        }

        /// <summary>
        /// get all lessions
        /// </summary>
        /// <param name="personID"></param>
        /// <param name="schoolyearID"></param>
        /// <returns></returns>
        public Models.LessonInfo.Root GetLessons(int personID, int schoolyearID = 7)
        {
            try
            {
                var url = webuntisBase.GetLessonsEndpoint(personID, schoolyearID);
                var LessionInfo = client.DownloadString(url);

                return LessionInfo.Deserialize<Models.LessonInfo.Root>();
            }
            catch (WebException Exception)
            {
                OnSessionTimeout?.Invoke(Exception);
                return null;
            }
        }

        /// <summary>
        /// Load Gades from lession
        /// </summary>
        /// <param name="personID"></param>
        /// <param name="lessionID"></param>
        /// <returns></returns>
        public Models.GradeInfo.Root GetGrades(int personID, int lessionID)
        {
            try
            {
                var url = webuntisBase.GetGradeInfoEndpoint(personID, lessionID);
                var GradesInfo = client.DownloadString(url);

                return GradesInfo.Deserialize<Models.GradeInfo.Root>();
            }
            catch (WebException Exception)
            {
                OnSessionTimeout?.Invoke(Exception);
                return null;
            }
        }

        /// <summary>
        /// Load Absences
        /// </summary>
        /// <param name="personID"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public Models.AbsenceInfo.Root GetAbsences(int personID, string startDate, string endDate, Models.AbsenceInfo.Status status)
        {
            try
            {
                var url = webuntisBase.GetAbsenceEndpoint(personID, startDate, endDate, status);
                var info = client.DownloadString(url);

                return info.Deserialize<Models.AbsenceInfo.Root>();
            }
            catch (WebException Exception)
            {
                OnSessionTimeout?.Invoke(Exception);
                return null;
            }
        }

        /// <summary>
        /// get grades in time range
        /// </summary>
        /// <param name="personID"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public Models.GradeListInfo.Root GetGradeList(int personID, string startDate, string endDate)
        {
            try
            {
                var url = webuntisBase.GetGradeListEndpoint(personID, startDate, endDate);
                var info = client.DownloadString(url);

                return info.Deserialize<Models.GradeListInfo.Root>();
            }
            catch (WebException Exception)
            {
                OnSessionTimeout?.Invoke(Exception);
                return null;
            }
        }

        /// <summary>
        /// Load ClassRegEvents
        /// </summary>
        /// <param name="personID"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public Models.ClassRegEventsInfo.Root GetClassRegEvents(int personID, string startDate, string endDate)
        {
            try
            {
                var url = webuntisBase.GetClassRegEventsEndpoint(personID, startDate, endDate);
                var info = client.DownloadString(url);

                return info.Deserialize<Models.ClassRegEventsInfo.Root>();
            }
            catch (WebException Exception)
            {
                OnSessionTimeout?.Invoke(Exception);
                return null;
            }
        }

        /// <summary>
        /// get time table as string object
        /// </summary>
        /// <param name="personID"></param>
        /// <param name="startOfWeekDate"></param>
        /// <returns></returns>
        public string GetTimeTableInfo(int personID, string startOfWeekDate)
        {
            try
            {
                var url = webuntisBase.GetTimeTableInfoEndpoint(personID, startOfWeekDate);
                return client.DownloadString(url);
            }
            catch (WebException Exception)
            {
                OnSessionTimeout?.Invoke(Exception);
                return null;
            }
            //return info.Deserialize<dynamic>();
        }

        /// <summary>
        /// syncronous heartbeat
        /// </summary>
        public bool HeartBeat()
        {
            try
            {
                var res = this.GetUserInfo();
                if(res != null)
                    return true;
                return false;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// asnycronous heart beat
        /// </summary>
        public void HeartBeatAsync()
        {
            new Thread(() =>
            {
                try
                {
                    using (WebuntisClient client = new WebuntisClient(this.Secret))
                    {
                        client.client.Headers = this.client.Headers;
                        client.GetUserInfo();
                        OnHeartBeat?.Invoke(true);
                    }
                }
                catch { OnHeartBeat?.Invoke(false); }
            }).Start();
        }

        /// <summary>
        /// add a header to requests
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        private void AddHeader(string key, string value) => client.Headers.Add(key, value);

    }
}
