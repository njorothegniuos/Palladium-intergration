using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PalladiumAPI.Models
{
    public class SalesOrder
    {
        public string Reference { get; set; }
        public string invoiceNo { get; set; }
        public dynamic Content { get; set; }
    }
}