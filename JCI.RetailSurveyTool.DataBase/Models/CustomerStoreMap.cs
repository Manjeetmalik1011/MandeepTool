using System.ComponentModel.DataAnnotations;

namespace JCI.RetailSurveyTool.DataBase.Models
{
    public class CustomerStoreMap
    {


        [Key]
        public int ID { get; set; }
        public int CustomerID { get; set; }

        [SQLite.Ignore]
        public virtual Customer Customer { get; set; }
        public int StoreTypeID { get; set; }
        [SQLite.Ignore]
        public virtual StoreType StoreType { get; set; }



    }
}
