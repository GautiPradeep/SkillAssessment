using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SkillAssessment.Models;
using SkillAssessment.Common;
using System.Collections.Generic;

namespace SkillAssessment.Employeedetails
{
    public static class AllEmployees
    {
       

        

        [FunctionName("AllEmployees")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "AllEmployees")] HttpRequest req,
            ILogger log)
        {
            //declaring the response
            List<Employee> lstEmployees = null;
            log.LogInformation("Calling Azure Function -- EmployeeGetByPhoneNumber");
            // initialising Azure Cosomosdb database connection.
            AzureCosmoDBActivity objCosmosDBActivitiy = new AzureCosmoDBActivity();
            await objCosmosDBActivitiy.InitiateConnection();
            
            lstEmployees = await objCosmosDBActivitiy.GetAllEmployees();
            return new OkObjectResult(lstEmployees);
        }
    }
}

 