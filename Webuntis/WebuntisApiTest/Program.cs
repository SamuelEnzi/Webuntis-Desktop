﻿using System;
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

            Console.WriteLine(UserInfo.currentSchoolYear.dateRange.start.DateToShorDate());
            Console.WriteLine(UserInfo.currentSchoolYear.dateRange.end.DateToShorDate());
            Console.WriteLine();

            foreach(var Lession in Lessions.data.lessons)
            {
                double avr = 0.0;
                string gradesString = "";
                var grades = Lession.GetGrades(UserInfo, client).data.grades;

                grades.ForEach(grade => 
                {
                    avr += grade.mark.markDisplayValue;
                    gradesString += $"{grade.mark.markDisplayValue};";
                });

                avr /= grades.Count;

                Console.WriteLine($"{Lession.subjects}: {gradesString} --- avr: ({avr})");
            }

            Console.WriteLine();
            Console.WriteLine("Absenzen");

            var absences = UserInfo.GetAbsences(client);
            absences.data.absences.ForEach(absences =>
            {
                Console.WriteLine($"reason: {absences.excuseStatus} => {absences.createdUser} FROM {absences.startDate} to {absences.endDate} :: {absences.startTime.TimeDiff(absences.endTime)}");
            });

            Console.ReadKey();
        }        
    }
}
