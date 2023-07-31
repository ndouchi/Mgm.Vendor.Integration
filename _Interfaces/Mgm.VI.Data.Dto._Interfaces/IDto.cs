using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Schema;
using Mgm.VI.Common;

namespace Mgm.VI.Data.Dto
{
    public interface IDto
    {
        XElement XmlElement { get;  }
        string Content { get; }
        IErrorMessages ErrorMessages { get; }
        bool IsHydrated { get; }
        void Initialize(XElement xElement, IErrorMessages errorMessages = null);
    }
}
