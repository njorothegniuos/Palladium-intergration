using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PalladiumAPI.Models
{
    public class ReceiptResponse
    {
        [JsonProperty("invoiceDocNo")]
        public string InvoiceDocNo { get; set; }

        [JsonProperty("ReceipRef")]
        public string ReceipRef { get; set; }

        [JsonProperty("Status")]
        public string Status { get; set; }

        [JsonProperty("ErrorMessage")]
        public string ErrorMessage { get; set; }

    }
}