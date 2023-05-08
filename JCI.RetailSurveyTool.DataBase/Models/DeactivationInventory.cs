using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace JCI.RetailSurveyTool.DataBase.Models
{
    public class DeactivationInventory : Inventory, INotifyPropertyChanged
    {
        private Command _IncrementQty;
        private int qty = 0;

        public override int ID { set; get; }
        public int DeactivatorTypeID { set; get; }
        [SQLite.Ignore]
        public virtual DeactivatorType DeactivatorType
        {
            get => deactivatorType; set
            {
                deactivatorType = value;
                NotifyPropertyChanged();
            }
        }
        public int Qty
        {
            get => qty; set
            {
                qty = value;
                NotifyPropertyChanged(nameof(Qty));
            }
        }
        public int? SlimPadCoversNeeded { set; get; }
        public int NumberOfRegisters
        {
            get => numberOfRegisters; set
            {
                numberOfRegisters = value;
                NotifyPropertyChanged(nameof(NumberOfRegisters));
            }
        }
        public string SelfCheckoutVendor { set; get; }
        [SQLite.Ignore]
        [NotMapped]
        public override int TotalQty => Qty;
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                if (propertyName == nameof(Qty))
                {
                    NotifyPropertyChanged(nameof(TotalQty));
                }
                if (propertyName == nameof(DeactivatorType))
                {
                    NotifyPropertyChanged(nameof(Description));
                }
            }
        }
        public static int MAX_QTY = 99;
        public static int MIN_QTY = 0;
        private Command _DecrementQty;
        private Command _IncrementRegQty;
        private Command _DecrementRegQty;
        private int numberOfRegisters = 0;
        private DeactivatorType deactivatorType;

        [SQLite.Ignore]
        [NotMapped]
        public ICommand IncrementQty
        {
            get
            {
                if (_IncrementQty == null)
                {
                    _IncrementQty = new Command(() => Qty++, () => Qty < MAX_QTY, PropertyChanged);
                    PropertyChanged += _IncrementQty.PossibleChangeHinted;
                }
                return _IncrementQty;
            }
        }
        [SQLite.Ignore]
        [NotMapped]
        public ICommand DecrementQty
        {
            get
            {
                if (_DecrementQty == null)
                {
                    _DecrementQty = new Command(() => Qty--, () => Qty > MIN_QTY, PropertyChanged);
                    PropertyChanged += _DecrementQty.PossibleChangeHinted;
                }
                return _DecrementQty;
            }
        }

        [SQLite.Ignore]
        [NotMapped]
        public ICommand IncrementRegQty
        {
            get
            {
                if (_IncrementRegQty == null)
                {
                    _IncrementRegQty = new Command(() => NumberOfRegisters++, () => NumberOfRegisters < MAX_QTY, PropertyChanged);
                    PropertyChanged += _IncrementRegQty.PossibleChangeHinted;
                }
                return _IncrementRegQty;
            }
        }
        [SQLite.Ignore]
        [NotMapped]
        public ICommand DecrementRegQty
        {
            get
            {
                if (_DecrementRegQty == null)
                {
                    _DecrementRegQty = new Command(() => NumberOfRegisters--, () => NumberOfRegisters > MIN_QTY, PropertyChanged);
                    PropertyChanged += _DecrementRegQty.PossibleChangeHinted;
                }
                return _DecrementRegQty;
            }
        }
        [SQLite.Ignore]
        [NotMapped]
        public override string Description => DeactivatorType?.Name ?? "Unknown";
    }
}
