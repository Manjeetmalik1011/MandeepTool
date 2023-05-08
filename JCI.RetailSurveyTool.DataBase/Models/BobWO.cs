using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace JCI.RetailSurveyTool.DataBase.Models
{
    public class BobWO
    {

        public string Id { get; set; }
        public string OwnerId { get; set; }
        public string SVMXC__Group_Member__c { get; set; }
        public string CaseId { get; set; }
        public bool IsDeleted { get; set; }
        public string Name { get; set; }
        [SQLite.Ignore]
        [NotMapped]
        public Case SVMXC__Case__r { get; set; }
        [SQLite.Ignore]
        [NotMapped]
        public SVMXC__Company__r SVMXC__Company__r { get; set; }
        public string RecordTypeId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedById { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string LastModifiedById { get; set; }
        public string SVMXC__City__c { get; set; }
        public string SVMXC__Country__c { get; set; }
        public bool SVMXC__Is_PM_Work_Order__c { get; set; }
        public string SVMXC__Order_Status__c { get; set; }
        public string SVMXC__Order_Type__c { get; set; }
        public string SVMXC__Problem_Description__c { get; set; }
        public string SVMXC__Purpose_of_Visit__c { get; set; }

        public string SVMXC__Site__c { get; set; }
        public string SVMXC__State__c { get; set; }
        public string SVMXC__Street__c { get; set; }
        public string SVMXC__Zip__c { get; set; }
        public string SVMXFLD_CaseStatus__c { get; set; }

        public string SVCINT_Case_Number__c { get; set; }
        public string SVMXFLD_Location_Name__c { get; set; }

        public string Customer { get; set; }
        public string ERP_Customer_Code__c { get; set; }
        public bool AuditCompleted { get; set; }
    }
}




