using System.Collections.Generic;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace OptinPoC
{

    public class ReadOptinPermissionChanges
    {

        [FunctionName("ReadOptinPermissionChanges")]

        public static async Task Run([CosmosDBTrigger(
            databaseName: "poc",
            collectionName: "optin",
            ConnectionStringSetting = "CosmosDbConnectionString",
            LeaseCollectionName = "leases",
            CreateLeaseCollectionIfNotExists = true)] IReadOnlyList<Document> input, ILogger log,
 [ServiceBus("optinpermissionchanges", Connection = "ServiceBusConnectionString")] IAsyncCollector<dynamic> serviceBusQueue)
        {

            if (input != null && input.Count > 0)
            {
                foreach (Document doc in input)
                {
                    log.LogInformation(doc.ToString());
                    var message = doc;
                    await serviceBusQueue.AddAsync(message);
                }
            }
        }
    }
}