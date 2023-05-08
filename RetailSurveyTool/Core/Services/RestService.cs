using Jci.RetailSurveyTool.TechnicianApp.Utility;
using JCI.RetailSurveyTool.DataBase.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Jci.RetailSurveyTool.TechnicianApp.Services
{
    public class RestService
    {
        private readonly string baseURL;
        HttpClient client;
        private JsonSerializerSettings serializerSettings;

        public RestService(string baseURL)
        {
            client = new HttpClient();
            this.baseURL = baseURL;
            DefaultContractResolver contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            this.serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = contractResolver,
                Formatting = Formatting.Indented,
                TypeNameHandling = TypeNameHandling.Auto
            };


        }
        public async Task<Audit> GetAuditAsync(int id)
        {

            Uri uri = new Uri(baseURL + $"Audit/{id}");
           
            HttpResponseMessage response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Audit>(content, serializerSettings);

            }
            else
            {
                throw new Exception(await ErrorHandler.APICustomExceptionMsg(response));
            }
            

        }
        public async Task<List<Audit>> SearchForAudits(int customerID, string storeNumber, int top = 10, int skip = 0)
        {

            Uri uri = new Uri(baseURL + $"Audit/byCustomer/{customerID}/{storeNumber}?top={top}&skip={skip}");
            
            HttpResponseMessage response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Audit>>(content, serializerSettings);

            }
            else
            {
                throw new Exception(await ErrorHandler.APICustomExceptionMsg(response));
            }
            
           
        }

        public async Task<List<SVMX_WO>> GetWOsAsync()
        {
            Uri uri = new Uri(baseURL + $"SVMAXWO/WOs/");

            HttpResponseMessage response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<SVMX_WO>>(content, serializerSettings);
            }
            else
            {
                throw new Exception(await ErrorHandler.APICustomExceptionMsg(response));
            }
        }

        
        public async Task<List<SVMX_WO>> GetWOByTechAsync(string email)
        {
            //email = "fernando.castaneda@jci.com";
            Uri uri = new Uri(baseURL + $"SVMAXWO/byTech/{email}");
           
            HttpResponseMessage response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<SVMX_WO>>(content, serializerSettings);
            }
            else
            {
                throw new Exception(await ErrorHandler.APICustomExceptionMsg(response));
            }
        }


        public async Task<SVMX_WO> GetWOByNumberAsync(string number)
        {
            //https://jciretailsurveytoolwebportaltest.azurewebsites.net/api/SVMAXWO/byWO/WO-01575850
            Uri uri = new Uri(baseURL + $"SVMAXWO/byWO/{number}");
           
            HttpResponseMessage response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<SVMX_WO>(content, serializerSettings);

            }
            else
            {
                throw new Exception(await ErrorHandler.APICustomExceptionMsg(response));
            }
            
           

        }

        public async Task<T> PostItem<T>(T item) where T : class
        {

            var content = JsonConvert.SerializeObject(item, new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Auto
            });

            return await PostItem<T>(content);

        }
        public async Task<T> PostItem<T>(String content) where T : class
        {
            Uri uri = new Uri(baseURL + typeof(T).Name);
            HttpResponseMessage response = await client.PostAsync(uri, new StringContent(content, Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(responseContent, serializerSettings);
            }
            else
            {
                throw new Exception(await ErrorHandler.APICustomExceptionMsg(response));
            }
           
        }
        public async Task PutItem<T>(T item, string pathEnd) where T : class
        {
            Uri uri = new Uri(baseURL + typeof(T).Name + "/" + pathEnd);
            
            var content = JsonConvert.SerializeObject(item, new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Auto
            });

            HttpResponseMessage response = await client.PutAsync(uri, new StringContent(content, Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
            }
            else
            {
                throw new Exception(await ErrorHandler.APICustomExceptionMsg(response));
            }
             
        }

        public async Task<T> PutAudit<T>(String content, String pathEnd) where T : class
        {
            Uri uri = new Uri(baseURL + typeof(T).Name + "/" + pathEnd);
            HttpResponseMessage response = await client.PutAsync(uri, new StringContent(content, Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(responseContent, serializerSettings);
            }
            else
            {
                throw new Exception(await ErrorHandler.APICustomExceptionMsg(response));
            }
        }

        public async Task<List<T>> GetItems<T>() where T : class
        {
            
            Uri uri = new Uri(baseURL + typeof(T).Name);

            HttpResponseMessage response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<T>>(content, serializerSettings);

            }
            else
            {
                throw new Exception(await ErrorHandler.APICustomExceptionMsg(response));
            }
        }
    }
}
