//////////////////////////////////////////////////////////////////
///
/// Engineer:   Nour Douchi
/// Company:    MGM
/// Project:    MGM-Vubiquity Asset Integration
/// Revision:   10/11/2019 
/// 
//////////////////////////////////////////////////////////////////

using Amazon.SQS;
using Amazon.SQS.Model;
using Mgm.VI.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace Mgm.VI.Aws.Sqs.Rules
{
    public class SqsRule_ValidateXsd : Rule
    {
        public XDocument Xdocument { get; set; }
        public XmlSchemaSet XmlSchemas {get; set;}
        public override bool IsMet
        {
            get
            {
                Validate();
                return __isMet;
            }
        }
        public SqsRule_ValidateXsd() : base() { }
        public SqsRule_ValidateXsd(XmlSchemaSet xmlSchemas) : this(null, xmlSchemas) { }
        public SqsRule_ValidateXsd(string xsdDocPath)
                        : this(null, XsdProcessor.GenerateSchemaFromDocument(xsdDocPath)) { }
        public SqsRule_ValidateXsd(XDocument xDoc, string xsdDocPath)
                          : this(xDoc, XsdProcessor.GenerateSchemaFromDocument(xsdDocPath)) { }
        public SqsRule_ValidateXsd(XDocument xDoc, XmlSchemaSet xmlSchemas) : base(xDoc)
        {
            Xdocument = xDoc;
            XmlSchemas = xmlSchemas;
        }
        public SqsRule_ValidateXsd(XDocument xDoc, XmlSchema xmlSchema)
            : this(xDoc, ConvertSchemaToSet(xmlSchema)) { }
        private static XmlSchemaSet ConvertSchemaToSet(XmlSchema xmlSchema)
        {
            var xs = new XmlSchemaSet();
            xs.Add(xmlSchema);
            return xs;
        }
        private void ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            if (e.Severity == XmlSeverityType.Error)
            {
                AddToReasonsNotMet(e.Message);
            }
        }
        private void XsdValidate()
        {
            var xsdValidator = new XsdProcessor();
            string guid = Guid.NewGuid().ToString();
            if (this.Content != null) Xdocument = XmlProcessor.LoadContent(this.Content.ToString());
            //__isMet = xsdValidator.Validate(XmlSchemas, Xdocument);
            if (!__isMet)
            {
                ErrorMessages.Add("SqsRule_ValidateXsd::XsdValidate()", String.Format("GUID: {0} \n XsdValidate Failed ", guid));
                if (xsdValidator.ErrorMessages != null) xsdValidator.ErrorMessages.ForEach(e =>
                                                            {
                                                                ErrorMessages.Add(String.Format("SqsRule_ValidateXsd::XsdValidate() => {0}", 
                                                                                            e.ErrorSource), 
                                                                            String.Format("GUID: {0} \n {1}", guid, e.ErrorText), 
                                                                            e.ErrorException);
                                                            });
            }
        }
        private void Validate()
        {
            if (Content == null)
                AddToReasonsNotMet("SqsRule_ValidateXsd does not have a valid message.  Content object is null.");
            else
                XsdValidate();
        }
    }
}
