
using System.Net;
using Newtonsoft.Json;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using ProDev.HRMS.Func.AzureTableStorageServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProDev.HRMS.Func.AzureTableStorageServices
{
    public class AzureTableStorageService : IAzureTableStorageService
    {
        CloudStorageAccount _storageAccount = null;
        public AzureTableStorageService()
        {
            // Connect to the Storage account.
            _storageAccount = CloudStorageAccount.Parse("XXX");
        }
        public async Task<T> CreateOrReplace<T>(ITableEntity TItem, string tableName)
        {
          var cloudTable= this.GetTableReference(tableName);
            TableOperation operation = TableOperation.InsertOrReplace(TItem);
           var item=await cloudTable.ExecuteAsync(operation);
            if (item != null) 
                return (T)item.Result;
            return 
                default(T);
        }

        public Task<T> DeleteItem<T>(string PartitionKe, string rowKey, string tableName)
        {
            throw new NotImplementedException();
        }

        public Task<List<T>> GetItems<T>(string tableName)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> CreateTable<CloudTable>(string tableName)
        {
            bool result = false;
            CloudTableClient client = _storageAccount.CreateCloudTableClient();
            var table = client.GetTableReference(tableName);
            if (table is null)
            {
                result= await table.CreateIfNotExistsAsync();
            }
            return result;
        }
        private  CloudTable GetTableReference(string tableName)
        {
            CloudTableClient client =_storageAccount.CreateCloudTableClient();
            var table = client.GetTableReference(tableName);
            return table;
        }
    }
}
