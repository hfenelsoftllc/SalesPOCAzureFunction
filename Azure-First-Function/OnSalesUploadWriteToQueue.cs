using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using azf_sales_endpoint.Models;

namespace Azure_First_Function
{
    public static class OnSalesUploadWriteToQueue
    {
        [FunctionName("OnSalesUploadWriteToQueue")]        
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            [Queue("SalesRequestInbound", Connection = "AzureWebJobsStorage")] IAsyncCollector<SalesRequest> salesRequestQueue,
            ILogger log)
        {
            log.LogInformation("Sales Request received by OnSalesUploadWriteToQueue function");

            // Deserialized the SaleRequest Data receive on the body          
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            SalesRequest data = JsonConvert.DeserializeObject<SalesRequest>(requestBody);

            // Add the sales information to a Queue in Azure
            await salesRequestQueue.AddAsync(data);
            string responseMessage = "Sales Request has been received for: " + data;

            //string responseMessage = string.IsNullOrEmpty(data)
            //    ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
            //    : $"Hello, {data}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);
        }
    }
}
