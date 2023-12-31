using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Attendence_And_Leave_Final.Model;

namespace FunctionApp_ALMS123
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            [Sql("dbo.LeaveData","Connection")] IAsyncCollector<Leaves>leaves,
            ILogger log)
        {
            //log.LogInformation("C# HTTP trigger function processed a request.");

           // string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            Leaves data = JsonConvert.DeserializeObject<Leaves>(requestBody);
            await leaves.AddAsync(data);
            await leaves.FlushAsync();

            //string responseMessage = string.IsNullOrEmpty(name)
            //  ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
            //: $"Hello, {name}. This HTTP triggered function executed successfully.";

            return new OkObjectResult("Leave Accpetd");
        }
    }
}
