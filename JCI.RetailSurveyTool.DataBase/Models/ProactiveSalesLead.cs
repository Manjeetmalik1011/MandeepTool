using System.ComponentModel.DataAnnotations;

namespace JCI.RetailSurveyTool.DataBase.Models
{
    public class ProactiveSalesLead
    {
        [Key]
        public int ID { set; get; }
        public virtual ProactiveSalesLeadStatus Status { set; get; }
        public virtual Inventory Source { set; get; }
    }
}
