using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace OptinPoC
{
    public static class SetOptinPermission
    {
        [FunctionName("SetOptinPermission")]
        public static async Task<IActionResult> Run(
    [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
    [CosmosDB(
        databaseName: "poc",
        collectionName: "optin",
        ConnectionStringSetting = "CosmosDbConnectionString")]IAsyncCollector<dynamic> documentsOut,
    ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string id = req.Query["subscriberkey"];
            string email = req.Query["email"];
            string pnoptinstatus = req.Query["pnoptinstatus"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            if (!string.IsNullOrEmpty(email))
            {
                // Add a JSON document to the output container.
                await documentsOut.AddAsync(new
                {
                    id = id,
                    email = email,
                    pnoptinstatus = pnoptinstatus
                });
            }

            string responseMessage = "This HTTP triggered function executed successfully";

            return new OkObjectResult(responseMessage);
        }
    }
}
