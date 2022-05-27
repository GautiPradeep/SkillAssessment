using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Cosmos;
using SkillAssessment.Common;
using SkillAssessment.Models;

namespace AzureFunctions_Employee.Employeedetails
{
    public static class GetEmployeeByName
    {
        [FunctionName("GetEmployeeByName")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "GetEmployeeByName/{EmployeePhoneNumber}/{partitionkey}")] HttpRequest req,
            string EmployeeName, int partitionkey,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function - GetEmployeeByName.");

            ItemResponse<Employee> objEmployeeResponse = null;
            log.LogInformation("Calling Azure Function -- GetEmployeeByName");
            // initialising Azure Cosomosdb database connection.
            AzureCosmoDBActivity objCosmosDBActivitiy = new AzureCosmoDBActivity();
            await objCosmosDBActivitiy.InitiateConnection();
            // retriving existing employee information based on employee name and partition key i.e. employeeId value
            objEmployeeResponse = await objCosmosDBActivitiy.GetEmployeeItem(EmployeeName, partitionkey);
            return new OkObjectResult(objEmployeeResponse.Resource);
        }
    }
}
    
