using Palladium.Models;
using System;
using System.IO;
using System.Xml;

namespace Palladium.Controllers
{
    public class WriteXML
    {
     
        public string writeXML(MakePaymentRequestModel makePaymentRequestModel)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = ("    ");
            settings.CloseOutput = true;

            using (StringWriter str = new StringWriter())
            {
                using (XmlTextWriter writer = new XmlTextWriter(str))
                {
                    // Root element - start tag  
                    writer.WriteStartElement("receipts");

                    writer.WriteStartElement("receipt");
 
                    writer.WriteElementString("custNumber", makePaymentRequestModel.CUSTOMERNO);

                    writer.WriteElementString("docNo", makePaymentRequestModel.docNo); //RC-

                    writer.WriteElementString("docDate", makePaymentRequestModel.docDate); // 2020-02-29T00:00:00

                    writer.WriteElementString("tenderType", "Cash");

                    writer.WriteElementString("intAccount", "");

                    writer.WriteElementString("comment", makePaymentRequestModel.BillRefNumber);

                    writer.WriteElementString("department", "");

                    writer.WriteElementString("exchangeRate", "");

                    writer.WriteElementString("receiptAmount", Convert.ToString(makePaymentRequestModel.TransAmount));

                    writer.WriteElementString("deposit", "0");

                    writer.WriteElementString("total", (Convert.ToString(makePaymentRequestModel.TransAmount)));

                    writer.WriteStartElement("lines");

                    writer.WriteStartElement("line");

                    writer.WriteElementString("invoiceDocNo", makePaymentRequestModel.InvoiceDocNo);

                    writer.WriteElementString("discountTaken", "0.0000");

                    writer.WriteElementString("amount", Convert.ToString(makePaymentRequestModel.TransAmount));

                    writer.WriteEndElement();

                    writer.WriteEndElement();

                    // Root element - end tag  
                    writer.WriteEndElement();

                    writer.WriteEndElement();

                    // Result is a string.
                    string xmlString = str.ToString();

                    return xmlString;
                }
            }
        }
    }
}
