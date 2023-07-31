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
    public class TitleDto : DtoBase, ITitleDto
    {
        public static readonly string XmlFieldName = "Title";
        public static readonly string XmlGroupingFieldName = "Titles";

        #region Properties
        public string ID { get; set; }
        public string Description { get; set; }
        public string ServicingStatus { get; set; }
        public string IPMStatus { get; set; }
        public string ContractualDueDate { get; set; }
        public string LicenseStartDate { get; set; }
        public string EOPResource { get; set; }
        public string PPSResource { get; set; }
        public List<ILineItemDto> LineItems { get; set; }
        #endregion Properties

        private void Initialize(string id, string description, string servicingStatus, 
                                string ipmStatus, string contractualDueDate, 
                                string licenseStartDate, string eopResource, 
                                string ppsResource, List<ILineItemDto> lineItems)
        {
            try
            {
                #region Initialize Properties
                ID = id;
                Description = description;
                ServicingStatus = servicingStatus;
                IPMStatus = ipmStatus;
                ContractualDueDate = contractualDueDate;
                LicenseStartDate = licenseStartDate;
                EOPResource = eopResource;
                PPSResource = ppsResource;
                LineItems = lineItems.ToList();
                #endregion Initialize Properties
                IsHydrated = true;
            }
            catch (Exception ex)
            {
                IsHydrated = false;
                ErrorMessages.Add("TitleDto::Initialize(...)", ex.Message, ex);
            }
        }
        public void Initialize(XElement xElement, IErrorMessages errorMessages = null)
        {
            try
            {
                var lineItems = LineItemDto.Parse(GetGroupingXElements(xElement, LineItemDto.XmlGroupingFieldName, LineItemDto.XmlFieldName).ToList());

                Initialize(
                            GetXElementAttribute(xElement, "ID"),
                            GetXElementAttribute(xElement, "Description"),
                            GetXElementAttribute(xElement, "ServicingStatus"),
                            GetXElementAttribute(xElement, "IPMStatus"),
                            GetXElementAttribute(xElement, "ContractualDueDate"),
                            GetXElementAttribute(xElement, "LicenseStartDate"),
                            GetXElementAttribute(xElement, "EOPResource"),
                            GetXElementAttribute(xElement, "PPSResource"),
                            lineItems
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
        public TitleDto() : base() { }
        public TitleDto(string id, string description, string servicingStatus,
                        string ipmStatus, string contractualDueDate,
                        string licenseStartDate, string eopResource,
                        string ppsResource, List<ILineItemDto> lineItems
            ) : this()
        {
            Initialize( id, description, servicingStatus, 
                        ipmStatus, contractualDueDate, licenseStartDate, 
                        eopResource, ppsResource, lineItems);
        }
        public TitleDto(XElement xElement) : this() 
        {
            try
            {
                var lineItems = LineItemDto.Parse(GetGroupingXElements(xElement, LineItemDto.XmlGroupingFieldName, LineItemDto.XmlFieldName).ToList());

                Initialize(
                    GetXElementAttribute(xElement, "ID"),
                    GetXElementAttribute(xElement, "Description"),
                    GetXElementAttribute(xElement, "ServicingStatus"),
                    GetXElementAttribute(xElement, "IPMStatus"),
                    GetXElementAttribute(xElement, "ContractualDueDate"),
                    GetXElementAttribute(xElement, "LicenseStartDate"),
                    GetXElementAttribute(xElement, "EOPResource"),
                    GetXElementAttribute(xElement, "PPSResource"),
                    lineItems
                    );
            }
            catch (Exception ex)
            {
                IsHydrated = false;
                ErrorMessages.Add("TitleDto Contstructor::TitleDto(XElement ...)", ex.Message, ex);
            }
        }
        #endregion Constructors

        #region Static Parsers
        public static List<ITitleDto> Parse(IEnumerable<XElement> xElements)
        {
            var dtoElements = new List<ITitleDto>();
            xElements.ToList<XElement>().ForEach(xElement => dtoElements.Add((ITitleDto) new TitleDto(xElement)));
            return dtoElements.Count > 0 ? dtoElements : null;
        }
        public static List<ITitleDto> Parse(XDocument xmlDoc, string xsdDocPath, string xElementsName, bool suppressExceptions = true)
        {
            if (!XsdProcessor.ValidateStatic(xsdDocPath, xmlDoc)) return null;
            return Parse(xmlDoc.Document.Elements(xElementsName));
        }
        public static List<ITitleDto> Parse(XElement xElement)
        {
            return Parse(xElement.Document.Elements(TitleDto.XmlFieldName));
        }
        public static List<ITitleDto> Parse(XElement xElement, XmlSchemaSet xsdSchemas, string xElementName, bool suppressExceptions = true)
        {
            if (!XsdProcessor.ValidateStatic(xsdSchemas, xElement, xElementName)) return null;
            return Parse(xElement.Elements(xElementName));
        }
        #endregion Static Parsers
    }
}
