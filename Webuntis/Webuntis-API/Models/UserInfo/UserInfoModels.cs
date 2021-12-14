using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webuntis_API.Models.UserInfo
{
    public class Person
    {
        public string displayName { get; set; }
        public string imageUrl { get; set; }
        public int id { get; set; }
    }

    public class Permissions
    {
        public List<string> views { get; set; }
    }

    public class User
    {
        public int id { get; set; }
        public string name { get; set; }
        public string locale { get; set; }
        public List<string> roles { get; set; }
        public Person person { get; set; }
        public List<object> students { get; set; }
        public Permissions permissions { get; set; }
    }

    public class Tenant
    {
        public int id { get; set; }
        public string displayName { get; set; }
        public object wuHostName { get; set; }
    }

    public class MessengerData
    {
        public int organizationId { get; set; }
        public string serviceUrl { get; set; }
        public bool hasMessenger { get; set; }
    }

    public class OneDriveData
    {
        public string oneDriveClientId { get; set; }
        public bool hasOneDriveRight { get; set; }
    }

    public class DateRange
    {
        public string start { get; set; }
        public string end { get; set; }
    }

    public class CurrentSchoolYear
    {
        public int id { get; set; }
        public string name { get; set; }
        public DateRange dateRange { get; set; }
    }

    public class Root
    {
        public User user { get; set; }
        public Tenant tenant { get; set; }
        public bool ui2020 { get; set; }
        public bool isPlayground { get; set; }
        public List<object> departments { get; set; }
        public MessengerData messengerData { get; set; }
        public OneDriveData oneDriveData { get; set; }
        public CurrentSchoolYear currentSchoolYear { get; set; }
    }

}
