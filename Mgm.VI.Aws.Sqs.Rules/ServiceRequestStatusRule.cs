//////////////////////////////////////////////////////////////////
///
/// Engineer:   Nour Douchi
/// Company:    MGM
/// Project:    MGM-Vubiquity Asset Integration
/// Revision:   01/15/2020 
/// 
//////////////////////////////////////////////////////////////////

using Amazon.SQS;
using Amazon.SQS.Model;
using Mgm.VI.Common;
using Mgm.VI.Data.Dto;
using Mgm.VI.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace Mgm.VI.Aws.Sqs.Rules
{
    public class ServiceRequestStatusRule : Rule //SqsRule_ValidateXsd
    {
        private IServiceRequestStatusUnitOfWork _srsUow;

        public XDocument Xdocument { get; set; }
        public XmlSchemaSet XmlSchemas { get; set; }
        public List<IServiceRequestStatusDto> ServiceRequestStatusDtos { get; set; }
        public List<IServiceRequestDto> ServiceRequestDtos { get; set; }
        public List<IOrderDto> InvalidOrders { get; set; }
        public List<string> AcceptableStatuses {get; set;}
        public override bool IsMet
        {
            get
            {
                Validate();
                return __isMet;
            }
        }
        //public ServiceRequestStatusRule(XmlSchemaSet xmlSchemas) : base(xmlSchemas) {}
        //public ServiceRequestStatusRule(string xsdDocPath) : base(xsdDocPath) { }
        //public ServiceRequestStatusRule(XDocument xDoc, string xsdDocPath) : base(xDoc, xsdDocPath) { }
        //public ServiceRequestStatusRule(XDocument xDoc, XmlSchemaSet xmlSchemas) : base(xDoc, xmlSchemas) {}
        //public ServiceRequestStatusRule(XDocument xDoc, XmlSchema xmlSchema) : base(xDoc, xmlSchema) {}

        public ServiceRequestStatusRule(string xsdDocPath, IServiceRequestStatusUnitOfWork srsUow) : this(xsdDocPath, srsUow, null) { }
        public ServiceRequestStatusRule(string xsdDocPath, IServiceRequestStatusUnitOfWork srsUow, string messageBody) : base(messageBody)
        {
            _srsUow = srsUow;
            XmlSchemas = XsdProcessor.GenerateSchemaFromDocument(xsdDocPath);
            Xdocument = XmlProcessor.LoadContent(messageBody);
            Content = messageBody;
            ServiceRequestDtos = ServiceRequestDto.Parse(Xdocument, xsdDocPath, ServiceRequestDto.XmlFieldName);
        }
        private void LoadAcceptableStatuses()
        {
            if (AcceptableStatuses == null) AcceptableStatuses = new List<string>();

            ServiceRequestStatusDtos = _srsUow.GetAllStatuses();
            ServiceRequestStatusDtos.ForEach(srsDto =>
                {
                    AcceptableStatuses.Add(srsDto.MasterDataName);
                }
            );

            //AcceptableStatuses.Add("Not Required");
            //AcceptableStatuses.Add("Researching");
            //AcceptableStatuses.Add("PO Issued");
            //AcceptableStatuses.Add("Vendor Confirmed");
            //AcceptableStatuses.Add("In Progress");
            //AcceptableStatuses.Add("In Progress Pending EOP");
            //AcceptableStatuses.Add("Completed");
            //AcceptableStatuses.Add("PPS Assigned");
            //AcceptableStatuses.Add("Rejected");
        }
        private void Validate()
        {
            if (Content == null)
                AddToReasonsNotMet("ServiceRequestStatusRule does not have a valid message.  Content object is null.");
            else
                ValidateStatus();
        }
        private void ValidateStatus()
        {
            LoadAcceptableStatuses();
            InvalidOrders = ExtractInvalidOrders();
            if (InvalidOrders != null && InvalidOrders.Count > 0) __isMet = false;
        }
        private List<IOrderDto> ExtractInvalidOrders()
        {
            if (ServiceRequestDtos == null) return null;

            var invalidOrders = new List<IOrderDto>();
            ServiceRequestDtos.ForEach(srDto =>
            {
                srDto.Contracts.ForEach(contract =>
                {
                    contract.Titles.ForEach(title =>
                    {
                        title.LineItems.ForEach(lineItem =>
                        {
                            lineItem.Orders.ForEach(order =>
                            {
                                if (AcceptableStatuses.Contains(order.PPSStatus.Trim()))
                                    invalidOrders.Add(order);
                            });
                        });
                    });
                });
            });
            return invalidOrders;
        }
    }
}
