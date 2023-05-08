using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;

namespace JCI.RetailSurveyTool.DataBase.Models
{
    public class StoreArea : INotifyPropertyChanged
    {
        [Key]
        public int ID { set; get; }
        /*public int StoreTypeID { set; get; }
        [SQLite.Ignore]
        public virtual StoreType StoreType { set; get; }*/
        public string Name { set; get; }
        public bool DeactivationArea { set; get; }
        public bool SelfCheckoutArea { set; get; }
        public bool PedestalArea { set; get; }
        public int IconID { set; get; }
        [SQLite.Ignore]
        public virtual Icon Icon { set; get; }
        [SQLite.Ignore]
        public virtual List<Inventory> Inventories { set; get; } = new List<Inventory>();
        [SQLite.Ignore]
        public virtual List<Issue> Issues { set; get; } = new List<Issue>();
        public bool Active { get; set; }

        [SQLite.Ignore]
        public virtual List<StoreType> StoreTypes { get; set; }

        [SQLite.Ignore]
        public virtual List<DeactivatorType> DeactivatorTypes { get; set; } = new List<DeactivatorType>();

        [SQLite.Ignore]
        [NotMapped]
        public string InventoryStatus
        {
            get
            {
                if (Inventories.Count > 0)
                {
                    return $"Inventoried {Inventories.Sum(x => x.TotalQty)} Devices";
                }
                else
                {
                    return "Inventory Not Completed";
                }
            }
        }
        [SQLite.Ignore]
        [NotMapped]
        public string IssueStatus
        {
            get
            {
                if (Issues.Count > 0)
                {
                    return $"{Issues.Count} Issues Logged";
                }
                else
                {
                    return "No Issues Logged";
                }
            }
        }

        [SQLite.Ignore]
        [NotMapped]
        public bool IssueInventoryLogged
        {
            get
            {
                if (Issues.Count > 0 || Inventories.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
