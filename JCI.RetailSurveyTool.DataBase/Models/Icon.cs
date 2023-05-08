using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JCI.RetailSurveyTool.DataBase.Models
{
    public class Icon
    {
        [Key]
        public int ID { set; get; }
        public byte[] IconImage { set; get; }
        public string MimeType { set; get; }
        [SQLite.Ignore]
        public virtual List<StoreArea> StoreAreas { set; get; } = new List<StoreArea>();
    }
}