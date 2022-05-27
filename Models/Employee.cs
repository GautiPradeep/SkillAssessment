using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkillAssessment.Models
{
    public class Employee
    {
        
        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("DateofBirth")]
        public string DateofBirth { get; set; }

        [JsonProperty("PhoneNumber")]
        public string PhoneNumber { get; set; }
        
        [JsonProperty("Email")]
        public string Email { get; set; }        

       
    }

    public class Employees
    {
        public Employees()
        {
            LstEmployee = new List<Employee>();
        }
        public List<Employee> LstEmployee { get; set; }
    }
}
