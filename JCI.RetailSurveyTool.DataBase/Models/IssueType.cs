using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JCI.RetailSurveyTool.DataBase.Models
{
    public class IssueType
    {
        [Key]
        public int ID { set; get; }
        public string Name { set; get; }
        public bool ShowForSystem { set; get; }
        public bool ShowForDeactivation { set; get; }
        /* public int? CustomerID { set; get; }
         [SQLite.Ignore]
         public virtual Customer Customer { set; get; } */
        [SQLite.Ignore]
        public virtual List<Customer> Customers { set; get; } = new List<Customer>();
        [SQLite.Ignore]
        public virtual List<Issue> Issues { set; get; } = new List<Issue>();
        [SQLite.Ignore]
        public virtual List<IssueCategory> IssueCategories { set; get; } = new List<IssueCategory>();
        public bool Active { get; set; }
    }
}