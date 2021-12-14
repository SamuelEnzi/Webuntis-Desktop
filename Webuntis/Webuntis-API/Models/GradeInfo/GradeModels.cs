using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webuntis_API.Models.GradeInfo
{
    public class Lesson
    {
        public string klassen { get; set; }
        public string teachers { get; set; }
        public string subjects { get; set; }
        public string text { get; set; }
        public int id { get; set; }
    }

    public class Schoolyear
    {
        public bool finalMarkActive { get; set; }
        public string endDate { get; set; }
        public object semesters { get; set; }
        public int schoolyearId { get; set; }
        public string startDate { get; set; }
        public string name { get; set; }
        public int id { get; set; }
    }

    public class SuggestedMark
    {
        public double markDisplayValue { get; set; }
        public int markValue { get; set; }
        public int markSchemaId { get; set; }
        public string name { get; set; }
        public int id { get; set; }
    }

    public class AssignedMark
    {
        public double markDisplayValue { get; set; }
        public int markValue { get; set; }
        public int markSchemaId { get; set; }
        public string name { get; set; }
        public int id { get; set; }
    }

    public class MarkSchema
    {
        public string longname { get; set; }
        public List<object> marks { get; set; }
        public string name { get; set; }
        public int id { get; set; }
    }

    public class FinalMark
    {
        public int id { get; set; }
        public int lessonId { get; set; }
        public Schoolyear schoolyear { get; set; }
        public int studentId { get; set; }
        public SuggestedMark suggestedMark { get; set; }
        public AssignedMark assignedMark { get; set; }
        public double suggestedMarkValue { get; set; }
        public MarkSchema markSchema { get; set; }
        public string text { get; set; }
    }

    public class ExamType
    {
        public string longname { get; set; }
        public int markSchemaId { get; set; }
        public int weightFactor { get; set; }
        public string name { get; set; }
        public int id { get; set; }
    }

    public class Mark
    {
        public double markDisplayValue { get; set; }
        public int markValue { get; set; }
        public int markSchemaId { get; set; }
        public string name { get; set; }
        public int id { get; set; }
    }

    public class Grade
    {
        public int schoolyearId { get; set; }
        public ExamType examType { get; set; }
        public int examTypeId { get; set; }
        public Mark mark { get; set; }
        public int markSchemaId { get; set; }
        public int examId { get; set; }
        public object lastUpdate { get; set; }
        public object exam { get; set; }
        public int date { get; set; }
        public string text { get; set; }
        public int id { get; set; }
    }

    public class Data
    {
        public Lesson lesson { get; set; }
        public List<FinalMark> finalMarks { get; set; }
        public List<Grade> grades { get; set; }
    }

    public class Root
    {
        public Data data { get; set; }
    }

}
