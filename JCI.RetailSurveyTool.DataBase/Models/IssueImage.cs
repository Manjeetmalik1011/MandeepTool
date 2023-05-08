using System.ComponentModel.DataAnnotations;

namespace JCI.RetailSurveyTool.DataBase.Models
{
    public class IssueImage
    {
        [Key]
        public int ID { set; get; }
        public byte[] Image { set; get; }
        public string MimeType { set; get; }
        public int IssueID { set; get; }
        [SQLite.Ignore]
        public virtual Issue Issue { set; get; }
    }
}