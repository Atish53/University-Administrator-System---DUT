using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Microsoft.Azure.Documents;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;


namespace DUTAdmin.ViewModels
{
    public class ViewModel
    {
        //Blob
        public string Name { get; set; }
        public string URI { get; set; }
        [Required(ErrorMessage = "Please select file.")]
        public HttpPostedFileBase FileUpload { get; set; }
        //Cosmos
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
        
        
    }
}