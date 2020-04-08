using Palladium;
using Palladium.Controllers;
using Palladium.Models;
using PalladiumAPI.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Xml;
using System.Xml.Linq;

namespace PalladiumAPI.Controllers
{
    public class PalladiumIntegrationController : ApiController
    {
        private Db db;

        public PalladiumIntegrationController()
        {

            db = new Db(Util.GetDbConnString());
        }

        [HttpPost]
        public async Task<ReceiptResponse> GetOrderReference(MakePaymentRequestModel makePaymentRequestModel)
        {
            ReceiptResponse receipt = new ReceiptResponse();

            MakePaymentRequestModel makePaymentRequestModel1 = new MakePaymentRequestModel();

            string InvoiceNumber = "";

            string docDate = "";

            string CUSTOMERNO = "";

            var oXDoc2 = await GetReceipt();

            //Reading XML
            XmlDocument xmltest = new XmlDocument();

            xmltest.LoadXml(oXDoc2);

            XmlNodeList dataNodes = xmltest.SelectNodes("//document");

            if (dataNodes.Count > 0)
            {
                foreach (XmlNode node in dataNodes)
                {
                    if (node != null)
                    {
                        var result = node["reference"].InnerText;

                        //GET INVOICE NUMBER
                        if (result == makePaymentRequestModel.BillRefNumber)
                        {

                            InvoiceNumber = node["docNo"].InnerText;

                            docDate = node["docDate"].InnerText;

                            CUSTOMERNO = node["custNumber"].InnerText;

                            //Get next receipt number
                            var num = db.GetNextReceiptNo();

                            makePaymentRequestModel1.BillRefNumber = makePaymentRequestModel.BillRefNumber;
                            makePaymentRequestModel1.CUSTOMERNO = CUSTOMERNO;
                            makePaymentRequestModel1.InvoiceDocNo = InvoiceNumber;
                            makePaymentRequestModel1.ReceipRef = makePaymentRequestModel.ReceipRef;
                            makePaymentRequestModel1.TransAmount = makePaymentRequestModel.TransAmount;
                            makePaymentRequestModel1.TransID = makePaymentRequestModel.TransID;
                            makePaymentRequestModel1.docDate = docDate;
                            makePaymentRequestModel1.docNo = num.Data1;

                            //Generate Receipt
                            WriteXML writeXML = new WriteXML();

                            string receipt_data = writeXML.writeXML(makePaymentRequestModel1);

                            //POST RECEIPT to PalladiumSoftware    

                            PalladiumSoftware.Integration.Documents documents = new PalladiumSoftware.Integration.Documents();

                            string responseXml = documents.ProcessReceipts("palladium2020", receipt_data);

                            //get response from palladium
                            //---- Process results
                            XDocument xml = XDocument.Parse(responseXml);

                            var myData = xml.Descendants().Where(x => x.Name.LocalName == "receipt").FirstOrDefault();

                            if (myData != null)
                            {
                                var status = (string)myData.Element("status");

                                var errorMessage = (string)myData.Element("errorMessage");

                                receipt.InvoiceDocNo = InvoiceNumber;

                                receipt.ReceipRef = makePaymentRequestModel1.ReceipRef;

                                receipt.Status = status;

                                receipt.ErrorMessage = errorMessage;

                            }

                            break;

                        }

                    }

                }
            }

            //get order

            var SalesOrder = await GetSalesOrder();

            xmltest.LoadXml(SalesOrder);

            XmlNodeList OrderdataNodes = xmltest.SelectNodes("//document");

            if (OrderdataNodes.Count > 0)
            {
                foreach (XmlNode Ordernode in OrderdataNodes)
                {
                    if (Ordernode != null)
                    {
                        var OrderReference = Ordernode["reference"].InnerText;

                        //GET INVOICE NUMBER
                        if (OrderReference == makePaymentRequestModel.BillRefNumber)
                        {
                            InvoiceNumber = Ordernode["docNo"].InnerText;

                            docDate = Ordernode["docDate"].InnerText;

                            CUSTOMERNO = Ordernode["custNumber"].InnerText;

                            //Get next receipt number
                            var num = db.GetNextReceiptNo();

                            makePaymentRequestModel1.BillRefNumber = makePaymentRequestModel.BillRefNumber;
                            makePaymentRequestModel1.CUSTOMERNO = CUSTOMERNO;
                            makePaymentRequestModel1.InvoiceDocNo = InvoiceNumber;
                            makePaymentRequestModel1.ReceipRef = makePaymentRequestModel.ReceipRef;
                            makePaymentRequestModel1.TransAmount = makePaymentRequestModel.TransAmount;
                            makePaymentRequestModel1.TransID = makePaymentRequestModel.TransID;
                            makePaymentRequestModel1.docDate = docDate;
                            makePaymentRequestModel1.docNo = num.Data1;

                            // generate receipt directly
                            //Generate Receipt
                            WriteXML writeXML = new WriteXML();

                            string receipt_data = writeXML.writeXML(makePaymentRequestModel1);

                            //POST RECEIPT to PalladiumSoftware    

                            PalladiumSoftware.Integration.Documents documents = new PalladiumSoftware.Integration.Documents();

                            string responseXml = documents.ProcessReceipts("palladium2020", receipt_data);

                            //get response from palladium
                            //---- Process results
                            XDocument xml = XDocument.Parse(responseXml);

                            var myData = xml.Descendants().Where(x => x.Name.LocalName == "receipt").FirstOrDefault();

                            if (myData != null)
                            {
                                var status = (string)myData.Element("status");

                                var errorMessage = (string)myData.Element("errorMessage");

                                receipt.InvoiceDocNo = InvoiceNumber;

                                receipt.ReceipRef = makePaymentRequestModel1.ReceipRef;

                                receipt.Status = status;

                                receipt.ErrorMessage = errorMessage;

                            }
                            break;
                        }
                    }
                }

            } 
            if(string.IsNullOrEmpty(receipt.Status))
            {
                return new ReceiptResponse
                {
                    InvoiceDocNo = "",
                    ReceipRef = "Success",
                    Status = "",
                    ErrorMessage = "Invoice with the reference " + makePaymentRequestModel.ReceipRef + " was not found!"
                };
            }

            return receipt;
        }

        public async Task<string> GetReceipt()
        {
            PalladiumSoftware.Integration.Enquiries enquiries = new PalladiumSoftware.Integration.Enquiries();

            DateTime enteredDate = DateTime.Parse("01/01/2020 11:26:43");

            string oXDoc2 = enquiries.GetSalesInvoices("palladium2020", enteredDate, DateTime.Now);

            return oXDoc2;
        }

        public async Task<string> GetSalesOrder()
        {
            PalladiumSoftware.Integration.Enquiries enquiries = new PalladiumSoftware.Integration.Enquiries();

            DateTime enteredDate = DateTime.Parse("01/01/2020 11:26:43");

            string oXDoc2 = enquiries.GetSalesOrders("palladium2020", enteredDate, DateTime.Now);

            return oXDoc2;
        }

    }
}