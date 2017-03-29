using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ekaH_server.Models
{
    public class Submission 
    {
        public int ID { get; set; }
        public int AssignmentID { get; set; }
        public string StudentID { get; set; }
        public byte[] RawContent { get; set; }
        public int Grade { get; set; }
        public DateTime SubmittedTime { get; set; }
    }

    public class Assignment
    {
        public int ID { get; set; }
        public string CourseID { get; set; }
        public int Project { get; set; }
        public string ProjectTitle { get; set; }
        public int Weight { get; set; }
        public DateTime Deadline { get; set; }
        public string InplaceContent { get; set; }
        public byte[] Attachments { get; set; }
    }

    public class SubmissionGrade
    {
        public int Grade { get; set; }
        public Submission Submission { get; set; }
    }

}