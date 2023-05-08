using Jci.RetailSurveyTool.TechnicianApp.Data;
using JCI.RetailSurveyTool.DataBase.Models;
using System.Collections.Generic;


namespace Jci.RetailSurveyTool.TechnicianApp.Services
{
    public class CustomerDataService : ICustomerDataService
    {
        //readonly SQLiteAsyncConnection database;
        //public SQLiteAsyncConnection GetRawConnection() => database;

        public LocalAppDatabase LocalAppDatabase => DependencyService.Resolve<LocalAppDatabase>();

        //public ObservableCollection<Customer> Customers { get; set; }

        //protected async Task LoadTableAsync<T>(ObservableCollection<T> collection, Func<Task<IEnumerable<T>>> source)
        //{

        //    try
        //    {
        //        collection.Clear();
        //        foreach (T item in await source.Invoke())
        //        {
        //            collection.Add(item);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(ex);
        //    }

        //}
        //protected async Task LoadTableAsync<T>(ObservableCollection<T> collection, Func<Task<List<T>>> source)
        //{
        //    try
        //    {
        //        collection.Clear();
        //        foreach (T item in await source.Invoke())
        //        {
        //            collection.Add(item);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(ex);
        //    }
        //}


        public List<Customer> GetAllCustomers()
        {
            var obj1 = LocalAppDatabase.GetRawConnection().Table<Customer>().ToListAsync().Result;

            return obj1;
        }



        //public void AddCustomer(Customer customer)
        //{
        //    CustomerRepository.AddCustomer(customer);
        //}

        //public void UpdateCustomer(Customer customer)
        //{
        //    CustomerRepository.UpdateCustomer(customer);
        //}
    }
}
