using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Schema;
using Mgm.VI.Common;
using Mgm.VI.Data.Dto;

namespace Mgm.VI.Data.Dto
{
    public class ContractDto : DtoBase, IContractDto
    {
        public static readonly string XmlFieldName = "Contract";
        public static readonly string XmlGroupingFieldName = "Contracts";
        #region Properties
        public string ID { get; set; }
        public string ServicingStatus { get; set; }
        public string Description { get; set; }
        public List<ITitleDto> Titles { get; set; }
        #endregion Properties

        private void Initialize(string id, string servicingStatus, string description, List<ITitleDto> titles)
        {
            try
            {
                #region Initialize Properties
                ID = id;
                ServicingStatus = servicingStatus;
                Description = description;
                Titles = titles.ToList();
                #endregion Initialize Properties
                IsHydrated = true;
            }
            catch (Exception ex)
            {
                IsHydrated = false;
                ErrorMessages.Add("ContractDto::Initialize(...)", ex.Message, ex);
            }
        }
        public void Initialize(XElement xElement, IErrorMessages errorMessages = null)
        {
            try
            {
                var titles = TitleDto.Parse(GetGroupingXElements(xElement, TitleDto.XmlGroupingFieldName, TitleDto.XmlFieldName));

                Initialize(
                                GetXElementAttribute(xElement, "ID"),
                                GetXElementAttribute(xElement, "Description"),
                                GetXElementAttribute(xElement, "ServicingStatus"),
                                titles
                            );
            }
            catch (Exception ex)
            {
                IsHydrated = false;
                ErrorMessages.Add("ContractDto Contstructor::ContractDto(XElement ..., bool sup...)", ex.Message, ex);
            }
        }
        #region Constructors
        public ContractDto() : base() { }
        public ContractDto(string id, string servicingStatus, string description, List<ITitleDto> titles) : this()
        {
            Initialize(id, servicingStatus, description, titles);
        }
        public ContractDto(XElement xElement, IErrorMessages errorMessages = null) : this()
        {
            Initialize(xElement, errorMessages);
        }
        #endregion Constructors

        #region Static Parsers
        public static List<IContractDto> Parse(IEnumerable<XElement> xElements)
        {
            var dtoElements = new List<IContractDto>();
            xElements.ToList<XElement>().ForEach(xElement => dtoElements.Add((IContractDto)(new ContractDto(xElement))));
            return dtoElements.Count > 0 ? dtoElements : null;
        }
        public static List<IContractDto> Parse(XDocument xmlDoc, string xsdDocPath, string xElementsName, bool suppressExceptions = true)
        {
            if (!XsdProcessor.ValidateStatic(xsdDocPath, xmlDoc)) return null;
            return Parse(xmlDoc.Document.Elements(xElementsName));
        }
        public static List<IContractDto> Parse(XElement xElement)
        {
            return Parse(xElement.Document.Elements(ContractDto.XmlFieldName));
        }
        public static List<IContractDto> Parse(XElement xElement, XmlSchemaSet xsdSchemas, string xElementName, bool suppressExceptions = true)
        {
            if (!XsdProcessor.ValidateStatic(xsdSchemas, xElement, xElementName)) return null;
            return Parse(xElement.Elements(xElementName));
        }
        #endregion Static Parsers
    }
}
