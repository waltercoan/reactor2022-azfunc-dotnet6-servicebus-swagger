using System;
using System.Collections.Generic;
using Azure;
using Microsoft.WindowsAzure.Storage;
using Azure.Data.Tables;

namespace azfunc.Model;
public class VoteRegistryEntity: VoteRegistry, ITableEntity
{
    public string PartitionKey { get; set; }
    public string RowKey { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
}