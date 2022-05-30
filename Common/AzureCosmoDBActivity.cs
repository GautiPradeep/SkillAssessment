using SkillAssessment.Models;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillAssessment.Common
{
    public class AzureCosmoDBActivity
    {
        // The Azure Cosmos DB endpoint for running this sample.
        private static readonly string EndpointUri = "https://localhost:8081";
        // The primary key for the Azure Cosmos account.
        private static readonly string PrimaryKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";

        // The Cosmos client instance
        private CosmosClient cosmosClient;

        // The database we will be created
        private Database database;

        // The container we will create.
        private Container container;

        
        private string databaseId = "Employdetails";
        private string containerId = "EmployList";

        public List<Employee> EmployeeName { get; private set; }

        public async Task InitiateConnection()
        {
            
            this.cosmosClient = new CosmosClient(EndpointUri, PrimaryKey);
            await CreateDatabaseAsync();
            await CreateContainerAsync();
        }

        private async Task CreateDatabaseAsync()
        {
            
            this.database = await this.cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);
        }


        private async Task CreateContainerAsync()
        {
            
            this.container = await this.database.CreateContainerIfNotExistsAsync(containerId, "/Employdetails");
        }

        public async Task<ItemResponse<Employee>> SaveNewEmployeeItem(Employee objEmployee)
        {
            ItemResponse<Employee> employeeResponse = null;
            try
            {
                
                employeeResponse = await this.container.CreateItemAsync<Employee>(objEmployee, new PartitionKey(objEmployee.PhoneNumber));
            }
            catch (CosmosException ex)
            {
                throw ex;
            }
            return employeeResponse;
        }

        

        public async Task<ItemResponse<Employee>> GetEmployeeItem(string EmployeeName, int partitionKey)
        {
            ItemResponse<Employee> EmployeeResponse = null;
            try
            {
                EmployeeResponse = await this.container.ReadItemAsync<Employee>(EmployeeName, new PartitionKey(partitionKey));
            }
            catch (CosmosException ex)
            {
                throw ex;
            }
            return EmployeeResponse;
        }


        public async Task<List<Employee>> GetAllEmployees()
        {
            var sqlQueryText = "SELECT * FROM c";
            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
            FeedIterator<Employee> queryResultSetIterator = this.container.GetItemQueryIterator<Employee>(queryDefinition);

            List<Employee> lstEmployees = new List<Employee>();

            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<Employee> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                lstEmployees = currentResultSet.Select(r => new Employee()
                {
                    Name = r.Name,
                    DateofBirth = r.DateofBirth,
                    PhoneNumber = r.PhoneNumber,
                    Email = r.Email
                    
                    
                }).ToList();
               
            }
            return lstEmployees;
        }

    }
}
