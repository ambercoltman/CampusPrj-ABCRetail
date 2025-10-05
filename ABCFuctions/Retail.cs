using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure;
using Azure.Data.Tables;

namespace ABCFuctions
{
    internal class Retail : ITableEntity
    {
        public string? custName { get; set; }
        public string? surname { get; set; }
        public string? email { get; set; }
        public string? phoneNumber { get; set; }
        public string? address { get; set; }

        public string? prodName { get; set; }
        public string? Description { get; set; }
        public string? Price { get; set; }
        public string? Availability { get; set; }
        public string? Image { get; set; }

        public string PartitionKey { get; set; } = "ABC";

        public string RowKey { get; set; } = string.Empty;

        //required by the ITableEntity interface
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
}
