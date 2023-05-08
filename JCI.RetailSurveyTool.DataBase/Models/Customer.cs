using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JCI.RetailSurveyTool.DataBase.Models
{
    public class Customer
    {
        [Key]
        public int ID { set; get; }
        public string Name { set; get; }
        [SQLite.Ignore]
        public virtual List<UserRole> UserRoles { set; get; } = new List<UserRole>();
        [SQLite.Ignore]
        public virtual List<CustomerErpMapping> ErpMappings { set; get; } = new List<CustomerErpMapping>();
        [SQLite.Ignore]
        public virtual List<StoreType> StoreTypes { set; get; } = new List<StoreType>();
        [SQLite.Ignore]
        public virtual List<CustomerPreferredDeactivator> PreferredDeactivatorTypes { set; get; } = new List<CustomerPreferredDeactivator>();
        [SQLite.Ignore]
        public virtual List<CustomerPreferredSystem> PreferredSystemTypes { set; get; } = new List<CustomerPreferredSystem>();
        [SQLite.Ignore]
        public virtual List<AuditType> CustomerSpecificAuditTypes { set; get; } = new List<AuditType>();
        [SQLite.Ignore]
        public virtual List<IssueType> IssueTypes { set; get; } = new List<IssueType>();

        public bool Active { get; set; }

    }
}
