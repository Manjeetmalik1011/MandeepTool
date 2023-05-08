using System.ComponentModel.DataAnnotations;

namespace JCI.RetailSurveyTool.DataBase.Models
{
    public class CustomerIssueTypeMap
    {
        [Key]
        public int ID { get; set; }
        public int CustomerID { get; set; }

        [SQLite.Ignore]
        public virtual Customer Customer { get; set; }
        public int IssueTypeID { get; set; }
        [SQLite.Ignore]
        public virtual IssueType IssueType { get; set; }
    }
}
