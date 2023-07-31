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
    public class LineItemDto : DtoBase, ILineItemDto
    {
        public static readonly string XmlFieldName = "LineItem";
        public static readonly string XmlGroupingFieldName = "LineItems";

        #region Properties
        public string ID { get; set; }
       // public string AssetGroupId { get; set; }
        public string ServicingStatus { get; set; }
        public string IPMMedia { get; set; }
        public string IPMTerritory { get; set; }
        public string IPMLanguage { get; set; }
        public string LicenseStart { get; set; }
        public string LicenseEnd { get; set; }
        public List<IOrderDto> Orders { get; set; }
        #endregion Properties

        private void Initialize(string id,
                                //string assetGroupId,
                                string servicingStatus,
                                string ipmMedia, string ipmTerritory,
                                string ipmLanguage, string licenseStart,
                                string licenseEnd, List<IOrderDto> orders)
        {
            try
            {
                #region Initialize Properties
                ID = id;
                //AssetGroupId = assetGroupId;
                ServicingStatus = servicingStatus;
                IPMMedia = ipmMedia;
                IPMTerritory = ipmTerritory;
                IPMLanguage = ipmLanguage;
                LicenseStart = licenseStart;
                LicenseEnd = licenseEnd;
                Orders = orders.ToList();
                #endregion Initialize Properties
                IsHydrated = true;
            }
            catch (Exception ex)
            {
                IsHydrated = false;
                ErrorMessages.Add("LineItemDto::Initialize(...)", ex.Message, ex);
            }
        }

        public void Initialize(XElement xElement, IErrorMessages errorMessages = null)
        {
            try
            {
                var orders = OrderDto.Parse(GetGroupingXElements(xElement, OrderDto.XmlGroupingFieldName, OrderDto.XmlFieldName).ToList());

                Initialize(
                            GetXElementAttribute(xElement, "ID"),
                            //GetXElementAttribute(xElement, "AssetGroupId"),
                            GetXElementAttribute(xElement, "ServicingStatus"),
                            GetXElementAttribute(xElement, "IPMMedia"),
                            GetXElementAttribute(xElement, "IPMTerritory"),
                            GetXElementAttribute(xElement, "IPMLanguage"),
                            GetXElementAttribute(xElement, "LicenseStart"),
                            GetXElementAttribute(xElement, "LicenseEnd"),
                            orders
                          );
                IsHydrated = true;
            }
            catch (Exception ex)
            {
                IsHydrated = false;
                ErrorMessages.Add("LineItemDto Contstructor::LineItemDto(XElement...)", ex.Message, ex);
            }
        }
        #region Constructors
        public LineItemDto() : base() { }
        public LineItemDto( string id, 
                            //string assetGroupId,
                            string servicingStatus, 
                            string ipmMedia, string ipmTerritory, 
                            string ipmLanguage, string licenseStart, 
                            string licenseEnd, List<IOrderDto> orders) : this()
        {
            Initialize(id,
                        //assetGroupId,
                        servicingStatus, ipmMedia, ipmTerritory, ipmLanguage, licenseStart, licenseEnd, orders);
        }
        public LineItemDto(XElement xElement) : this()
        {
            Initialize(xElement);
        }
        #endregion Constructors

        #region Static Parsers
        public static List<ILineItemDto> Parse(IEnumerable<XElement> xElements)
        {
            var dtoElements = new List<ILineItemDto>();
            xElements.ToList<XElement>().ForEach(xElement => dtoElements.Add((ILineItemDto) (new LineItemDto(xElement))));
            return dtoElements.Count > 0 ? dtoElements : null;
        }
        public static List<ILineItemDto> Parse(XDocument xmlDoc, string xsdDocPath, string xElementsName, bool suppressExceptions = true)
        {
            if (!XsdProcessor.ValidateStatic(xsdDocPath, xmlDoc)) return null;
            return Parse(xmlDoc.Document.Elements(xElementsName));
        }
        public static List<ILineItemDto> Parse(XElement xElement)
        {
            return Parse(xElement.Document.Elements(LineItemDto.XmlFieldName));
        }
        public static List<ILineItemDto> Parse(XElement xElement, XmlSchemaSet xsdSchemas, string xElementName, bool suppressExceptions = true)
        {
            if (!XsdProcessor.ValidateStatic(xsdSchemas, xElement, xElementName)) return null;
            return Parse(xElement.Elements(xElementName));
        }
        #endregion Static Parsers
    }
}
