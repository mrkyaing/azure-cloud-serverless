using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProDev.HRMS.Func.Models
{
    public class OrganizationModel:TableEntity
    {
        public string Extension { get; set; }
        public string Parent { get; set; }
    }
}
