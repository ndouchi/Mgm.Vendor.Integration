using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Schema;

namespace Mgm.Mss.Common
{
    class XsdValidator2
    {

        private void Load()
        {
            XmlDocument xmlDoc = new XmlDocument();

            string schemaFile = @"Test.xsd";

            XmlTextReader schemaReader = new XmlTextReader(schemaFile);
            XmlSchema schema = XmlSchema.Read(schemaReader, SchemaValidationHandler);

            xmlDoc.Schemas.Add(schema);

            string filename = @"FileToValidate.xml";
            xmlDoc.Load(filename);
            xmlDoc.Validate(DocumentValidationHandler);

        }
        private static void SchemaValidationHandler(object sender, ValidationEventArgs e)
        {
            System.Console.WriteLine(e.Message);
        }

        private static void DocumentValidationHandler(object sender, ValidationEventArgs e)
        {
            System.Console.WriteLine(e.Message);
        }
    }
}