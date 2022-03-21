using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProDev.HRMS.Func.AzureTableStorageServices.Interfaces
{
    public interface IAzureTableStorageService
    {
        Task<bool> CreateTable<CloudTable>(string tableName);
        Task<T> CreateOrReplace<T>(ITableEntity TItem, string tableName);
        Task<List<T>> GetItems<T>(string tableName);
        Task<T> DeleteItem<T>(string PartitionKe,string rowKey, string tableName);
    }
}
