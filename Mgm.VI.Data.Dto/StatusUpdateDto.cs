using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Linq;
using Mgm.VI.Common;
using System.Xml.Schema;

namespace Mgm.VI.Data.Dto 
{
    public class StatusUpdateDto : ServiceRequestBaseDto, IStatusUpdateDto
    {
        #region Properties
        public int StatusUpdateId { get; set; }
        public string VendorId { get; set; }
        public string VendorName { get; set; }
        public string ServiceRequestId { get; set; }
        public string Comments { get; set; }
        public string MessageContent { get; set; }
        public string SqsRetrievalTimestamp { get; set; }
        public bool IsPersistedToMss { get; set; }
        #endregion Properties

        private void Initialize(int statusUpdateId, string vendorId,
                                string vendorName,
                                string serviceRequestId, string comments,
                                string messageContent, string sqsRetrievalTimestamp,
                                bool isPersistedToMss)
        {
            try
            {
                #region Initialize Properties
                StatusUpdateId = statusUpdateId;
                VendorId = vendorId;
                VendorName = vendorName;
                ServiceRequestId = serviceRequestId;
                Comments = comments;
                MessageContent = messageContent;
                SqsRetrievalTimestamp = sqsRetrievalTimestamp;
                IsPersistedToMss = isPersistedToMss;
                #endregion Initialize Properties
                IsHydrated = true;
            }
            catch (Exception ex)
            {
                IsHydrated = false;
                ErrorMessages.Add("StatusUpdateDto::Initialize(...)", ex.Message, ex);
            }
        }
        public void Initialize(XElement xElement, IErrorMessages errorMessages = null)
        {
            try
            {
                Initialize(-1, //StatusUpdateId
                            string.Empty, //vendorId
                            string.Empty, //vendorName
                            GetXElementAttribute(xElement, "ID"), //serviceRequestId
                            string.Empty, //comments
                            xElement.ToString(), //messageContent
                            DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"), // sqsRetrievalTimestamp
                            false // isPersistedToMss
                        );//Initialize
                IsHydrated = true;
            }
            catch (Exception ex)
            {
                IsHydrated = false;
                ErrorMessages.Add("StatusUpdateDto Contstructor::Initialize(XElement...)", ex.Message, ex);
            }
        }
        #region Constructors
        public StatusUpdateDto() : base() { }
        public StatusUpdateDto(string content) : base(content) { }
        public StatusUpdateDto( int    statusUpdateId, string vendorId,
                                string vendorName,
                                string serviceRequestId, string comments,
                                string messageContent, string sqsRetrievalTimestamp,
                                bool isPersistedToMss) : this()
        {
            Initialize( statusUpdateId, vendorId, 
                        vendorName, serviceRequestId,
                        comments, messageContent, sqsRetrievalTimestamp, isPersistedToMss);
        }
        #endregion Constructors

        #region Static Parsers
        public static List<IStatusUpdateDto> Parse(string xmlString, string xsdDocPath, bool suppressExceptions = true)
        {
            if (!XmlProcessor.IsXml(xmlString)) return null;
            XDocument xmldoc = XmlProcessor.LoadContent(xmlString);
            if (xmldoc == null) return null;
            return Parse(xmldoc, xsdDocPath, XmlFieldName, suppressExceptions);
        }
        public static List<IStatusUpdateDto> Parse(IEnumerable<XElement> xElements)
        {
            var dtoElements = new List<IStatusUpdateDto>();
            xElements.ToList<XElement>().ForEach(xElement =>
            {
                var instance = Instantiate<StatusUpdateDto>(xElement);
                dtoElements.Add((IStatusUpdateDto)instance);
            });
            return dtoElements.Count > 0 ? dtoElements : null;
        }
        public static List<IStatusUpdateDto> Parse( IEnumerable<XElement> xElements, 
                                                    IVendorDto vendorDto, string comments, 
                                                    string messageContent, 
                                                    string sqsRetrievalTimestamp,
                                                    bool isPersistedToMss = false
                                                    )
        {
            var dtoElements = new List<IStatusUpdateDto>();
            xElements.ToList().ForEach(xElement => Instantiate(vendorDto, comments, messageContent, 
                                                                sqsRetrievalTimestamp, isPersistedToMss, 
                                                                xElement, dtoElements));
            return dtoElements.Count > 0 ? dtoElements : null;
        }

        private static void Instantiate(IVendorDto vendorDto, string comments, string messageContent, 
                                        string sqsRetrievalTimestamp, bool isPersistedToMss, 
                                        XElement xElement, List<IStatusUpdateDto> dtoElements)
        {
            var instance = Instantiate<StatusUpdateDto>(xElement);
            ((IStatusUpdateDto)instance).VendorId = vendorDto.VendorId;
            ((IStatusUpdateDto)instance).VendorName = vendorDto.VendorName;
            ((IStatusUpdateDto)instance).Comments = comments;
            ((IStatusUpdateDto)instance).MessageContent = messageContent;
            ((IStatusUpdateDto)instance).SqsRetrievalTimestamp = sqsRetrievalTimestamp;
            ((IStatusUpdateDto)instance).IsPersistedToMss = isPersistedToMss;
            dtoElements.Add((IStatusUpdateDto)instance);
        }

        public static List<IStatusUpdateDto> Parse(XDocument xmlDoc,
                                                        string xsdDocPath, 
                                                        string xElementName, 
                                                        bool suppressExceptions = true)
        {
            if (!XsdProcessor.ValidateStatic(xsdDocPath, xmlDoc)) return null;
            return Parse(xmlDoc.Document.Elements(xElementName));
        }
        public static List<IStatusUpdateDto> Parse(XDocument xmlDoc,
                                                    string xsdDocPath,
                                                    string xElementName,
                                                    IVendorDto vendorDto, 
                                                    string comments,
                                                    string messageContent,
                                                    string sqsRetrievalTimestamp,
                                                    bool isPersistedToMss = false,
                                                    bool suppressExceptions = true)
        {
            if (!XsdProcessor.ValidateStatic(xsdDocPath, xmlDoc)) return null;
            return Parse(xmlDoc.Document.Elements(xElementName),
                            vendorDto, comments, messageContent,
                            sqsRetrievalTimestamp, isPersistedToMss);
        }
        public static List<IStatusUpdateDto> Parse(XElement xElement)
        {
            return Parse(xElement.Document.Elements(StatusUpdateDto.XmlFieldName));
        }
        public static List<IStatusUpdateDto> Parse(XElement xElement, XmlSchemaSet xsdSchemas, string xElementName, bool suppressExceptions = true)
        {
            if (!XsdProcessor.ValidateStatic(xsdSchemas, xElement, xElementName)) return null;
            return Parse(xElement.Elements(xElementName));
        }
        #endregion Static Parsers
    }
}
