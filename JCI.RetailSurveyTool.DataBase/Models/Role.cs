using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JCI.RetailSurveyTool.DataBase.Models
{
    public class Role
    {
        [Key]
        public int ID { set; get; }
        public bool IsCustomerTied { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
        [SQLite.Ignore]
        public virtual List<UserRole> UserRoles { set; get; } = new List<UserRole>();
    }
}
