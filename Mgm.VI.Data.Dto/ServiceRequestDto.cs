using System;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml.Serialization;
using Mgm.VI.Common;
using System.Xml.Schema;

namespace Mgm.VI.Data.Dto
{
    public class ServiceRequestDto : DtoBase, IServiceRequestDto
    {
        public static readonly string XmlFieldName = "ServiceRequest";
        public static readonly string XmlGroupingFieldName = "ServiceRequests";

        #region Properties
        public string ID { get; set; }
        public string Description { get; set; }
        public string TransactionType { get; set; }
        public string ServicingStatus { get; set; }
        public string RushOrder { get; set; }
        public string DueDate { get; set; }
        public string BusinessPartnerID { get; set; }
        public string BusinessPartner { get; set; }
        public string ProfileID { get; set; }
        public string ProfileDescription { get; set; }
        public string FastTrack { get; set; }
        public string CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string CompletedDate { get; set; }
        public List<IContractDto> Contracts { get; set; }
        #endregion Properties

        private void Initialize(string id, string description, string transactionType, 
                                string servicingStatus, string rushOrder, string dueDate, 
                                string businessPartnerId, string businessPartner, string profileId, 
                                string profileDescription, string fastTrack, string createdDate, 
                                string createdBy, string completedDate, List<IContractDto> contracts)
        {
            try
            {
                #region Initialize Properties
                ID = id;
                Description = description;
                TransactionType = transactionType;
                ServicingStatus = servicingStatus;
                RushOrder = rushOrder;
                DueDate = dueDate;
                BusinessPartnerID = businessPartnerId;
                BusinessPartner = businessPartner;
                ProfileID = profileId;
                ProfileDescription = profileDescription;
                FastTrack = fastTrack;
                CreatedDate = createdDate;
                CreatedBy = createdBy;
                CompletedDate = completedDate;
                Contracts = contracts.ToList();
                #endregion Initialize Properties
                IsHydrated = true;
            }
            catch (Exception ex)
            {
                IsHydrated = false;
                ErrorMessages.Add("ServiceRequestDto::Initialize(...)", ex.Message, ex);
            }
        }
        public void Initialize(XElement xElement, IErrorMessages errorMessages = null)
        {
            try
            {
                var contracts = ContractDto.Parse(GetGroupingXElements(xElement, ContractDto.XmlGroupingFieldName, ContractDto.XmlFieldName).ToList());

                Initialize(
                            GetXElementAttribute(xElement, "ID"),
                            GetXElementAttribute(xElement, "Description"),
                            GetXElementAttribute(xElement, "TransactionType"),
                            GetXElementAttribute(xElement, "ServicingStatus"),
                            GetXElementAttribute(xElement, "RushOrder"),
                            GetXElementAttribute(xElement, "DueDate"),
                            GetXElementAttribute(xElement, "BusinessPartnerID"),
                            GetXElementAttribute(xElement, "BusinessPartner"),
                            GetXElementAttribute(xElement, "ProfileID"),
                            GetXElementAttribute(xElement, "ProfileDescription"),
                            GetXElementAttribute(xElement, "FastTrack"),
                            GetXElementAttribute(xElement, "CreatedDate"),
                            GetXElementAttribute(xElement, "CreatedBy"),
                            GetXElementAttribute(xElement, "CompletedDate"),
                            contracts
                        );//Initialize
                IsHydrated = true;
            }
            catch (Exception ex)
            {
                IsHydrated = false;
                ErrorMessages.Add("ServiceRequestDto Contstructor::ServiceRequestDto(XElement...)", ex.Message, ex);
            }
        }
        #region Constructors
        public ServiceRequestDto() : base() { }
        public ServiceRequestDto(string content) : base(content) { }
        public ServiceRequestDto(
                                string id, string description,
                                string transactionType, string servicingStatus,
                                string rushOrder, string dueDate,
                                string businessPartnerId, string businessPartner,
                                string profileId, string profileDescription,
                                string fastTrack, string createdDate,
                                string createdBy, string completedDate,
                                List<IContractDto> contracts
            ) : this()
        {
            Initialize( id, description, transactionType, 
                        servicingStatus, rushOrder, dueDate, 
                        businessPartnerId, businessPartner, 
                        profileId, profileDescription, 
                        fastTrack, createdDate, createdBy, 
                        completedDate, contracts);
        }
        public ServiceRequestDto(XElement xElement, IErrorMessages errorMessages = null) : this()
        {
            Initialize(xElement, errorMessages);
        }
        #endregion Constructors

        #region Static Parsers
        public static List<IServiceRequestDto> Parse(string xmlString, string xsdDocPath, bool suppressExceptions = true)
        {
            if (!XmlProcessor.IsXml(xmlString)) return null;
            XDocument xmldoc = XmlProcessor.LoadContent(xmlString);
            if (xmldoc == null) return null;
            return Parse(xmldoc, xsdDocPath, XmlFieldName, suppressExceptions);
        }
        public static List<IServiceRequestDto> Parse(IEnumerable<XElement> xElements)
        {
            var dtoElements = new List<IServiceRequestDto>();
            xElements.ToList<XElement>().ForEach(xElement =>
            {
                var instance = Instantiate<ServiceRequestDto>(xElement);
                dtoElements.Add((IServiceRequestDto) instance);
            });
            return dtoElements.Count > 0 ? dtoElements : null;
        }
        // TODO: Refactor the xElementName to be automatically understood from the static variable
        public static List<IServiceRequestDto> Parse(XDocument xmlDoc, 
                                                        string xsdDocPath, string xElementName, 
                                                        bool suppressExceptions = true)
        {
            if (!XsdProcessor.ValidateStatic(xsdDocPath, xmlDoc)) return null;
            return Parse(xmlDoc.Document.Elements(xElementName));
        }
        public static List<IServiceRequestDto> Parse(XElement xElement)
        {
            return Parse(xElement.Document.Elements(ServiceRequestDto.XmlFieldName));
        }
        public static List<IServiceRequestDto> Parse(XElement xElement, XmlSchemaSet xsdSchemas, string xElementName, bool suppressExceptions = true)
        {
            if (!XsdProcessor.ValidateStatic(xsdSchemas, xElement, xElementName)) return null;
            return Parse(xElement.Elements(xElementName));
        }
        #endregion Static Parsers
    }
}
