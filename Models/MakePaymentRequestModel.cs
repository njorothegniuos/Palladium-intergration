using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Palladium.Models
{
    public class MakePaymentRequestModel
    {

        [JsonProperty("CUSTOMERNO")]
        public string CUSTOMERNO { get; set; }

        [JsonProperty("TransID")]
        public string TransID { get; set; }

        [JsonProperty("TransAmount")]
        public decimal TransAmount { get; set; }

        [JsonProperty("BillRefNumber")]
        public string BillRefNumber { get; set; }

        [JsonProperty("invoiceDocNo")]
        public string InvoiceDocNo { get; set; }

        [JsonProperty("ReceipRef")]
        public string ReceipRef { get; set; }

        [JsonProperty("docNo")]
        public string docNo { get; set; }

        [JsonProperty("docDate")]
        public string docDate { get; set; }

    }
}
