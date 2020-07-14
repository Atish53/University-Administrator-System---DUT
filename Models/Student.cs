using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Microsoft.Azure.Documents;
using Newtonsoft.Json;

namespace DUTAdmin.Models
{
    public class Student
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "studentNo")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Please use numbers only")]
        [MaxLength(8)]
        [MinLength(8)]
        public string StudentNo { get; set; }

        [JsonProperty(PropertyName = "FirstName")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Please use letters only")]
        [StringLength(20)]
        public string FirstName { get; set; }

        [JsonProperty(PropertyName = "LastName")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Please use letters only")]
        [StringLength(20)]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "Email")]
        [StringLength(60)]
        [DataType(System.ComponentModel.DataAnnotations.DataType.EmailAddress)]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "HomeAddress")]
        [StringLength(100)]
        public string HomeAddress { get; set; }


        [JsonProperty(PropertyName = "Mobile")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Please use numbers only")]
        [MaxLength(10)]
        [MinLength(10)]
        public string Mobile { get; set; }

        [JsonProperty(PropertyName = "isActive")]
        [Display(Name = "IsActive")]
        public bool IsActive { get; set; }

        [JsonProperty(PropertyName = "PhotoPath")]
        [Display(Name = "Photo Path")]
        public string StudentPhoto { get; set; }

        [NotMapped]
        public string To { get; set; }
    }
}