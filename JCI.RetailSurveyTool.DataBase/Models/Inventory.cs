using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JCI.RetailSurveyTool.DataBase.Models
{
    public abstract class Inventory
    {
        [Key]
        public virtual int ID
        {
            set => this.ID = value;
            get => this.ID;

        }
        public int AuditID { set; get; }
        [SQLite.Ignore]
        public virtual Audit Audit { set; get; }
        public int StoreAreaID { set; get; }
        [SQLite.Ignore]
        public virtual List<ProactiveSalesLead> Leads { set; get; }
        [SQLite.Ignore]
        public virtual StoreArea StoreArea { set; get; }
        [SQLite.Ignore]
        [NotMapped]
        public abstract int TotalQty { get; }
        [SQLite.Ignore]
        [NotMapped]
        public abstract string Description { get; }

        public bool IsOperational { get; set; }
    }
}