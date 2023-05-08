using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JCI.RetailSurveyTool.DataBase.Models
{
    public class DeactivatorType
    {
        [Key]
        public int ID { set; get; }
        public string Name { set; get; }
        [SQLite.Ignore]
        public virtual List<StoreArea> StoreAreas { set; get; }
        [SQLite.Ignore]
        public virtual List<CustomerPreferredDeactivator> CustomerPreferrences { set; get; } = new List<CustomerPreferredDeactivator>();
        [SQLite.Ignore]
        public virtual List<DeactivationInventory> DeactivationInventories { set; get; } = new List<DeactivationInventory>();
    }
}