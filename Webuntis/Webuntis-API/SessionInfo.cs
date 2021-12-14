using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webuntis_API
{
    public class SessionInfo
    {
        public string JSessionID { get; set; }
        public string Schoolname { get; set; }
        public string TraceID { get; set; }
        public string Auth { get; set; }
        public string Token { get; set; }

        public SessionInfo()
        {

        }

        public override string ToString()
        {
            var JsID = JSessionID != null ? $"JSESSIONID={JSessionID};" : string.Empty;
            var ScID = Schoolname != null ? $"schoolname=\"{Schoolname}\";" : string.Empty;
            var TrID = TraceID != null ?    $"traceId={TraceID};" :       string.Empty;
            var Au = Auth != null ?         $"auth={Auth};" :             string.Empty;
            return $"{JsID} {ScID} {TrID} {Au}";
        }
    }
}
