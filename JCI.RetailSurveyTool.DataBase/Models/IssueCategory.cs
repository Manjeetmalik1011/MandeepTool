using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JCI.RetailSurveyTool.DataBase.Models
{
    public class IssueCategory
    {
        [Key]
        public int ID { set; get; }
        /* public int IssueTypeID { set; get; }
         [SQLite.Ignore]
         public virtual IssueType IssueType { set; get; }*/

        [SQLite.Ignore]
        public virtual List<IssueType> IssueTypes { set; get; }
        public string Name { set; get; }
        [SQLite.Ignore]
        public virtual List<Issue> Issues { set; get; } = new List<Issue>();
        public bool Active { get; set; }
    }
}