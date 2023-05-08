using JCI.RetailSurveyTool.DataBase.Models;
using System.Collections.Generic;

namespace Jci.RetailSurveyTool.TechnicianApp.Services
{
    public interface ICustomerDataService
    {
        List<Customer> GetAllCustomers();

        //void AddCustomer(Customer customer);

        //void UpdateCustomer(Customer customer);

    }
}
