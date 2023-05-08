using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JCI.RetailSurveyTool.DataBase.Models
{
    public class AuditType
    {
        [Key]
        public int ID { set; get; }
        public string Name { set; get; }
        public int? CustomerID { set; get; }
        [SQLite.Ignore]
        public virtual Customer Customer { set; get; }
        [SQLite.Ignore]
        public virtual List<Audit> Audits { set; get; }
    }
}