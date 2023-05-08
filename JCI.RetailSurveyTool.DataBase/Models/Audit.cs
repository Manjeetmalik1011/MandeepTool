using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JCI.RetailSurveyTool.DataBase.Models
{
    public class Audit
    {
        [Key]
        public int ID { set; get; }
        public int AuditTypeID { set; get; }
        [SQLite.Ignore]
        public virtual AuditType AuditType { set; get; }
        public string ServiceCallNumber { set; get; }
        public string Status { set; get; }
        public TimeSpan AuditHours { set; get; }
        public string AuditDescription { set; get; }
        public string StoreNumber { set; get; }
        public string StoreContact { set; get; }
        public DateTime Created { set; get; }
        public DateTime Completed { set; get; }
        public int StoreTypeID { set; get; }
        public string TechnicianName { set; get; }
        public string TechnicianEmail { set; get; }
        public string BobWOId { set; get; }
        public string LocationName { set; get; }
        public string Address { set; get; }
        public string City { set; get; }
        public string State { set; get; }
        public string Zip { set; get; }

        [SQLite.Ignore]
        public virtual StoreType StoreType { set; get; }
        [SQLite.Ignore]
        public virtual List<Issue> Issues { set; get; } = new List<Issue>();
        [SQLite.Ignore]
        public virtual List<Inventory> Inventories { set; get; } = new List<Inventory>();


        public string GetAuditTime()
        {
            return string.Format("{0:D2}:{1:D2}", AuditHours.Hours, AuditHours.Minutes);
        }
    }
}