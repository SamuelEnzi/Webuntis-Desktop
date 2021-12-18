using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webuntis_API.Models.GradeListInfo
{
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

    public class Exam
    {
        public int markSchemaId { get; set; }
        public int date { get; set; }
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
        public Exam exam { get; set; }
        public int date { get; set; }
        public string text { get; set; }
        public int id { get; set; }
    }

    public class Datum
    {
        public Grade grade { get; set; }
        public string subject { get; set; }
        public int personId { get; set; }
    }

    public class Root
    {
        public List<Datum> data { get; set; }
    }

}
