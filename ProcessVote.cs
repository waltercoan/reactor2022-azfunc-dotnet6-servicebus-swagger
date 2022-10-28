using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using azfunc.Model;
using Azure;
using Azure.Data.Tables;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace azfunc
{
    public class ProcessVote
    {
        [FunctionName("ProcessVote")]
        /*[return: Table("VoteTable", Connection = "StorageConnectionTables")]*/
        public async Task Run(
            [ServiceBusTrigger("votequeue", Connection = "ServiceBusConnection")]string myQueueItem,
            [Table("VoteTable", Connection = "StorageConnectionTables")] TableClient tableClient,
            ILogger log)
        {
            log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
            
            Vote vote = JsonSerializer.Deserialize<Vote>(myQueueItem);
            
            var music = MusicData.GetMusicData().FirstOrDefault(m => m.id == vote.musicId);
            
            if(music == null)
            {
                throw new Exception("Invalid music id");
            }
            AsyncPageable<VoteRegistryEntity> queryResults = 
                tableClient.QueryAsync<VoteRegistryEntity>(filter: $"(PartitionKey eq '{music.id}') and (RowKey eq '{music.id}')");

            IAsyncEnumerator<VoteRegistryEntity> enumerator = queryResults.GetAsyncEnumerator();
            VoteRegistryEntity voteRegistry;
            if(await enumerator.MoveNextAsync()){
                voteRegistry = enumerator.Current;
                voteRegistry.Quantity++;
            }
            else
            {
                voteRegistry = new VoteRegistryEntity()
                {
                    PartitionKey = $"{music.id}",
                    RowKey = $"{music.id}",
                    MusicName = music.name,
                    Quantity = 1
                };
            }

            await tableClient.UpsertEntityAsync<VoteRegistryEntity>(voteRegistry);
        }
    }
    
}
