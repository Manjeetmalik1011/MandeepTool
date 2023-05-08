using Newtonsoft.Json;
using System.Collections.Generic;

namespace JCI.RetailSurveyTool.DataBase.Models
{
    public class MapCaseWO
    {
        [JsonProperty("Id")]
        public string CaseId { get; set; }

        [JsonProperty("SVMXC__Service_Order__r")]
        public SVMXC__Service_Order__r sVMXC__Service_Order__R { get; set; }
    }



    public class SVMXC__Service_Order__r
    {

        [JsonProperty("records")]
        public List<BobWO> BobWOs { set; get; }
    }
}
