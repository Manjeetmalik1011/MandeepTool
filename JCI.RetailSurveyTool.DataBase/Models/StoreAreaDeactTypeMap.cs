using System.ComponentModel.DataAnnotations;

namespace JCI.RetailSurveyTool.DataBase.Models
{
    public class StoreAreaDeactivatorTypeMap
    {
        [Key]
        public int ID { get; set; }
        public int StoreAreaID { get; set; }

        [SQLite.Ignore]
        public virtual StoreArea StoreArea { get; set; }
        public int DeactivatorTypeID { get; set; }
        [SQLite.Ignore]
        public virtual DeactivatorType DeactivatorType { get; set; }

    }
}
