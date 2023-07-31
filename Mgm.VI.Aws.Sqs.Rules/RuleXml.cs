//////////////////////////////////////////////////////////////////
///
/// Engineer:   Nour Douchi
/// Company:    MGM
/// Project:    MGM-Vubiquity Asset Integration
/// Revision:   10/11/2019 
/// 
//////////////////////////////////////////////////////////////////

using Amazon.SQS.Model;
using Mgm.VI.Aws.Sqs.Rules;
using Mgm.VI.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Mgm.VI.Aws.Sqs.Rules
{
    public abstract class RuleXml : Rule, IRuleXml
    {
        private XDocument _xmlContent;

        public XDocument XmlContent { 
            get
            {
                return _xmlContent;
            }
            set 
            {
                _xmlContent = value;
            } 
        }

        public RuleXml(XDocument xmlContent, IErrorMessages errorMessages = null) : this(errorMessages) //TODO: Reverse contructor chaining order
        {
            _xmlContent = xmlContent;
        }

        public RuleXml(IErrorMessages errorMessages = null) : base (errorMessages) { }  //TODO: Reverse contructor chaining order
    }
}
