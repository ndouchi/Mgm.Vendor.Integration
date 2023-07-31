using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;
namespace Mgm.VI.Common
{
    public interface IXsdProcessor
    {
        List<string> SchemaErrors { get; }
        List<XmlSchema> Schemas { get; set; }
        XmlSchemaSet SchemaSet { get; set; }
        List<String> Warnings { get; set; }
        bool SchemaLoaded { get; }

        #region Static Instantiators
        //IXsdProcessor Instantiate(string xsdDocPath, bool suppressExceptions = true);
        //IXsdProcessor New(string xsdDocPath, bool suppressExceptions = true);
        #endregion Static Instantiators

        #region Static Validators
        //bool ValidateStatic(string xsdDocPath, string xmlString, bool suppressExceptions = true);
        //bool ValidateStatic(string xsdDocPath, XDocument xmlDoc, bool suppressExceptions = true);
        //bool ValidateStatic(XmlSchemaSet schemas, string xmlDoc, bool suppressExceptions = true);
        //bool ValidateStatic(XmlSchemaSet schemas, XElement xElement, string xElementName, bool suppressExceptions = true);
        //bool ValidateStatic(XmlSchemaSet schemas, XDocument xDoc, bool suppressExceptions = true);
        #endregion Static Validators

        #region Static GenerateSchema
        XmlSchemaSet GenerateSchema(string xsdDocPath, bool suppressExceptions = true);
        XmlSchemaSet GenerateSchemaFromDocument(string xsdDocPath, bool suppressExceptions = true);
        XmlSchemaSet GenerateSchemaFromString(string xsdContent, bool suppressExceptions = true);
        #endregion Static GenerateSchema

        bool LoadXsd(string xsdDocPath, bool suppressExceptions = true);
        /// TODO: Decide to keep or remove the following AddSchema method
        bool AddSchema(string xsdDocPath, bool suppressExceptions = true);

        #region Validation Methods
        bool ValidateDocument(string xsdDocPath, string xmlDocPath, bool suppressExceptions = true);
        bool ValidateDocument(XmlSchemaSet schemas, string xmlDocPath, bool suppressExceptions = true);
        bool ValidateContent(string xsdDoc, string xmlDoc, bool suppressExceptions = true);
        bool ValidateContent(string xsdDoc, XDocument xDoc, bool suppressExceptions = true);
        bool Validate(string xsdDocPath, string xmlString, bool suppressExceptions = true);
        bool Validate(XmlSchemaSet schemas, string xmlDoc, bool suppressExceptions = true);
        bool Validate(XmlSchemaSet schemas, XDocument xDoc, bool suppressExceptions = true);
        bool Validate(XmlSchemaSet schemas, Stream xmlStream, bool suppressExceptions = true);
        bool Validate(string xsdDocPath, XDocument xmlDoc, bool suppressExceptions = true);
        bool Validate(XmlSchemaSet schemas, XElement xElement, string xElementName, bool suppressExceptions = true);
        #endregion Validation Methods
    }
}