using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Microsoft.Azure.Documents;
using Newtonsoft.Json;

namespace DUTAdmin.Models
{
    public class Student
    {
        [Key]
        [JsonProperty(PropertyName = "studentNo")]
        public string StudentNo { get; set; }

        [JsonProperty(PropertyName = "firstName")]
        public string FirstName { get; set; }

        [JsonProperty(PropertyName = "lastName")]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "homeAddress")]
        public string HomeAddress { get; set; }

        [JsonProperty(PropertyName = "mobile")]
        public string Mobile { get; set; }

        [JsonProperty(PropertyName = "studentPhoto")]
        public string StudentPhoto { get; set; }

        [JsonProperty(PropertyName = "isActive")]
        public bool IsActive { get; set; }

    }
}