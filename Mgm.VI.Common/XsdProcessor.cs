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
    public class XsdProcessor : XProcessorBase
    {
        public List<string> SchemaErrors { get; private set; }
        public List<XmlSchema> Schemas { get; set; }
        public XmlSchemaSet SchemaSet { get; set; }
        public List<String> Warnings { get; set; }
        public bool SchemaLoaded { get; private set; }

        #region Constructors
        public XsdProcessor(IErrorMessages errorMessages = null) : base(errorMessages)
        {
            InitializeObjects();
        }
        #endregion Constructors

        #region Static Instantiators
        public static XsdProcessor Instantiate(string xsdDocPath, bool suppressExceptions = true)
        {
            return New(xsdDocPath, suppressExceptions);
        }
        public static XsdProcessor New(string xsdDocPath, bool suppressExceptions = true)
        {
            XsdProcessor instance = new XsdProcessor();
            try
            {
                if (!instance.LoadXsd(xsdDocPath, suppressExceptions))
                    instance = null;
            }
            catch
            {
                instance = null;
                if (!suppressExceptions) throw new Exception("Could not create a new instance because of faulty data");
            }

            return instance;
        }
        #endregion Static Instantiators

        #region Static Validators
        public static bool ValidateStatic(string xsdDocPath, string xmlString, bool suppressExceptions = true)
        {
            if (!File.Exists(xsdDocPath) || String.IsNullOrEmpty(xmlString.Trim())) return false;

            bool isValid;
            try
            {
                var schemas = GenerateSchemaFromDocument(xsdDocPath);
                isValid = ValidateStatic(schemas, xmlString);
            }
            catch (Exception ex)
            {
                isValid = false;
                if (!suppressExceptions) throw;
            }
            return isValid;
        }
        public static bool ValidateStatic(string xsdDocPath, XDocument xmlDoc, bool suppressExceptions = true)
        {
            if (!File.Exists(xsdDocPath) || xmlDoc == null) return false;

            bool isValid;
            try
            {
                var schemas = GenerateSchemaFromDocument(xsdDocPath);
                isValid = true;// ValidateStatic(schemas, xmlDoc);
            }
            catch (Exception ex)
            {
                isValid = false;
                if (!suppressExceptions) throw;
            }
            return isValid;
        }
        public static bool ValidateStatic(XmlSchemaSet schemas, string xmlDoc, bool suppressExceptions = true)
        {
            bool isValid;
            try
            {
                XDocument xDoc = XDocument.Parse(xmlDoc);
                isValid = true;// ValidateStatic(schemas, xDoc);
            }
            catch (Exception ex)
            {
                isValid = false;
                if (!suppressExceptions) throw;
            }

            return isValid;
        }
        public static bool ValidateStatic(XmlSchemaSet schemas, XElement xElement, string xElementName, bool suppressExceptions = true)
        {
            bool isValid = true;
            try
            {
                ///TODO: Write an extension method to handle xElement validation or/and add
                /// code to parse relevant schema sub-structure

                //xElement.Validate(schemas, (o, e) =>
                //{
                //    isValid = false;
                //});
            }
            catch (Exception ex)
            {
                isValid = false;
                if (!suppressExceptions) throw;
            }
            return isValid;
        }
        public static bool ValidateStatic(XmlSchemaSet schemas, XDocument xDoc, bool suppressExceptions = true)
        {
            bool isValid = true;
            try
            {
                xDoc.Validate(schemas, (o, e) =>
                {
                    isValid = false;
                });
            }
            catch (Exception ex)
            {
                isValid = false;
                if (!suppressExceptions) throw;
            }
            return isValid;
        }
        #endregion Static Validators

        #region Static GenerateSchema
        public static XmlSchemaSet GenerateSchema(string xsdDocPath, bool suppressExceptions = true) { return GenerateSchemaFromDocument(xsdDocPath, suppressExceptions); }
        public static XmlSchemaSet GenerateSchemaFromDocument(string xsdDocPath, bool suppressExceptions = true)
        {
            if (!File.Exists(xsdDocPath)) return null;

            string xsdContent = string.Empty;
            using DataSet ds = new DataSet();
            try
            {
                var xsdStream = File.OpenRead(xsdDocPath);
                ds.ReadXmlSchema(xsdStream);
                xsdContent = ds.GetXmlSchema();
            }
            catch
            {
                if (!suppressExceptions) throw;
            }
            return GenerateSchemaFromString(xsdContent);
        }
        public static XmlSchemaSet GenerateSchemaFromString(string xsdContent, bool suppressExceptions = true)
        {
            if (string.IsNullOrEmpty(xsdContent.Trim())) return null;
            XmlSchemaSet schemas;
            try
            {
                schemas = new XmlSchemaSet();
                schemas.Add("", XmlReader.Create(new StringReader(xsdContent)));
            }
            catch
            {
                schemas = null;
                if (!suppressExceptions) throw;
            }
            return schemas;
        }
        #endregion Static GenerateSchema

        public bool LoadXsd(string xsdDocPath, bool suppressExceptions = true)
        {
            if (String.IsNullOrEmpty(xsdDocPath)
               || (!File.Exists(xsdDocPath))
               //|| SchemaErrors.Any() 
               //|| Warnings.Any()
               ) return false;

            bool isLoaded = true;
            try
            {
                XmlTextReader schemaReader = new XmlTextReader(xsdDocPath);
                XmlSchema schema = XmlSchema.Read(schemaReader, (o, e) =>
                {
                    ErrorMessages.Add("XsdProcessor.cs::LoadXsd(string xsdDocPath)", e.Message);
                    isLoaded = false;
                });
                /// TODO: RE-WORK the following Schemas.Add and SchemaSet.Add.  Choose only one.
                Schemas ??= new List<XmlSchema>();
                Schemas.Add(schema);
                SchemaSet ??= new XmlSchemaSet();
                SchemaSet.Add(schema);
            }
            catch (Exception e)
            {
                isLoaded = false;
                ErrorMessages.Add("XsdProcessor.cs::LoadXsd(string xsdDocPath)", e.Message);
                if (!suppressExceptions) throw;
            }
            return isLoaded;
        }
        /// TODO: Decide to keep or remove the following AddSchema method
        public bool AddSchema(string xsdDocPath, bool suppressExceptions = true)
        {
            if (String.IsNullOrEmpty(xsdDocPath)
                || (!File.Exists(xsdDocPath))
                //|| SchemaErrors.Any()
                //|| Warnings.Any()
                ) return false;

            bool isValid;
            try
            {
                using var fs = File.OpenRead(xsdDocPath);
                XmlSchema schema = XmlSchema.Read(fs, ValidationEventHandler);
                /// TODO: RE-WORK the following Schemas.Add and SchemaSet.Add.  Choose only one.
                Schemas ??= new List<XmlSchema>();
                Schemas.Add(schema);
                SchemaSet ??= new XmlSchemaSet();
                SchemaSet.Add(schema);
                isValid = true;
            }
            catch (Exception ex)
            {
                ErrorMessages.Add("XsdProcessor.cs::AddSchema(schemaFileLocation)", ex.Message, ex);
                isValid = false;
                if (!suppressExceptions) throw;
            }
            return isValid;
        }

        #region Validation Methods
        public bool ValidateDocument(string xsdDocPath, string xmlDocPath, bool suppressExceptions = true)
        {
            if (!File.Exists(xsdDocPath) || !File.Exists(xmlDocPath)) return false;

            bool isValid;
            try
            {
                var xmlStream = File.OpenRead(xmlDocPath);
                var schemas = GenerateSchemaFromDocument(xsdDocPath);
                isValid = Validate(schemas, xmlStream);
            }
            catch (Exception ex)
            {
                isValid = false;
                ErrorMessages.Add(string.Format("Couldn't generate XmlSchemaSet from the path provided: {0}", xsdDocPath), ex.Message, ex);
                if (!suppressExceptions) throw;
            }
            return isValid;
        }
        public bool ValidateDocument(XmlSchemaSet schemas, string xmlDocPath, bool suppressExceptions = true)
        {
            if (File.Exists(xmlDocPath) && schemas != null)
            {
                using var xmlStream = File.OpenRead(xmlDocPath);
                return Validate(schemas, xmlStream);
            }
            return false;
        }
        public bool ValidateContent(string xsdDoc, string xmlDoc, bool suppressExceptions = true)
        {
            if (string.IsNullOrEmpty(xsdDoc.Trim()) || string.IsNullOrEmpty(xmlDoc.Trim())) return false;

            XmlSchemaSet schemas = new XmlSchemaSet();
            bool isValid;
            try
            {
                XDocument xDoc = XDocument.Parse(xmlDoc);
                schemas.Add("", XmlReader.Create(new StringReader(xsdDoc)));
                isValid = Validate(schemas, xDoc, suppressExceptions);
            }
            catch (Exception ex)
            {
                isValid = false;
                ErrorMessages.Add("XsdProcessor.cs::Validate(string xsdDoc, string xmlDoc)", ex.Message, ex);
                if (!suppressExceptions) throw;
            }
            return isValid;
        }
        public bool ValidateContent(string xsdDoc, XDocument xDoc, bool suppressExceptions = true)
        {
            if (string.IsNullOrEmpty(xsdDoc.Trim()) || xDoc == null) return false;

            XmlSchemaSet schemas = new XmlSchemaSet();
            bool isValid;
            try
            {
                schemas.Add("", XmlReader.Create(new StringReader(xsdDoc)));
                isValid = Validate(schemas, xDoc);
            }
            catch (Exception ex)
            {
                isValid = false;
                ErrorMessages.Add("XsdProcessor.cs::Validate(string xsdDoc, string xmlDoc)", ex.Message, ex);
                if (!suppressExceptions) throw;
            }

            return isValid;
        }
        public bool Validate(string xsdDocPath, string xmlString, bool suppressExceptions = true)
        {
            if (!File.Exists(xsdDocPath) || String.IsNullOrEmpty(xmlString.Trim())) return false;

            bool isValid;
            try
            {
                var schemas = GenerateSchemaFromDocument(xsdDocPath);
                isValid = Validate(schemas, xmlString);
            }
            catch (Exception ex)
            {
                isValid = false;
                ErrorMessages.Add(string.Format("Couldn't generate XmlSchemaSet from the path provided: {0}", xsdDocPath), ex.Message, ex);
                if (!suppressExceptions) throw;
            }
            return isValid;
        }
        public bool Validate(XmlSchemaSet schemas, string xmlDoc, bool suppressExceptions = true)
        {
            if (string.IsNullOrEmpty(xmlDoc.Trim()) || schemas == null) return false;
   
            bool isValid;
            try
            {
                XDocument xDoc = XDocument.Parse(xmlDoc);
                isValid = Validate(schemas, xDoc);
            }
            catch (Exception ex)
            {
                isValid = false;
                ErrorMessages.Add("XsdProcessor.cs::Validate(string xsdDoc, string xmlDoc)", ex.Message, ex);
                if (!suppressExceptions) throw;
            }

            return isValid;
        }
        public bool Validate(XmlSchemaSet schemas, XDocument xDoc, bool suppressExceptions = true)
        {
            if (schemas == null || xDoc == null) return false;

            bool isValid = true;
            try
            {
                xDoc.Validate(schemas, (o, e) =>
                {
                    ErrorMessages.Add("XsdProcessor.cs::Validate(string xsdDoc, string xDoc)", e.Message);
                    isValid = false;
                });
            }
            catch (Exception ex)
            {
                isValid = false;
                ErrorMessages.Add("XsdProcessor.cs::Validate(string xsdDoc, string xmlDoc)", ex.Message, ex);
                if (!suppressExceptions) throw;
            }
            return isValid;
        }
        public bool Validate(XmlSchemaSet schemas, Stream xmlStream, bool suppressExceptions = true)
        {
            if (schemas == null || xmlStream == null) return false;

            var settings = new XmlReaderSettings
            {
                ValidationType = ValidationType.Schema
            };
            settings.ValidationEventHandler += ValidationEventHandler;
            settings.Schemas = schemas;

            bool isValid = true;
            try
            {
                using var xmlFile = XmlReader.Create(xmlStream, settings);
                while (xmlFile.Read()) { }
            }
            catch (Exception ex)
            {
                isValid = false;
                SchemaErrors.Add(ex.Message);
                ErrorMessages.Add("XsdProcessor.cs::Validate(XmlSchemaSet schemas, Stream xmlStream,...)", ex.Message, ex);
                if (!suppressExceptions) throw;
            }

            return isValid;
        }
        public  bool Validate(string xsdDocPath, XDocument xmlDoc, bool suppressExceptions = true)
        {
            if (!File.Exists(xsdDocPath) || xmlDoc == null) return false;

            bool isValid;
            try
            {
                var schemas = GenerateSchemaFromDocument(xsdDocPath);
                isValid = Validate(schemas, xmlDoc);
            }
            catch (Exception ex)
            {
                isValid = false;
                SchemaErrors.Add(ex.Message);
                ErrorMessages.Add("XsdProcessor.cs::Validate(string xsdDocPath, XDocument xmlDoc,...)", ex.Message, ex);
                if (!suppressExceptions) throw;
            }
            return isValid;
        }
        public  bool Validate(XmlSchemaSet schemas, XElement xElement, string xElementName, bool suppressExceptions = true)
        {
            if (schemas == null || xElement == null || string.IsNullOrEmpty(xElementName.Trim())) return false;

            bool isValid = true;
            try
            {
                ///TODO: Write an extension method to handle xElement validation or/and add
                /// code to parse relevant schema sub-structure

                //xElement.Validate(schemas, (o, e) =>
                //{
                //    isValid = false;
                //});
            }
            catch (Exception ex)
            {
                isValid = false;
                SchemaErrors.Add(ex.Message);
                ErrorMessages.Add("XsdProcessor.cs::Validate(XmlSchemaSet schemas, XElement xElement, string xElementName,...)", ex.Message, ex);
                if (!suppressExceptions) throw;
            }
            return isValid;
        }
        #endregion Validation Methods

        #region Private Methods
        private void ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            switch (e.Severity)
            {
                case XmlSeverityType.Error:
                    ErrorMessages.Add("XsdProcessor.cs::ValidationEventHandler(...)", e.Message);
                    SchemaErrors.Add(e.Message);
                    break;
                case XmlSeverityType.Warning:
                    ErrorMessages.Add("XsdProcessor.cs::ValidationEventHandler(...)", e.Message);
                    Warnings.Add(e.Message);
                    break;
            }
        }
        private void InitializeObjects()
        {
            SchemaErrors = new List<string>();
            Schemas = new List<XmlSchema>();
            Warnings = new List<string>();
        }
        #endregion Private Methods
    }
}
