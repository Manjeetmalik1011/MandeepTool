
using System.ComponentModel.DataAnnotations;

namespace JCI.RetailSurveyTool.DataBase.Models
{
    public class IssueTypeIssueCategoryMap
    {
        [Key]
        public int ID { get; set; }
        public int IssueTypeID { get; set; }
        [SQLite.Ignore]
        public virtual IssueType IssueType { get; set; }
        public int IssueCategoryID { get; set; }
        [SQLite.Ignore]
        public virtual IssueCategory IssueCategory { get; set; }
    }
}
