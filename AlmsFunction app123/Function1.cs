using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Security.Policy;

namespace AlmsFunction_app123
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            [Sql("dbo.LeaveData", "Connection")] IAsyncCollector<Leaves> leave,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            Leaves data = JsonConvert.DeserializeObject<Leaves>(requestBody);
            //name = name ?? data?.name;

            await leave.AddAsync(data);
            await leave.FlushAsync();
           

            return new OkObjectResult("Leave data");
        }
    }
}
