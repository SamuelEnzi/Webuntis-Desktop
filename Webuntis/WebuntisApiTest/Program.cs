using System;
using Webuntis_API;

namespace WebuntisApiTest
{
    internal class Program
    {
        static WebuntisClient client;
        static void Main(string[] args)
        {
            client = new WebuntisClient(new Secret("enz-sam", "AmanakoiAmana1!"));
            bool success = client.Open();
            if (!success) return;

            var UserInfo = client.GetUserInfo();
            var Lessions = client.GetLessions(UserInfo.user.person.id, UserInfo.currentSchoolYear.id);
            var PMGrades = client.GetGrades(UserInfo.user.person.id, 24090); //24090 project management

        }

        
    }
}
