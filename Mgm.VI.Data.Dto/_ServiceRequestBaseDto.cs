using System;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml.Serialization;
using Mgm.VI.Common;
using System.Xml.Schema;

namespace Mgm.VI.Data.Dto
{
    public abstract class ServiceRequestBaseDto : DtoBase
    {
        public static readonly string XmlFieldName = "ServiceRequest";
        public static readonly string XmlGroupingFieldName = "ServiceRequests";

        #region Constructors
        public ServiceRequestBaseDto() : base() { }
        public ServiceRequestBaseDto(string content) : base(content) { }
        #endregion Constructors
    }
}
