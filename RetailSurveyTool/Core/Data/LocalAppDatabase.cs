using Jci.RetailSurveyTool.TechnicianApp.Services;
using JCI.RetailSurveyTool.DataBase.Models;
using Nito.AsyncEx;
using SQLite;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jci.RetailSurveyTool.TechnicianApp.Data
{
    public partial class LocalAppDatabase
    {
        readonly SQLiteAsyncConnection database;
        public RestService restService { get; }

        public LocalAppDatabase(RestService restService, string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            //var createTablesTask = CreateTables();
            var task = Task.Run(() => AsyncContext.Run(() => CreateTables()));
            task.GetAwaiter().GetResult();

            this.restService = restService;
        }

        private async Task CreateTables()
        {
            var types = EntitySyncMethods.Select(x => x.Key).ToArray();
            await database.CreateTablesAsync(CreateFlags.ImplicitPK | CreateFlags.ImplicitIndex, types);
        }

        public SQLiteAsyncConnection GetRawConnection() => database;


        public Task<List<AlarmTone>> GetAlarmToneAsync()
        {
            //Get all notes.
            return database.Table<AlarmTone>().ToListAsync();
        }

        public Task<AlarmTone> GetAlarmToneAsync(int id)
        {
            // Get a specific note.
            return database.Table<AlarmTone>()
                            .Where(i => i.ID == id)
                            .FirstOrDefaultAsync();
        }

        public Task<int> SaveAlarmToneAsync(AlarmTone obj)
        {
            if (obj.ID != 0)
            {
                // Update an existing AlarmTone.
                return database.UpdateAsync(obj);
            }
            else
            {
                // Save a new AlarmTone.
                return database.InsertAsync(obj);
            }
        }

        public Task<int> DeleteAlarmToneAsync(AlarmTone obj)
        {
            // Delete a AlarmTone.
            return database.DeleteAsync(obj);
        }


        public Task<List<AuditType>> GetAuditTypeAsync()
        {
            //Get all notes.
            return database.Table<AuditType>().ToListAsync();
        }

        public Task<AuditType> GetAuditTypeAsync(int id)
        {
            // Get a specific note.
            return database.Table<AuditType>()
                            .Where(i => i.ID == id)
                            .FirstOrDefaultAsync();
        }

        public Task<int> SaveAuditTypeAsync(AuditType obj)
        {
            if (obj.ID != 0)
            {
                // Update an existing AuditType.
                return database.UpdateAsync(obj);
            }
            else
            {
                // Save a new AuditType.
                return database.InsertAsync(obj);
            }
        }

        public Task<int> DeleteAuditTypeAsync(AuditType obj)
        {
            // Delete a AuditType.
            return database.DeleteAsync(obj);
        }


        public Task<List<Audit>> GetAuditAsync()
        {
            //Get all notes.
            return database.Table<Audit>().ToListAsync();
        }

        public Task<Audit> GetAuditAsync(int id)
        {
            // Get a specific note.
            return database.Table<Audit>()
                            .Where(i => i.ID == id)
                            .FirstOrDefaultAsync();
        }

        public Task<int> SaveAuditAsync(Audit obj)
        {
            if (obj.ID != 0)
            {
                // Update an existing Audit.
                return database.UpdateAsync(obj);
            }
            else
            {
                // Save a new Audit.
                return database.InsertAsync(obj);
            }
        }

        public Task<int> DeleteAuditAsync(Audit obj)
        {
            // Delete a Audit.
            return database.DeleteAsync(obj);
        }


        public Task<List<Customer>> GetCustomerAsync()
        {
            //Get all notes.
            return database.Table<Customer>().ToListAsync();
        }

        public Task<Customer> GetCustomerAsync(int id)
        {
            // Get a specific note.
            return database.Table<Customer>()
                            .Where(i => i.ID == id)
                            .FirstOrDefaultAsync();
        }

        public Task<int> SaveCustomerAsync(Customer obj)
        {
            if (obj.ID != 0)
            {
                // Update an existing Customer.
                return database.UpdateAsync(obj);
            }
            else
            {
                // Save a new Customer.
                return database.InsertAsync(obj);
            }
        }

        public Task<int> DeleteCustomerAsync(Customer obj)
        {
            // Delete a Customer.
            return database.DeleteAsync(obj);
        }


        public Task<List<CustomerErpMapping>> GetCustomerErpMappingAsync()
        {
            //Get all notes.
            return database.Table<CustomerErpMapping>().ToListAsync();
        }

        public Task<CustomerErpMapping> GetCustomerErpMappingAsync(int id)
        {
            // Get a specific note.
            return database.Table<CustomerErpMapping>()
                            .Where(i => i.ID == id)
                            .FirstOrDefaultAsync();
        }

        public Task<int> SaveCustomerErpMappingAsync(CustomerErpMapping obj)
        {
            if (obj.ID != 0)
            {
                // Update an existing CustomerErpMapping.
                return database.UpdateAsync(obj);
            }
            else
            {
                // Save a new CustomerErpMapping.
                return database.InsertAsync(obj);
            }
        }

        public Task<int> DeleteCustomerErpMappingAsync(CustomerErpMapping obj)
        {
            // Delete a CustomerErpMapping.
            return database.DeleteAsync(obj);
        }


        public Task<List<CustomerPreferredDeactivator>> GetCustomerPreferredDeactivatorAsync()
        {
            //Get all notes.
            return database.Table<CustomerPreferredDeactivator>().ToListAsync();
        }

        public Task<CustomerPreferredDeactivator> GetCustomerPreferredDeactivatorAsync(int id)
        {
            // Get a specific note.
            return database.Table<CustomerPreferredDeactivator>()
                            .Where(i => i.ID == id)
                            .FirstOrDefaultAsync();
        }

        public Task<int> SaveCustomerPreferredDeactivatorAsync(CustomerPreferredDeactivator obj)
        {
            if (obj.ID != 0)
            {
                // Update an existing CustomerPreferredDeactivator.
                return database.UpdateAsync(obj);
            }
            else
            {
                // Save a new CustomerPreferredDeactivator.
                return database.InsertAsync(obj);
            }
        }

        public Task<int> DeleteCustomerPreferredDeactivatorAsync(CustomerPreferredDeactivator obj)
        {
            // Delete a CustomerPreferredDeactivator.
            return database.DeleteAsync(obj);
        }

        public Task<List<CustomerPreferredSystem>> GetCustomerPreferredSystemAsync()
        {
            //Get all notes.
            return database.Table<CustomerPreferredSystem>().ToListAsync();
        }

        public Task<CustomerPreferredSystem> GetCustomerPreferredSystemAsync(int id)
        {
            // Get a specific note.
            return database.Table<CustomerPreferredSystem>()
                            .Where(i => i.ID == id)
                            .FirstOrDefaultAsync();
        }

        public Task<int> SaveCustomerPreferredSystemAsync(CustomerPreferredSystem obj)
        {
            if (obj.ID != 0)
            {
                // Update an existing CustomerPreferredSystem.
                return database.UpdateAsync(obj);
            }
            else
            {
                // Save a new CustomerPreferredSystem.
                return database.InsertAsync(obj);
            }
        }

        public Task<int> DeleteCustomerPreferredSystemAsync(CustomerPreferredSystem obj)
        {
            // Delete a CustomerPreferredSystem.
            return database.DeleteAsync(obj);
        }

        public Task<List<DeactivationInventory>> GetDeactivationInventoryAsync()
        {
            //Get all notes.
            return database.Table<DeactivationInventory>().ToListAsync();
        }

        public Task<DeactivationInventory> GetDeactivationInventoryAsync(int id)
        {
            // Get a specific note.
            return database.Table<DeactivationInventory>()
                            .Where(i => i.ID == id)
                            .FirstOrDefaultAsync();
        }

        public Task<int> SaveDeactivationInventoryAsync(DeactivationInventory obj)
        {
            if (obj.ID != 0)
            {
                // Update an existing DeactivationInventory.
                return database.UpdateAsync(obj);
            }
            else
            {
                // Save a new DeactivationInventory.
                return database.InsertAsync(obj);
            }
        }

        public Task<int> DeleteDeactivationInventoryAsync(DeactivationInventory obj)
        {
            // Delete a DeactivationInventory.
            return database.DeleteAsync(obj);
        }

        public Task<List<DeactivatorType>> GetDeactivatorTypeAsync()
        {
            //Get all notes.
            return database.Table<DeactivatorType>().ToListAsync();
        }

        public Task<DeactivatorType> GetDeactivatorTypeAsync(int id)
        {
            // Get a specific note.
            return database.Table<DeactivatorType>()
                            .Where(i => i.ID == id)
                            .FirstOrDefaultAsync();
        }

        public Task<int> SaveDeactivatorTypeAsync(DeactivatorType obj)
        {
            if (obj.ID != 0)
            {
                // Update an existing DeactivatorType.
                return database.UpdateAsync(obj);
            }
            else
            {
                // Save a new DeactivatorType.
                return database.InsertAsync(obj);
            }
        }

        public Task<int> DeleteDeactivatorTypeAsync(DeactivatorType obj)
        {
            // Delete a DeactivatorType.
            return database.DeleteAsync(obj);
        }


        public Task<List<Icon>> GetIconAsync()
        {
            //Get all notes.
            return database.Table<Icon>().ToListAsync();
        }

        public Task<Icon> GetIconAsync(int id)
        {
            // Get a specific note.
            return database.Table<Icon>()
                            .Where(i => i.ID == id)
                            .FirstOrDefaultAsync();
        }

        public Task<int> SaveIconAsync(Icon obj)
        {
            if (obj.ID != 0)
            {
                // Update an existing Icon.
                return database.UpdateAsync(obj);
            }
            else
            {
                // Save a new Icon.
                return database.InsertAsync(obj);
            }
        }

        public Task<int> DeleteIconAsync(Icon obj)
        {
            // Delete a Icon.
            return database.DeleteAsync(obj);
        }


        public Task<List<Issue>> GetIssueAsync()
        {
            //Get all notes.
            return database.Table<Issue>().ToListAsync();
        }

        public Task<Issue> GetIssueAsync(int id)
        {
            // Get a specific note.
            return database.Table<Issue>()
                            .Where(i => i.ID == id)
                            .FirstOrDefaultAsync();
        }

        public Task<int> SaveIssueAsync(Issue obj)
        {
            if (obj.ID != 0)
            {
                // Update an existing Issue.
                return database.UpdateAsync(obj);
            }
            else
            {
                // Save a new Issue.
                return database.InsertAsync(obj);
            }
        }

        public Task<int> DeleteIssueAsync(Issue obj)
        {
            // Delete a Issue.
            return database.DeleteAsync(obj);
        }


        public Task<List<IssueCategory>> GetIssueCategoryAsync()
        {
            //Get all notes.
            return database.Table<IssueCategory>().ToListAsync();
        }

        public Task<IssueCategory> GetIssueCategoryAsync(int id)
        {
            // Get a specific note.
            return database.Table<IssueCategory>()
                            .Where(i => i.ID == id)
                            .FirstOrDefaultAsync();
        }

        public Task<int> SaveIssueCategoryAsync(IssueCategory obj)
        {
            if (obj.ID != 0)
            {
                // Update an existing IssueCategory.
                return database.UpdateAsync(obj);
            }
            else
            {
                // Save a new IssueCategory.
                return database.InsertAsync(obj);
            }
        }

        public Task<int> DeleteIssueCategoryAsync(IssueCategory obj)
        {
            // Delete a IssueCategory.
            return database.DeleteAsync(obj);
        }


        public Task<List<IssueImage>> GetIssueImageAsync()
        {
            //Get all notes.
            return database.Table<IssueImage>().ToListAsync();
        }

        public Task<IssueImage> GetIssueImageAsync(int id)
        {
            // Get a specific note.
            return database.Table<IssueImage>()
                            .Where(i => i.ID == id)
                            .FirstOrDefaultAsync();
        }

        public Task<int> SaveIssueImageAsync(IssueImage obj, bool issueImageExisit)
        {
            //try {
            //    return database.InsertAsync(obj);
            //} catch {
            //    return database.UpdateAsync(obj);
            //}


            //if (obj.ID != 0)
            if (issueImageExisit)
            {
                // Update an existing IssueImage.
                return database.UpdateAsync(obj);
            }
            else
            {
                // Save a new IssueImage.
                return database.InsertAsync(obj);
            }
        }

        public Task<int> DeleteIssueImageAsync(IssueImage obj)
        {
            // Delete a IssueImage.
            return database.DeleteAsync(obj);
        }


        public Task<List<IssueType>> GetIssueTypeAsync()
        {
            //Get all notes.
            return database.Table<IssueType>().ToListAsync();
        }

        public Task<IssueType> GetIssueTypeAsync(int id)
        {
            // Get a specific note.
            return database.Table<IssueType>()
                            .Where(i => i.ID == id)
                            .FirstOrDefaultAsync();
        }

        public Task<int> SaveIssueTypeAsync(IssueType obj)
        {
            if (obj.ID != 0)
            {
                // Update an existing IssueType.
                return database.UpdateAsync(obj);
            }
            else
            {
                // Save a new IssueType.
                return database.InsertAsync(obj);
            }
        }

        public Task<int> DeleteIssueTypeAsync(IssueType obj)
        {
            // Delete a IssueType.
            return database.DeleteAsync(obj);
        }




        public Task<List<PedestalInventory>> GetPedestalInventoryAsync()
        {
            //Get all notes.
            return database.Table<PedestalInventory>().ToListAsync();
        }

        public Task<PedestalInventory> GetPedestalInventoryAsync(int id)
        {
            // Get a specific note.
            return database.Table<PedestalInventory>()
                            .Where(i => i.ID == id)
                            .FirstOrDefaultAsync();
        }

        public Task<int> SavePedestalInventoryAsync(PedestalInventory obj)
        {
            if (obj.ID != 0)
            {
                // Update an existing PedestalInventory.
                return database.UpdateAsync(obj);
            }
            else
            {
                // Save a new PedestalInventory.
                return database.InsertAsync(obj);
            }
        }

        public Task<int> DeletePedestalInventoryAsync(PedestalInventory obj)
        {
            // Delete a PedestalInventory.
            return database.DeleteAsync(obj);
        }



        public Task<List<StoreArea>> GetStoreAreaAsync()
        {
            //Get all notes.
            return database.Table<StoreArea>().ToListAsync();
        }

        public Task<StoreArea> GetStoreAreaAsync(int id)
        {
            // Get a specific note.
            return database.Table<StoreArea>()
                            .Where(i => i.ID == id)
                            .FirstOrDefaultAsync();
        }

        public Task<int> SaveStoreAreaAsync(StoreArea obj)
        {
            if (obj.ID != 0)
            {
                // Update an existing StoreArea.
                return database.UpdateAsync(obj);
            }
            else
            {
                // Save a new StoreArea.
                return database.InsertAsync(obj);
            }
        }

        public Task<int> DeleteStoreAreaAsync(StoreArea obj)
        {
            // Delete a StoreArea.
            return database.DeleteAsync(obj);
        }

        public Task<List<StoreType>> GetStoreTypeAsync()
        {
            //Get all notes.
            return database.Table<StoreType>().ToListAsync();
        }

        public Task<StoreType> GetStoreTypeAsync(int id)
        {
            // Get a specific note.
            return database.Table<StoreType>()
                            .Where(i => i.ID == id)
                            .FirstOrDefaultAsync();
        }

        public Task<int> SaveStoreTypeAsync(StoreType obj)
        {
            if (obj.ID != 0)
            {
                // Update an existing StoreType.
                return database.UpdateAsync(obj);
            }
            else
            {
                // Save a new StoreType.
                return database.InsertAsync(obj);
            }
        }

        public Task<int> DeleteStoreTypeAsync(StoreType obj)
        {
            // Delete a StoreType.
            return database.DeleteAsync(obj);
        }


        public Task<List<SystemType>> GetSystemTypeAsync()
        {
            //Get all notes.
            return database.Table<SystemType>().ToListAsync();
        }

        public Task<SystemType> GetSystemTypeAsync(int id)
        {
            // Get a specific note.
            return database.Table<SystemType>()
                            .Where(i => i.ID == id)
                            .FirstOrDefaultAsync();
        }

        public Task<int> SaveSystemTypeAsync(SystemType obj)
        {
            if (obj.ID != 0)
            {
                // Update an existing SystemType.
                return database.UpdateAsync(obj);
            }
            else
            {
                // Save a new SystemType.
                return database.InsertAsync(obj);
            }
        }

        public Task<int> DeleteSystemTypeAsync(SystemType obj)
        {
            // Delete a SystemType.
            return database.DeleteAsync(obj);
        }

        public Task<List<TableSyncStatus>> GetTableSyncStatusAsync()
        {
            //Get all notes.
            return database.Table<TableSyncStatus>().ToListAsync();
        }

        public Task<TableSyncStatus> GetTableSyncStatusAsync(int id)
        {
            // Get a specific note.
            return database.Table<TableSyncStatus>()
                            .Where(i => i.ID == id)
                            .FirstOrDefaultAsync();
        }

        public Task<int> SaveTableSyncStatusAsync(TableSyncStatus obj)
        {
            if (obj.ID != 0)
            {
                // Update an existing TableSyncStatus.
                return database.UpdateAsync(obj);
            }
            else
            {
                // Save a new TableSyncStatus.
                return database.InsertAsync(obj);
            }
        }

        public Task<int> DeleteTableSyncStatusAsync(TableSyncStatus obj)
        {
            // Delete a TableSyncStatus.
            return database.DeleteAsync(obj);
        }


    }
}

