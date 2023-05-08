using Newtonsoft.Json;
using System;


namespace JCI.RetailSurveyTool.DataBase.Models
{
    public class SFUser
    {
        public string Id { get; set; }
        public bool IsDeleted { get; set; }
        public string Name { get; set; }
        public string SVMXC__Country__c { get; set; }
        public string SVMXC__Email__c { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        
    }
}
