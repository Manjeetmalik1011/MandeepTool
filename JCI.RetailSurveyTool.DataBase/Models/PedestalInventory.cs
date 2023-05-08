using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace JCI.RetailSurveyTool.DataBase.Models
{
    public class PedestalInventory : Inventory, INotifyPropertyChanged
    {

        public override int ID { set; get; }
        public int PedestalQty
        {
            get => pedestalQty; set
            {
                pedestalQty = value;
                NotifyPropertyChanged();
            }
        }
        public int AlarmToneID { set; get; }
        [SQLite.Ignore]
        public virtual AlarmTone AlarmTone
        {
            get => alarmTone; set
            {
                alarmTone = value;
                NotifyPropertyChanged();
            }
        }
        public int SystemTypeID { set; get; }
        [SQLite.Ignore]
        public virtual SystemType SystemType
        {
            get => systemType; set
            {
                systemType = value;
                NotifyPropertyChanged();

            }
        }
        public int SystemQty
        {
            get => systemQty; set
            {
                systemQty = value;
                NotifyPropertyChanged();
            }
        }
        [SQLite.Ignore]
        [NotMapped]
        public string BollardInstalledString => BollardsInstalled ? "Yes" : "No";
        public bool BollardsInstalled { set; get; }
        [SQLite.Ignore]
        [NotMapped]
        public override int TotalQty => SystemQty + PedestalQty;

        public static int MAX_QTY = 99;
        public static int MIN_QTY = 0;

        private Command _IncrementPedQty;
        private Command _DecrementPedQty;
        private Command _IncrementSysQty;
        private Command _DecrementSysQty;
        private int systemQty;
        private int pedestalQty;
        private SystemType systemType;
        private AlarmTone alarmTone;

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            try
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                    if (propertyName == nameof(SystemQty) || propertyName == nameof(PedestalQty))
                    {
                        NotifyPropertyChanged(nameof(TotalQty));
                    }
                    if (propertyName == nameof(SystemType) || propertyName == nameof(AlarmTone))
                    {
                        NotifyPropertyChanged(nameof(Description));
                    }
                }
            }
            catch (Exception ex)
            {

            }

        }

        [SQLite.Ignore]
        [NotMapped]
        public ICommand IncrementPedQty
        {
            get
            {
                if (_IncrementPedQty == null)
                {
                    _IncrementPedQty = new Command(() => PedestalQty++, () => PedestalQty < MAX_QTY, PropertyChanged);
                    PropertyChanged += _IncrementPedQty.PossibleChangeHinted;
                }
                return _IncrementPedQty;
            }
        }
        [SQLite.Ignore]
        [NotMapped]
        public ICommand DecrementPedQty
        {
            get
            {
                if (_DecrementPedQty == null)
                {
                    _DecrementPedQty = new Command(() => PedestalQty--, () => PedestalQty > MIN_QTY, PropertyChanged);
                    PropertyChanged += _DecrementPedQty.PossibleChangeHinted;
                }
                return _DecrementPedQty;
            }
        }

        [SQLite.Ignore]
        [NotMapped]
        public ICommand IncrementSysQty
        {
            get
            {
                if (_IncrementSysQty == null)
                {
                    _IncrementSysQty = new Command(() => SystemQty++, () => SystemQty < MAX_QTY, PropertyChanged);
                    PropertyChanged += _IncrementSysQty.PossibleChangeHinted;
                }
                return _IncrementSysQty;
            }
        }
        [SQLite.Ignore]
        [NotMapped]
        public ICommand DecrementSysQty
        {
            get
            {
                if (_DecrementSysQty == null)
                {
                    _DecrementSysQty = new Command(() => SystemQty--, () => SystemQty > MIN_QTY, PropertyChanged);
                    PropertyChanged += _DecrementSysQty.PossibleChangeHinted;
                }
                return _DecrementSysQty;
            }
        }


        [SQLite.Ignore]
        [NotMapped]
        public override string Description => $"{this.SystemType?.Name} {AlarmTone?.Name}";

    }
}
