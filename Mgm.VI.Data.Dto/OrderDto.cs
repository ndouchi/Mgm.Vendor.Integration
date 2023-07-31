using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Linq;
using Mgm.VI.Common;
using AutoMapper;
using AutoMapper.Configuration.Annotations;
using System.Xml.Schema;
using Mgm.VI.Data.Dto;

namespace Mgm.VI.Data.Dto
{
    public class OrderDto : DtoBase, IOrderDto
    {
        public static readonly string XmlFieldName = "Order";
        public static readonly string XmlGroupingFieldName = "Orders";

        #region Properties
        public string ID { get; set; }
        public string ServicingStatus { get; set; }
        public string Version { get; set; }

        public string SRDueDate { get; set; }
        public string EmbargoDate { get; set; }
        public string PrimaryVideo { get; set; }
        public string SecondaryAudio { get; set; }
        public string SubtitlesFull { get; set; }
        public string SubtitlesForced { get; set; }
        public string ClosedCaptions { get; set; }
        public string Trailer { get; set; }
        public string RebillType { get; set; }
        public string Amount { get; set; }
        public string MetaData { get; set; }
        public string ArtWork { get; set; }
        public string Document { get; set; }
        public string EOPPO { get; set; }
        public string EOPDueDate { get; set; }
        public string EOPStatus { get; set; }
        public string EOPResource { get; set; }
        public string EOPNotes { get; set; }
        public string PPSPO { get; set; }
        public string PPSDueDate { get; set; }
        public string PPSStatus { get; set; }
        public string PPSResource { get; set; }
        public string PPSNotes { get; set; }
        public string MaterialNotes { get; set; }
        public string Other { get; set; }
        public string IPMMedia { get; set; }

        public List<IRejectionDto> Rejections { get; set; }
        #endregion Properties

