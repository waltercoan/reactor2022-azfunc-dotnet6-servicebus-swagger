using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using azfunc.Model;
using System.Text.Json;
using System.Linq;
using Azure.Data.Tables;
using Azure;
using System.Collections.Generic;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using System.Net;
using Microsoft.OpenApi.Models;

namespace azfunc
{
    public class Voting
    {
        [FunctionName("VoteOptions")]
        [OpenApiOperation(operationId: "VoteOptions", tags: new[] { "VoteOptions" })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Music), Description = "Lista de Músicas disponíveis para votação")]
        public async Task<IActionResult> VoteOptions(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("GET - Returning vote options");

            return new OkObjectResult(MusicData.GetMusicData());
        }
        [FunctionName("VoteResult")]
        [OpenApiOperation(operationId: "VoteResult", tags: new[] { "VoteResult" })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(VoteRegistry), Description = "Lista de resultados")]
        public async Task<IActionResult> VoteResult(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            [Table("VoteTable", Connection = "StorageConnectionTables")] TableClient tableClient,
            ILogger log)
        {
            log.LogInformation("GET - Returning vote options");
            AsyncPageable<VoteRegistryEntity> queryResults = tableClient.QueryAsync<VoteRegistryEntity>(filter: $"");

            List<VoteRegistry> voteRegistryList = new List<VoteRegistry>();
            await foreach (VoteRegistry vote in queryResults)
            {
                voteRegistryList.Add(new VoteRegistry(){
                    MusicName = vote.MusicName,
                    Quantity = vote.Quantity
                });
            }
            return new OkObjectResult(voteRegistryList);
        }

        [FunctionName("Vote")]
        [OpenApiOperation(operationId: "Vote", tags: new[] { "Vote" })]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(Vote), Description = "Voto", Required = true)]
        [return: ServiceBus("votequeue", Connection = "ServiceBusConnection")]
        public  async Task<string> Vote(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("POST - Storing vote in queue.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            Vote vote = JsonSerializer.Deserialize<Vote>(requestBody);
            vote.id = Guid.NewGuid().ToString();

            var music = MusicData.GetMusicData().FirstOrDefault(m => m.id == vote.musicId);
            if(music == null)
            {
                throw new Exception("Invalid music id");
            }

            return JsonSerializer.Serialize(vote);
        }
    }
}
