using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JCI.RetailSurveyTool.DataBase.Models
{
    public class StoreType
    {
        [Key]
        public int ID { set; get; }
        /* public int CustomerID { set; get; }*/
        public string Name { set; get; }
        /* [SQLite.Ignore]
         public virtual Customer Customer { set; get; }*/
        [SQLite.Ignore]
        public virtual List<StoreArea> StoreAreas { set; get; } = new List<StoreArea>();
        [SQLite.Ignore]
        public virtual List<Audit> Audits { set; get; } = new List<Audit>();

        [SQLite.Ignore]
        public virtual List<Customer> Customers { set; get; }
        public bool Active { get; set; }
    }
}
