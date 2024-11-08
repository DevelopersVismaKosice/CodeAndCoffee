using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace Common.Models
{
    public class Result : TableEntity
    {
        public int Sum { get; set; }

        public Result()
        { }

        public Result(string requestId, int sum)
        {
            PartitionKey = "default";
            Timestamp = DateTimeOffset.UtcNow;
            
            RowKey = requestId;
            Sum = sum;
        }
    }
}
