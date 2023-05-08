using SQLite;
using System;

namespace JCI.RetailSurveyTool.DataBase.Models
{
    public class Case
    {

        [PrimaryKey]
        public string Id { get; set; }
        public bool IsDeleted { get; set; }
        public string MasterRecordId { get; set; }
        public string CaseNumber { get; set; }
        public string ContactId { get; set; }
        public string AccountId { get; set; }
        public string ParentId { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string Reason { get; set; }
        public string Subject { get; set; }
        public string Priority { get; set; }
        public string Description { get; set; }
        public DateTime? ClosedDate { get; set; }
        public string OwnerId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedById { get; set; }
        public DateTime? LastModifiedDate { get; set; }

         
    }
}