        private void Initialize(string id, 
                                string servicingStatus, 
                                string version,
                                string sRDueDate,
                                string embargoDate,
                                string primaryVideo,
                                string secondaryAudio,
                                string subtitlesFull,
                                string subtitlesForced,
                                string closedCaptions,
                                string trailer,
                                string rebillType,
                                string amount,
                                string metaData,
                                string artWork,
                                string document,
                                string eOPPO,
                                string eOPDueDate,
                                string eOPStatus,
                                string eOPResource,
                                string eOPNotes,
                                string pPSPO,
                                string pPSDueDate,
                                string pPSStatus,
                                string pPSResource,
                                string pPSNotes,
                                string materialNotes,
                                string other,
                                string iPMMedia,
                                List<IRejectionDto> rejections)
        {
            try
            {
                #region Initialize Properties
                ID = id;
                ServicingStatus = servicingStatus;
                Version = version;
                SRDueDate = sRDueDate;
                EmbargoDate = embargoDate;
                PrimaryVideo = primaryVideo;
                SecondaryAudio = secondaryAudio;
                SubtitlesFull = subtitlesFull;
                SubtitlesForced = subtitlesForced;
                ClosedCaptions = closedCaptions;
                Trailer = trailer;
                RebillType = rebillType;
                Amount = amount;
                MetaData = metaData;
                ArtWork = artWork;
                Document = document;
                EOPPO = eOPPO;
                EOPDueDate = eOPDueDate;
                EOPStatus = eOPStatus;
                EOPResource = eOPResource;
                EOPNotes = eOPNotes;
                PPSPO = pPSPO;
                PPSDueDate = pPSDueDate;
                PPSStatus = pPSStatus;
                PPSResource = pPSResource;
                PPSNotes = pPSNotes;
                MaterialNotes = materialNotes;
                Other = other;
                IPMMedia = iPMMedia;
                Rejections = rejections.ToList();
                #endregion Initialize Properties
                IsHydrated = true;
            }
            catch (Exception ex)
            {
                IsHydrated = false;
                ErrorMessages.Add("OrderDto::Initialize(...)", ex.Message, ex);
            }
        }
        public void Initialize(XElement xElement, IErrorMessages errorMessages = null)
        {
            try
            {
                var rejections = RejectionDto.Parse(GetGroupingXElements(xElement, RejectionDto.XmlGroupingFieldName, RejectionDto.XmlFieldName).ToList());

                Initialize(
                            GetXElementAttribute(xElement, "ID"),
                            GetXElementAttribute(xElement, "ServicingStatus"),
                            GetXElementAttribute(xElement, "Version"),
                            GetXElementAttribute(xElement, "SRDueDate"),
                            GetXElementAttribute(xElement, "EmbargoDate"),
                            GetXElementAttribute(xElement, "PrimaryVideo"),
                            GetXElementAttribute(xElement, "SecondaryAudio"),
                            GetXElementAttribute(xElement, "SubtitlesFull"),
                            GetXElementAttribute(xElement, "SubtitlesForced"),
                            GetXElementAttribute(xElement, "ClosedCaptions"),
                            GetXElementAttribute(xElement, "Trailer"),
                            GetXElementAttribute(xElement, "RebillType"),
                            GetXElementAttribute(xElement, "Amount"),
                            GetXElementAttribute(xElement, "MetaData"),
                            GetXElementAttribute(xElement, "ArtWork"),
                            GetXElementAttribute(xElement, "Document"),
                            GetXElementAttribute(xElement, "EOPPO"),
                            GetXElementAttribute(xElement, "EOPDueDate"),
                            GetXElementAttribute(xElement, "EOPStatus"),
                            GetXElementAttribute(xElement, "EOPResource"),
                            GetXElementAttribute(xElement, "EOPNotes"),
                            GetXElementAttribute(xElement, "PPSPO"),
                            GetXElementAttribute(xElement, "PPSDueDate"),
                            GetXElementAttribute(xElement, "PPSStatus"),
                            GetXElementAttribute(xElement, "PPSResource"),
                            GetXElementAttribute(xElement, "PPSNotes"),
                            GetXElementAttribute(xElement, "MaterialNotes"),
                            GetXElementAttribute(xElement, "Other"),
                            GetXElementAttribute(xElement, "IPMMedia"),
                            rejections
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
        public OrderDto() : base() { }
        public OrderDto(    string id,
                                string servicingStatus,
                                string version,
                                string sRDueDate,
                                string embargoDate,
                                string primaryVideo,
                                string secondaryAudio,
                                string subtitlesFull,
                                string subtitlesForced,
                                string closedCaptions,
                                string trailer,
                                string rebillType,
                                string amount,
                                string metaData,
                                string artWork,
                                string document,
                                string eOPPO,
                                string eOPDueDate,
                                string eOPStatus,
                                string eOPResource,
                                string eOPNotes,
                                string pPSPO,
                                string pPSDueDate,
                                string pPSStatus,
                                string pPSResource,
                                string pPSNotes,
                                string materialNotes,
                                string other,
                                string iPMMedia,
                            List<IRejectionDto> rejections) : this()
        {
            Initialize(id,
                       servicingStatus,
                       version,
                       sRDueDate,
                       embargoDate,
                       primaryVideo,
                       secondaryAudio,
                       subtitlesFull,
                       subtitlesForced,
                       closedCaptions,
                       trailer,
                       rebillType,
                       amount,
                       metaData,
                       artWork,
                       document,
                       eOPPO,
                       eOPDueDate,
                       eOPStatus,
                       eOPResource,
                       eOPNotes,
                       pPSPO,
                       pPSDueDate,
                       pPSStatus,
                       pPSResource,
                       pPSNotes,
                       materialNotes,
                       other,
                       iPMMedia,
                       rejections);
        }
        public OrderDto(XElement xElement) : this()
        {
            try
            {
                var rejections = RejectionDto.Parse(GetGroupingXElements(xElement, RejectionDto.XmlGroupingFieldName, RejectionDto.XmlFieldName).ToList());

                Initialize(
                            GetXElementAttribute(xElement, "ID"),
                            GetXElementAttribute(xElement, "ServicingStatus"),
                            GetXElementAttribute(xElement, "Version"),
                            GetXElementValue(xElement, "SRDueDate"),
                            GetXElementValue(xElement, "EmbargoDate"),
                            GetXElementValue(xElement, "PrimaryVideo"),
                            GetXElementValue(xElement, "SecondaryAudio"),
                            GetXElementValue(xElement, "SubtitlesFull"),
                            GetXElementValue(xElement, "SubtitlesForced"),
                            GetXElementValue(xElement, "ClosedCaptions"),
                            GetXElementValue(xElement, "Trailer"),
                            GetXElementValue(xElement, "RebillType"),
                            GetXElementValue(xElement, "Amount"),
                            GetXElementValue(xElement, "MetaData"),
                            GetXElementValue(xElement, "ArtWork"),
                            GetXElementValue(xElement, "Document"),
                            GetXElementValue(xElement, "EOPPO"),
                            GetXElementValue(xElement, "EOPDueDate"),
                            GetXElementValue(xElement, "EOPStatus"),
                            GetXElementValue(xElement, "EOPResource"),
                            GetXElementValue(xElement, "EOPNotes"),
                            GetXElementValue(xElement, "PPSPO"),
                            GetXElementValue(xElement, "PPSDueDate"),
                            GetXElementValue(xElement, "PPSStatus"),
                            GetXElementValue(xElement, "PPSResource"),
                            GetXElementValue(xElement, "PPSNotes"),
                            GetXElementValue(xElement, "MaterialNotes"),
                            GetXElementValue(xElement, "Other"),
                            GetXElementValue(xElement, "IPMMedia"),

                            rejections
                           );

                IsHydrated = true;
            }
            catch (Exception ex)
            {
                IsHydrated = false;
                ErrorMessages.Add("OrderDto Contstructor::OrderDto(XElement...)", ex.Message, ex);
            }
        }
        #endregion Constructors

        #region Static Parsers
        public static List<IOrderDto> Parse(IEnumerable<XElement> xElements)
        {
            var dtoElements = new List<IOrderDto>();
            xElements.ToList<XElement>().ForEach(xElement => dtoElements.Add((IOrderDto)(new OrderDto(xElement))));
            return dtoElements.Count > 0 ? dtoElements : null;
        }
        public static List<IOrderDto> Parse(XDocument xmlDoc, string xsdDocPath, string xElementsName, bool suppressExceptions = true)
        {
            if (!XsdProcessor.ValidateStatic(xsdDocPath, xmlDoc)) return null;
            return Parse(xmlDoc.Document.Elements(xElementsName));
        }
        public static List<IOrderDto> Parse(XElement xElement)
        {
            return Parse(xElement.Document.Elements(OrderDto.XmlFieldName));
        }
        public static List<IOrderDto> Parse(XElement xElement, XmlSchemaSet xsdSchemas, string xElementName, bool suppressExceptions = true)
        {
            if (!XsdProcessor.ValidateStatic(xsdSchemas, xElement, xElementName)) return null;
            return Parse(xElement.Elements(xElementName));
        }
        #endregion Static Parsers
    }
}
