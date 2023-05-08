using System;
using System.Text.RegularExpressions;

namespace JCI.RetailSurveyTool.DataBase.Models
{
    public class SVMX_WO
    {
        public string ERP_CUST_NUM { get; set; }
        public string ERP_DEL_CODE { get; set; }
        public string SITE_ID { get; set; }
        public string STORE_NUM { get; set; }
        public string LOCATION_NAME { get; set; }
        public string ADDRESS { get; set; }
        public string CITY { get; set; }
        public string STATE { get; set; }
        public string ZIP { get; set; }
        public string LOCATION_PHONE { get; set; }
        public string COUNTRY { get; set; }
        public string CASE_ID { get; set; }
        public string CASE_NUMBER { get; set; }
        public string CASE_RECORD_TYPE { get; set; }
        [SQLite.Ignore]
        public string CASE_SUBJECT { get; set; }
        public string NEW_PO { get; set; }
        public string CASE_STATUS { get; set; }
        public string CASE_TYPE { get; set; }
        public string PROJECT_ID { get; set; }
        public string PRODUCT_FAMILY { get; set; }
        public string WORK_ORDER_ID { get; set; }
        [SQLite.PrimaryKey]
        public string WORK_ORDER { get; set; }
        public string ORDER_STATUS { get; set; }
        public string ORDER_TYPE { get; set; }
        public string FIELD_STATUS { get; set; }
        public string REF_A { get; set; }
        public string REF_B { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public DateTime? DATE_ASSIGNED_TECH { get; set; }
        public DateTime? SCHEDULE_DATE { get; set; }
        public DateTime? CLOSED_DATE { get; set; }
        public string PROBLEM_DESCRIPTION { get; set; }
        public string PURPOSE_OF_VISIT { get; set; }
        public string WORK_PERFORMED { get; set; }
        public string COMMENTS { get; set; }
        public string NOTES { get; set; }
        public bool IS_PM_WORK_ORDER { get; set; }
        public string SERVICE_GROUP_ID { get; set; }
        public string SERVICE_REGION { get; set; }
        public string DISTRICT { get; set; }
        public string SERVICE_GROUP_NAME { get; set; }
        public string OWNER_ID { get; set; }
        public string OWNER_NAME { get; set; }
        public string OWNER_EMAIL { get; set; }
        public bool Consumed { set; get; } = false;
        public string GetStoreNumber()
        {
            Regex storeNumberRegex = new Regex(@"#(\d*)");
            return storeNumberRegex.Match(this.LOCATION_NAME)?.Groups[0]?.Value;
        }
    }
}
