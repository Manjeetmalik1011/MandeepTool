using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JCI.RetailSurveyTool.DataBase.Models
{
    public class IssueStatus
    {
        [Key]
        [MaxLength(100)]
        public string IssueStatusName { set; get; }
        [SQLite.Ignore]
        public virtual List<Issue> Issues { set; get; }
    }
}
