using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Webuntis_API;

namespace WebuntisApiTest
{
    internal class Program
    {
        static WebuntisClient client;
        static Secret secret = null;
        static void Main(string[] args)
        {
            if (File.Exists("_rtData.efs"))
                secret = Secret.LoadSecret(File.ReadAllText("_rtData.efs"));

            if(secret == null)
            {
                Console.Write("Username: ");
                var un = Console.ReadLine();
                Console.Clear();

                Console.Write("Password: ");
                var pw = Console.ReadLine();
                Console.Clear();

                secret = new Secret(un, pw);

                File.WriteAllText("_rtData.efs", secret.GetProtectedSecret());
            }
            else
            {
                Console.WriteLine($"loaded secret for {secret.Username}");
            }

            client = new WebuntisClient(secret);
            bool success = client.TryOpen();
            if (!success)
            {
                File.Delete("_rtData.efs");
                return;
            }

            var UserInfo = client.GetUserInfo();
            var Lessions = UserInfo.GetLessons(client);

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

            double hours = 0;
            double mins = 0;

            double hoursTotal = 0;
            double minsTotal = 0;

            if (absences.data.absences.Count > 0)
            {
                Console.ResetColor();
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Absenzen");
                Console.ResetColor();
                var last = "";
                absences.data.absences.ForEach(absences =>
                {
                    string timeDiff =  absences.startTime.TimeDiff(absences.endTime);
                    var timeDiffTuple =  absences.startTime.TimeDiffTuple(absences.endTime);
                    Console.WriteLine($"reason: {absences.excuseStatus} => {absences.createdUser} FROM {absences.startDate} to {absences.endDate} :: {timeDiff}");
                    last = timeDiff;
                    try
                    {
                        if (absences.excuseStatus == "unentschuldigt" || absences.excuseStatus == "noch nicht entschuldigt")
                        {
                            hours += timeDiffTuple.Item1;
                            mins += timeDiffTuple.Item2;
                        }

                        hoursTotal += timeDiffTuple.Item1;
                        minsTotal += timeDiffTuple.Item2;
                    }
                    catch (Exception e) { Console.WriteLine($"{e.Message}: {last}"); }
                });
            }

            hours += mins / 60;
            double hd = ((double)mins / 60.0);
            mins = (int)((double)((double)hd - (int)hd) * 60.0);


            hoursTotal += minsTotal / 60;
            double hdTotal = ((double)minsTotal / 60.0);
            minsTotal = (int)((double)((double)hdTotal - (int)hdTotal) * 60.0);

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"insgesamte unentschuldigten fehlstunden: {(int)hours}h {(int)mins}m");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"insgesamte fehlstunden: {(int)hoursTotal}h {(int)minsTotal}m");
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
