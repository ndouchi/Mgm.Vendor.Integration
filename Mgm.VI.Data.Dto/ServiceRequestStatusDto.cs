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
    public class ServiceRequestStatusDto : ServiceRequestBaseDto, IServiceRequestStatusDto
    {
        #region Properties
        public int Id { get; set; }
        public string MasterDataName { get; set; }
        public string MasterDataCode { get; set; }
        public string MasterDataValue { get; set; }
        public string SequenceOrder { get; set; }
        public string CreatedBy { get; set; }
        public string Comments { get; set; }
        public bool Active { get; set; }
        public string CreateDate { get; set; }
        public string UpdateDate { get; set; }
        #endregion Properties

        private void Initialize(int id, string masterDataName, string masterDataCode,
                                string masterDataValue, string sequenceOrder,
                                string createdBy, string comments,
                                bool active, string createDate,
                                string updateDate
                                )
        {
            try
            {
                #region Initialize Properties
                Id = id;
                MasterDataName = masterDataName;
                MasterDataCode = masterDataCode;
                MasterDataValue = masterDataValue;
                SequenceOrder = sequenceOrder;
                CreatedBy = createdBy;
                Comments = comments;
                Active = active;
                CreateDate = createDate;
                #endregion Initialize updateDate
                IsHydrated = true;
            }
            catch (Exception ex)
            {
                IsHydrated = false;
                ErrorMessages.Add("ServiceRequestStatusDto::Initialize(...)", ex.Message, ex);
            }
        }

        public void Initialize(XElement xElement, IErrorMessages errorMessages = null)
        {
            throw new NotImplementedException();
        }
        #region Constructors
        public ServiceRequestStatusDto() : base() { }
        public ServiceRequestStatusDto(int id, string masterDataName, string masterDataCode,
                                        string masterDataValue, string sequenceOrder,
                                        string createdBy, string comments,
                                        bool active, string createDate,
                                        string updateDate) : this()
        {
            Initialize(id, masterDataName, masterDataCode,
                                    masterDataValue, sequenceOrder,
                                    createdBy, comments,
                                    active, createDate,
                                    updateDate
                                    );
        }
        #endregion Constructors

        #region Static Parsers
        public static List<IServiceRequestStatusDto> Parse(string xmlString, string xsdDocPath, bool suppressExceptions = true)
        {
            if (!XmlProcessor.IsXml(xmlString)) return null;
            XDocument xmldoc = XmlProcessor.LoadContent(xmlString);
            if (xmldoc == null) return null;
            return Parse(xmldoc, xsdDocPath, XmlFieldName, suppressExceptions);
        }
        public static List<IServiceRequestStatusDto> Parse(IEnumerable<XElement> xElements)
        {
            var dtoElements = new List<IServiceRequestStatusDto>();
            xElements.ToList<XElement>().ForEach(xElement =>
            {
                var instance = Instantiate<ServiceRequestStatusDto>(xElement);
                dtoElements.Add((IServiceRequestStatusDto)instance);
            });
            return dtoElements.Count > 0 ? dtoElements : null;
        }
        public static List<IServiceRequestStatusDto> Parse(XDocument xmlDoc,
                                                        string xsdDocPath, string xElementName,
                                                        bool suppressExceptions = true)
        {
            if (!XsdProcessor.ValidateStatic(xsdDocPath, xmlDoc)) return null;
            return Parse(xmlDoc.Document.Elements(xElementName));
        }
        public static List<IServiceRequestStatusDto> Parse(XElement xElement)
        {
            return Parse(xElement.Document.Elements(ServiceRequestStatusDto.XmlFieldName));
        }
        public static List<IServiceRequestStatusDto> Parse(XElement xElement, XmlSchemaSet xsdSchemas, string xElementName, bool suppressExceptions = true)
        {
            if (!XsdProcessor.ValidateStatic(xsdSchemas, xElement, xElementName)) return null;
            return Parse(xElement.Elements(xElementName));
        }
        #endregion Static Parsers
    }
}
