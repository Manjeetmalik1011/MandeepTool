using Jci.RetailSurveyTool.TechnicianApp.Contracts.Repository;
using System;
using System.Threading.Tasks;

namespace Jci.RetailSurveyTool.TechnicianApp.Repository
{
    public class GenericRepository : IGenericRepository
    {
        public Task DeleteAsync(string uri, string authToken = "")
        {
            throw new NotImplementedException();
        }

        public Task<T> GetAsync<T>(string uri, string authToken = "")
        {
            throw new NotImplementedException();
        }

        public Task<T> PostAsync<T>(string uri, T data, string authToken = "")
        {
            throw new NotImplementedException();
        }

        public Task<R> PostAsync<T, R>(string uri, T data, string authToken = "")
        {
            throw new NotImplementedException();
        }

        public Task<T> PutAsync<T>(string uri, T data, string authToken = "")
        {
            throw new NotImplementedException();
        }
    }
}
