using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JCI.RetailSurveyTool.DataBase.Models
{
    public class IssueArea
    {
        [Key]
        [MaxLength(100)]
        public string IssueAreaName { set; get; }
        public virtual List<Issue> Issues { set; get; }
    }
}
