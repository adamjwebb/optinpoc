using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace OptinPoC
{

    public class OptinPermission
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("pnoptinstatus")]
        public string Pnoptinstatus { get; set; }
    }
    public static class GetOptinPermission
    {
        [FunctionName("GetOptinPermission")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]
                HttpRequest req,
            [CosmosDB(
                databaseName: "poc",
        collectionName: "optin",
        ConnectionStringSetting = "CosmosDbConnectionString",
                Id = "{Query.subscriberkey}",
                PartitionKey = "{Query.subscriberkey}")] OptinPermission guest,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            if (guest == null)
            {
                log.LogInformation("Guest not found");
            }
            else
            {
                log.LogInformation(guest.Id);
            }
            return new OkObjectResult(guest);
        }
    }
}