using System;
using System.Collections.Generic;
using System.Linq;
using Webuntis_API;

namespace WebuntisApiTest
{
    internal class Program
    {
        static WebuntisClient client;
        static void Main(string[] args)
        {
            Console.Write("Username: ");
            var un = Console.ReadLine();
            Console.Clear();

            Console.Write("Password: ");
            var pw = Console.ReadLine();
            Console.Clear();


            client = new WebuntisClient(new Secret(un, pw));
            bool success = client.Open();
            if (!success) return;

            var UserInfo = client.GetUserInfo();
            var Lessions = UserInfo.GetLessions(client);

            List<Grade> gradesCollection = new List<Grade>();

            foreach (var Lession in Lessions.data.lessons)
            {
                Console.Write($"{Lession.subjects} => ");
                double avr = 0.0;
                string gradesString = "";
                var grades = Lession.GetGrades(UserInfo, client).data.grades;

                grades = grades.Where(x => x.mark.markDisplayValue >= 4).ToList();

                grades.ForEach(grade =>
                {
                    avr += grade.mark.markDisplayValue;
                    gradesString += $"{grade.mark.markDisplayValue};";
                });

                avr /= grades.Count;


                gradesCollection.Add(new Grade(Lession.subjects, gradesString, avr));
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"OK");
                Console.ResetColor();
            }

            Console.WriteLine();

            gradesCollection = gradesCollection.OrderBy(x => x.avr).ToList();

            gradesCollection.ForEach(grade =>
            {
                if (grade.avr > 6)
                    Console.ForegroundColor = ConsoleColor.Green;
                else if (grade.avr < 6)
                    Console.ForegroundColor = ConsoleColor.Red;
                else if (grade.avr == 6)
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                else
                    Console.ForegroundColor = ConsoleColor.White;

                Console.WriteLine($"{grade.Subject}: {grade.gradesString} --- avr: ({grade.avr})");
            });



            var absences = UserInfo.GetAbsences(client, Webuntis_API.Models.AbsenceInfo.Status.All);

            if (absences.data.absences.Count > 0)
            {
                Console.ResetColor();
                Console.WriteLine();
                Console.WriteLine("Absenzen");

                absences.data.absences.ForEach(absences =>
                {
                    Console.WriteLine($"reason: {absences.excuseStatus} => {absences.createdUser} FROM {absences.startDate} to {absences.endDate} :: {absences.startTime.TimeDiff(absences.endTime)}");
                });
            }



            Console.ReadKey();
        }
    }

    public class Grade
    {
        public string Subject { get; set; }
        public string gradesString { get; set; }
        public double avr { get; set; }

        public Grade(string Subject, string gradeString, double avr)
        {
            this.Subject = Subject;
            this.gradesString = gradeString;
            this.avr = avr;
        }
    }

}
