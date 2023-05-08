using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JCI.RetailSurveyTool.DataBase.Models
{
    public class SystemType
    {
        [Key]
        public int ID { set; get; }
        public string Name { set; get; }
        [SQLite.Ignore]
        public virtual List<PedestalInventory> PedestalInventories { set; get; } = new List<PedestalInventory>();
        [SQLite.Ignore]
        public virtual List<CustomerPreferredSystem> CustomerPreferredSystems { set; get; } = new List<CustomerPreferredSystem>();
    }
}