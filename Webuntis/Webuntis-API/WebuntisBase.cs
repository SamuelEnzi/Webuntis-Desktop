using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webuntis_API
{
    public class WebuntisBase
    {
        public string BaseURL { get; set; } = "https://mese.webuntis.com";
        public string GetSessionEndpoint(Secret s) => $"{BaseURL}/WebUntis/?school={s.SchoolName}";
        public string GetTraceEndpoint() => $"{BaseURL}/WebUntis/api/help/helpmapping";
        public string GetSecurityEndpoint() => $"{BaseURL}/WebUntis/j_spring_security_check";
        public string GetNewTokenEndpoint() => $"{BaseURL}/WebUntis/api/token/new";
        public string GetUserInfoEndpoint() => $"{BaseURL}/WebUntis/api/rest/view/v1/app/data";
        public string GetLessionsEndpoint(int personID, int schoolyearID = 7) => $"{BaseURL}/WebUntis/api/classreg/grade/grading/list?schoolyearId={schoolyearID}&studentId={personID}";
        public string GetGradeInfoEndpoint(int personID, int lessionID) => $"{BaseURL}/WebUntis/api/classreg/grade/grading/lesson?studentId={personID}&lessonId={lessionID}";
    }
}
