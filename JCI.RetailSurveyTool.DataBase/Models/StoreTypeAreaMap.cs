using System.ComponentModel.DataAnnotations;
namespace JCI.RetailSurveyTool.DataBase.Models
{
    public class StoreTypeAreaMap
    {
        [Key]
        public int ID { get; set; }
        public int StoreTypeId { get; set; }
        [SQLite.Ignore]
        public virtual StoreType StoreType { get; set; }
        public int StoreAreaId { get; set; }
        [SQLite.Ignore]
        public virtual StoreArea StoreArea { get; set; }

        public int CustomerID { get; set; }
        [SQLite.Ignore]
        public virtual Customer Customer { get; set; }

    }
}
