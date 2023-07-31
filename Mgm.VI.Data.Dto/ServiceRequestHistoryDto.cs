using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Linq;
using Mgm.VI.Common;
using System.Xml.Schema;
using Mgm.VI.Data.Dto;

namespace Mgm.VI.Data.Dto 
{
    public class ServiceRequestHistoryDto : ServiceRequestBaseDto, IServiceRequestHistoryDto
    {
        #region Properties
        public int Id { get; set; }
        public string VendorId { get; set; }
        public string VendorName { get; set; }
        public string ServiceRequestId { get; set; }
        public string Comments { get; set; }
        public string MessageContent { get; set; }
        public string SubmissionTimestamp { get; set; }
        #endregion Properties

        private void Initialize(int id, string vendorId, string vendorName,
                                string serviceRequestId, string comments,
                                string messageContent, string submissionTimestamp)
        {
            try
            {
                #region Initialize Properties
                Id = id;
                VendorId = vendorId;
                VendorName = vendorName;
                ServiceRequestId = serviceRequestId;
                Comments = comments;
                MessageContent = messageContent;
                SubmissionTimestamp = submissionTimestamp;
                #endregion Initialize Properties
                IsHydrated = true;
            }
            catch (Exception ex)
            {
                IsHydrated = false;
                ErrorMessages.Add("ServiceRequestHistoryDto::Initialize(...)", ex.Message, ex);
            }
        }

        public void Initialize(XElement xElement, IErrorMessages errorMessages = null)
        {
            throw new NotImplementedException();
        }
        #region Constructors
        public ServiceRequestHistoryDto() : base() { }
        public ServiceRequestHistoryDto(int id, string vendorId,
                                        string vendorName, 
                                        string serviceRequestId, string comments,
                                        string messageContent, string submissionTimestamp) : this()
        {
            Initialize(id, vendorId, 
                        vendorName, serviceRequestId,
                        comments, messageContent, submissionTimestamp);
        }
        #endregion Constructors

        #region Static Parsers
        public static List<IServiceRequestHistoryDto> Parse(string xmlString, string xsdDocPath, bool suppressExceptions = true)
        {
            if (!XmlProcessor.IsXml(xmlString)) return null;
            XDocument xmldoc = XmlProcessor.LoadContent(xmlString);
            if (xmldoc == null) return null;
            return Parse(xmldoc, xsdDocPath, XmlFieldName, suppressExceptions);
        }
        public static List<IServiceRequestHistoryDto> Parse(IEnumerable<XElement> xElements)
        {
            var dtoElements = new List<IServiceRequestHistoryDto>();
            xElements.ToList<XElement>().ForEach(xElement =>
            {
                var instance = Instantiate<ServiceRequestHistoryDto>(xElement);
                dtoElements.Add((IServiceRequestHistoryDto)instance);
            });
            return dtoElements.Count > 0 ? dtoElements : null;
        }
        public static List<IServiceRequestHistoryDto> Parse(XDocument xmlDoc,
                                                        string xsdDocPath, string xElementName,
                                                        bool suppressExceptions = true)
        {
            if (!XsdProcessor.ValidateStatic(xsdDocPath, xmlDoc)) return null;
            return Parse(xmlDoc.Document.Elements(xElementName));
        }
        public static List<IServiceRequestHistoryDto> Parse(XElement xElement)
        {
            return Parse(xElement.Document.Elements(ServiceRequestHistoryDto.XmlFieldName));
        }
        public static List<IServiceRequestHistoryDto> Parse(XElement xElement, XmlSchemaSet xsdSchemas, string xElementName, bool suppressExceptions = true)
        {
            if (!XsdProcessor.ValidateStatic(xsdSchemas, xElement, xElementName)) return null;
            return Parse(xElement.Elements(xElementName));
        }
        #endregion Static Parsers
    }
}
