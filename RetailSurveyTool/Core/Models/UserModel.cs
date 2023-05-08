namespace Jci.RetailSurveyTool.TechnicianApp.Models
{
    public class UserModel
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string GivenName { get; set; }
        public string Mail { get; set; }
        public string OfficeLocation { get; set; }
        public string Surname { get; set; }
        public string UserPrincipalName { get; set; }

        public string Token { get; set; }
    }
}
