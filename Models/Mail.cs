using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DUTAdmin.Models
{
    public class Mail
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}