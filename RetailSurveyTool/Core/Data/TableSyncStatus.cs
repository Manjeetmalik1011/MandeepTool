using System;

namespace Jci.RetailSurveyTool.TechnicianApp.Data
{
    public class TableSyncStatus
    {
        [SQLite.AutoIncrement]
        public int ID { set; get; }
        public string Table { set; get; }
        public DateTime LastSync { set; get; }
    }
}
