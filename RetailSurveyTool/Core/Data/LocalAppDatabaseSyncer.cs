using Jci.RetailSurveyTool.TechnicianApp.Services;
using JCI.RetailSurveyTool.DataBase.Models;
using Microsoft.AppCenter.Analytics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Jci.RetailSurveyTool.TechnicianApp.Data
{
    public partial class LocalAppDatabase
    {
        private static void doNothing(int valueIn)
        {

        }
        const bool forceAllDefault =
#if DEBUG
            true;
#else
            false;
#endif
        public async Task SyncEntities(bool forceAll, Action<int> tablesToSync = null, Action<int> tablesSynced = null)
        {
            if (tablesToSync == null)
            {
                tablesToSync = doNothing;
            }
            if (tablesSynced == null)
            {
                tablesSynced = doNothing;
            }
            int tablesSyncedInt = 0;
            tablesToSync.Invoke(EntitySyncMethods.Count(x => x.Value != SyncMethod.LOCALONLY));

            var lastSyncStatuses = await GetTableSyncStatusAsync();

            foreach (var type in EntitySyncMethods.Where(x => x.Value == SyncMethod.FETCH24).Select(x => x.Key))
            {
                if (forceAll || !lastSyncStatuses.Any(x => x.Table == type.Name && x.LastSync >= (DateTime.UtcNow - TimeSpan.FromDays(1))))
                {
                    //Do Sync
                    var syncerType = typeof(Syncer<>).MakeGenericType(type);
                    var Syncer = Activator.CreateInstance(syncerType, restService, this);
                    await (syncerType.GetMethod("DoSyncByPullAndReplace").Invoke(Syncer, null) as Task);

                    //Write to local db when we did sync
                    var lastSyncStatus = lastSyncStatuses.FirstOrDefault(x => x.Table == type.Name);
                    if (lastSyncStatus == null)
                    {
                        lastSyncStatus = new TableSyncStatus()
                        {
                            Table = type.Name,
                            //ID = 0
                        };
                    }
                    lastSyncStatus.LastSync = DateTime.UtcNow;
                    await SaveTableSyncStatusAsync(lastSyncStatus);
                }
                //Update UI with sync progress
                tablesSyncedInt++;
                tablesSynced.Invoke(tablesSyncedInt);
            }
            //PUSH EVERYTIME BLOCK (Handles Audit, DeactivationInventory, PedestalInventory, IssueImage)
            foreach (Audit audit in await database.Table<Audit>().Where(x => x.Status == "Completed").ToListAsync())
            {

                await LoadRelatedRecords(audit);
                addModifer(audit);

                if (audit.ID < 0)
                {
                    var returnedAudit = await restService.PostItem<Audit>(GenerateJSONWithNoIDs(audit));
                    returnedAudit.Status = "Completed - Finalized";
                    await DeleteWithRelatedRecords(audit);
                    await AddWithRelatedRecords(returnedAudit);
                    sendAuditEvent(audit);
                }
                else
                {

                    var responseAudit = await restService.PutAudit<Audit>(GenerateJSONWithNoIDs(audit), audit.ID.ToString());
                    responseAudit.Status = "Completed - Finalized";

                    try
                    {
                        await database.Table<SVMX_WO>().DeleteAsync(x => x.WORK_ORDER_ID == responseAudit.BobWOId);

                    }catch(Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }

                    //await DeleteWithRelatedRecords(responseAudit);
                    await AddWithRelatedRecords(responseAudit);
                }
                tablesSyncedInt += 4;
                tablesSynced.Invoke(tablesSyncedInt);
            }


            //SVMX WO Worker
            //var remoteWOs = await restService.GetWOsAsync(); //All Open work orders

            var remoteWOs = await restService.GetWOByTechAsync(App.objUserModel.Mail.ToString());
            var allLocalWOs = await database.Table<SVMX_WO>().ToListAsync();

            //Delete Locals That No Longer Are Coming From Remote
            foreach (var wo in allLocalWOs.Where(x => !remoteWOs.Any(y => y.WORK_ORDER_ID == x.WORK_ORDER_ID)))
            {
                await database.Table<SVMX_WO>().DeleteAsync(x => x.WORK_ORDER_ID == wo.WORK_ORDER_ID);
            }
            //Add New Remotes
            foreach (var wo in remoteWOs.Where(x => !allLocalWOs.Any(y => y.WORK_ORDER_ID == x.WORK_ORDER_ID)))
            {
                wo.Consumed = false;
                await database.InsertOrReplaceAsync(wo);
            }
            tablesSyncedInt++;
            tablesSynced.Invoke(tablesSyncedInt);


        }

        private void addModifer(Audit audit)
        {
            audit.TechnicianName = App.objUserModel.DisplayName;
            audit.TechnicianEmail = App.objUserModel.Mail;
            foreach (var issue in audit.Issues)
            {
                issue.LastModified = DateTime.UtcNow;
                issue.LastModifiedBy = App.objUserModel.Mail;
            }
        }

        private void sendAuditEvent(Audit audit)
        {
            var properties = new Dictionary<string, string> {
                { "WorkOrder", audit.ServiceCallNumber},
                { "Completed By", audit.TechnicianName },
                { "Email", audit.TechnicianEmail } 
            };

            Analytics.TrackEvent("Audit Completed", properties);

        }

        private string GenerateJSONWithNoIDs(Audit audit)
        {

            var content = JsonConvert.SerializeObject(audit, new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Auto
            });

            var jObj = JObject.Parse(content);
            if (audit.ID < 0)
            {
                jObj.Remove("ID");
            }

            foreach (JObject issue in ((JArray)jObj["Issues"]))
            {
                var issueId = Int32.Parse(issue.GetValue("ID").ToString());
                if (issueId < 0)
                {
                    issue.Remove("ID");
                }
                foreach (JObject img in ((JArray)issue["IssueImages"]))
                {
                    var imageId = Int32.Parse(img.GetValue("ID").ToString());
                    if (issueId < 0)
                    {
                        img.Remove("ID");
                    }
                }
            }
            foreach (JObject inv in ((JArray)jObj["Inventories"]))
            {
                var invID = Int32.Parse(inv.GetValue("ID").ToString());
                if (invID < 0)
                {
                    inv.Remove("ID");
                }

            }
            return jObj.ToString();
        }



        private async Task LoadRelatedRecords(Audit audit)
        {
            audit.Inventories = new List<Inventory>();
            audit.Inventories.AddRange(await database.Table<DeactivationInventory>().Where(x => x.AuditID == audit.ID).ToListAsync());
            audit.Inventories.AddRange(await database.Table<PedestalInventory>().Where(x => x.AuditID == audit.ID).ToListAsync());
            audit.Issues = await database.Table<Issue>().Where(x => x.AuditID == audit.ID).ToListAsync();
            foreach (var issue in audit.Issues)
            {
                issue.IssueImages = await database.Table<IssueImage>().Where(x => x.IssueID == issue.ID).ToListAsync();
            }
        }
        private async Task DeleteWithRelatedRecords(Audit audit)
        {
            await database.Table<DeactivationInventory>().DeleteAsync(x => x.AuditID == audit.ID);
            await database.Table<PedestalInventory>().DeleteAsync(x => x.AuditID == audit.ID);
            var issueIDs = (await database.Table<Issue>().Where(x => x.AuditID == audit.ID).ToListAsync()).Select(x => x.ID).ToList();
            await database.Table<IssueImage>().DeleteAsync(x => issueIDs.Contains(x.IssueID));
            await database.Table<Issue>().DeleteAsync(x => x.AuditID == audit.ID);
            await database.Table<Audit>().DeleteAsync(x => x.ID == audit.ID);
        }
        private async Task AddWithRelatedRecords(Audit audit)
        {

            await database.Table<DeactivationInventory>().Where(x => x.ID == x.ID).DeleteAsync();
            await database.Table<PedestalInventory>().Where(x => x.ID == x.ID).DeleteAsync();
            await database.Table<IssueImage>().Where(x => x.ID == x.ID).DeleteAsync();
            await database.Table<Issue>().Where(x => x.ID == x.ID).DeleteAsync();
            await database.Table<Audit>().Where(x => x.ID == x.ID).DeleteAsync();

            await database.InsertOrReplaceAsync(audit);


            foreach (var inv in audit.Inventories)
            {
                await database.InsertAsync(inv);
            }


            foreach (var issue in audit.Issues)
            {
                await database.InsertAsync(issue);
                foreach (var img in issue.IssueImages)
                {
                    await database.InsertAsync(img);
                }
            }

        }

        private class Syncer<T> where T : class
        {
            private readonly RestService restService;
            private readonly LocalAppDatabase db;

            public Syncer(RestService restService, LocalAppDatabase db)
            {
                this.restService = restService;
                this.db = db;
            }
            public async Task DoSyncByPullAndReplace()
            {
                var items = await restService.GetItems<T>();
                await db.GetRawConnection().DeleteAllAsync<T>();
                foreach (var item in items)
                {

                    //Update and add from Cloud DB
                    try
                    {
#if DEBUG
                        Debug.WriteLine(JsonConvert.SerializeObject(item));
#endif
                        await db.GetRawConnection().InsertOrReplaceAsync(item);
                    }
                    catch (Exception e)
                    {
                        
                        Debug.WriteLine(e.Message);
                    }
                }
            }
        }



        public enum SyncMethod
        {
            FETCH24,
            LOCALONLY,
            PUSHEVERYTIME,
            FETCHNOREPLACE,
        }
        public Dictionary<Type, SyncMethod> EntitySyncMethods = new Dictionary<Type, SyncMethod>()
        {
                {typeof(CustomerIssueTypeMap), SyncMethod.FETCH24},
                {typeof(IssueTypeIssueCategoryMap), SyncMethod.FETCH24},
                {typeof(CustomerStoreMap), SyncMethod.FETCH24},
                {typeof(StoreTypeAreaMap), SyncMethod.FETCH24},
                {typeof(StoreAreaDeactivatorTypeMap), SyncMethod.FETCH24},
                {typeof(AlarmTone), SyncMethod.FETCH24},
                {typeof(Audit), SyncMethod.PUSHEVERYTIME},
                {typeof(AuditType),SyncMethod.FETCH24},
                {typeof(Customer), SyncMethod.FETCH24},
                {typeof(CustomerErpMapping), SyncMethod.FETCH24},
                {typeof(CustomerPreferredDeactivator),SyncMethod.FETCH24},
                {typeof(CustomerPreferredSystem),SyncMethod.FETCH24},
                {typeof(DeactivationInventory),SyncMethod.PUSHEVERYTIME},
                {typeof(DeactivatorType),SyncMethod.FETCH24},
                {typeof(Icon),SyncMethod.FETCH24},
                {typeof(Issue),SyncMethod.PUSHEVERYTIME},
                {typeof(IssueCategory),SyncMethod.FETCH24},
                {typeof(IssueImage),SyncMethod.PUSHEVERYTIME},
                {typeof(IssueType),SyncMethod.FETCH24},
                {typeof(PedestalInventory),SyncMethod.PUSHEVERYTIME},
                {typeof(StoreArea),SyncMethod.FETCH24},
                {typeof(StoreType),SyncMethod.FETCH24},
                {typeof(SystemType),SyncMethod.FETCH24},
                {typeof(TableSyncStatus),SyncMethod.LOCALONLY},
                {typeof(SVMX_WO),SyncMethod.FETCHNOREPLACE},
        };
    }
}
