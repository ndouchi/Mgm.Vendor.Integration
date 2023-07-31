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
    public class RejectionDto : DtoBase, IRejectionDto
    {
        public static readonly List<string> XmlFieldName = GetRejectionStatuses();
        public static readonly string XmlGroupingFieldName = "Rejections";

        #region Properties
        public RejectionStatusCodeEnum StatusCode { get; set; }

        public string ID { get; set; }
        public string CurrentStatus { get; set; }
        public string Asset { get; set; }
        public string RejectionCode { get; set; }
        public string Issue { get; set; }
        public string CommentsHistory { get; set; }
        public string RejectedBy { get; set; }
        public string RejectionDate { get; set; }
        public string Urgency { get; set; }
        public string RootCause { get; set; }
        public string Document { get; set; }
        #endregion Properties

        private static List<string> GetRejectionStatuses()
        {
            List<string> statuses = new List<string>();
            statuses.Add(RejectionStatusCodeEnum.Rejection.ToString());
            statuses.Add(RejectionStatusCodeEnum.Redelivered.ToString());
            statuses.Add(RejectionStatusCodeEnum.Resolved.ToString());
            return statuses;
        }
        private void Initialize(string statusCode, string id,
                                string currentStatus, string asset,
                                string rejectionCode, string issue,
                                string commentsHistory, string rejectedBy,
                                string rejectionDate, string urgency,
                                string rootCause, string document)
        {
            try
            {
                #region Initialize Properties
                StatusCode = GetRejectionStatusCode(statusCode);
                ID = id;
                CurrentStatus = currentStatus;
                Asset = asset;
                RejectionCode = rejectionCode;
                Issue = issue;
                CommentsHistory = commentsHistory;
                RejectedBy = rejectedBy;
                RejectionDate = rejectionDate;
                Urgency = urgency;
                RootCause = rootCause;
                Document = document;
                #endregion Initialize Properties
                IsHydrated = true;
            }
            catch (Exception ex)
            {
                IsHydrated = false;
                ErrorMessages.Add("RejectionDto::Initialize(...)", ex.Message, ex);
            }
        }
        public void Initialize(XElement xElement, IErrorMessages errorMessages = null)
        {
            try
            {
                var statusCode = "";
                Initialize(statusCode,
                                GetXElementAttribute(xElement, "ID"),
                                GetXElementAttribute(xElement, "CurrentStatus"),
                                GetXElementAttribute(xElement, "Asset"),
                                GetXElementAttribute(xElement, "RejectionCode"),
                                GetXElementAttribute(xElement, "Issue"),
                                GetXElementAttribute(xElement, "CommentsHistory"),
                                GetXElementAttribute(xElement, "RejectionBy"),
                                GetXElementAttribute(xElement, "RejectionDate"),
                                GetXElementAttribute(xElement, "Urgency"),
                                GetXElementAttribute(xElement, "RootCause"),
                                GetXElementAttribute(xElement, "Document")
                            );
            }
            catch (Exception ex)
            {
                IsHydrated = false;
                ErrorMessages.Add("RejectionDto Contstructor::RejectionDto(XElement ...)", ex.Message, ex);
            }
        }
        #region Constructors
        public RejectionDto() : base() { }
        public RejectionDto(        string statusCode, string id, 
                                    string currentStatus, string asset, 
                                    string rejectionCode, string issue,
                                    string commentsHistory, string rejectedBy,
                                    string rejectionDate, string urgency,
                                    string rootCause, string document) : this()
        {
            Initialize( statusCode, id, currentStatus, 
                        asset, rejectionCode, issue, 
                        commentsHistory, rejectedBy, 
                        rejectionDate, urgency, 
                        rootCause, document);
        }
        public RejectionDto(XElement xElement, IErrorMessages errorMessages = null) : this()
        {
            Initialize(xElement, errorMessages);
        }
        #endregion Constructors

        #region Static Parsers
        public static List<IRejectionDto> Parse(IEnumerable<XElement> xElements)
        {
            var dtoElements = new List<IRejectionDto>();
            xElements.ToList<XElement>().ForEach(xElement => dtoElements.Add((IRejectionDto)new RejectionDto(xElement)));
            return (dtoElements.Count > 0 ? dtoElements : null);
        }
        public static List<IRejectionDto> Parse(XDocument xmlDoc, string xsdDocPath, string xElementsName, bool suppressExceptions = true)
        {
            if (!XsdProcessor.ValidateStatic(xsdDocPath, xmlDoc)) return null;
            return Parse(xmlDoc.Document.Elements(xElementsName));
        }
        public static List<IRejectionDto> Parse(XElement xElement)
        {
            List<IRejectionDto> dtos = new List<IRejectionDto>();
            RejectionDto.XmlFieldName.ForEach(rejectionStatus => 
                            dtos.AddRange(Parse(xElement.Document.Elements(rejectionStatus))));
            return dtos;
        }
        public static List<IRejectionDto> Parse(XElement xElement, XmlSchemaSet xsdSchemas, string xElementName, bool suppressExceptions = true)
        {
            if (!XsdProcessor.ValidateStatic(xsdSchemas, xElement, xElementName)) return null;
            return Parse(xElement.Elements(xElementName));
        }
  
        public static RejectionStatusCodeEnum GetRejectionStatusCode (string statusCode)
        {
            RejectionStatusCodeEnum statusCodeEnum;
            switch (statusCode.ToLower())
            {
                case "redelivered":
                    statusCodeEnum = RejectionStatusCodeEnum.Redelivered;
                    break;
                case "resolved":
                    statusCodeEnum = RejectionStatusCodeEnum.Resolved;
                    break;
                case "rejection":
                default:
                    statusCodeEnum = RejectionStatusCodeEnum.Rejection;
                    break;
            }
            return statusCodeEnum;
        }
        #endregion Static Parsers
    }
}
