using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Xml.Linq;
using System.Xml.Schema;
using Mgm.VI.Common;
namespace Mgm.VI.Data.Dto
{
    public abstract class DtoBase
    {
        public string Content { get; }
        public XElement XmlElement { get; private set; }
        public IErrorMessages ErrorMessages { get; private set; }
        public bool IsHydrated { get; protected set; }
        //public void LogErrors {}

        public DtoBase(string content = null, IErrorMessages errorMessages = null)
        {
            if (content != null) Content = content;
            ErrorMessages = errorMessages ?? new ErrorMessages();//Use DI later to inject an instance
        }
        public static T Instantiate<T>(XElement xElement, IErrorMessages errorMessages = null) where T : DtoBase, new()
        {
            var instance = new T { XmlElement = xElement }; // DtoFactory<T>(xElement);
            ((IDto)instance).Initialize(xElement, errorMessages);
            return instance;
        }

        public static List<IT> Parse<IT, T>(IEnumerable<XElement> xElements) where T: DtoBase, IT, new()
        {
            var dtoElements = new List<IT>();
            xElements.ToList<XElement>().ForEach(xElement =>
            {
                var instance = Instantiate<T>(xElement);
                dtoElements.Add((IT)instance);
            });
            return dtoElements.Count > 0 ? dtoElements : null;
        }
        #region Static Parsers
        public static List<IT> Parse<IT, T>(string xmlString, string xsdDocPath, 
                                            string xmlFieldName, bool suppressExceptions = true) where T : DtoBase, IT, new()
        {
            if (!XmlProcessor.IsXml(xmlString)) return null;
            XDocument xmldoc = XmlProcessor.LoadContent(xmlString);
            if (xmldoc == null) return null;
            return Parse<IT, T>(xmldoc, xsdDocPath, xmlFieldName, suppressExceptions);
        }
        public static List<IT> Parse<IT, T>(XDocument xmlDoc,
                                                        string xsdDocPath, string xmlFieldName,
                                                        bool suppressExceptions = true) where T : DtoBase, IT, new()
        {
            if (!XsdProcessor.ValidateStatic(xsdDocPath, xmlDoc)) return null;
            return Parse<IT, T>(xmlDoc.Document.Elements(xmlFieldName));
        }
        public static List<IT> Parse<IT, T>(XElement xElement, string xmlFieldName) where T : DtoBase, IT, new()
        {
            return Parse<IT, T>(xElement.Document.Elements(xmlFieldName));
        }
        public static List<IT> Parse<IT, T>(XElement xElement, XmlSchemaSet xsdSchemas, 
                                            string xmlFieldName, bool suppressExceptions = true) where T: DtoBase, IT, new()
        {
            if (!XsdProcessor.ValidateStatic(xsdSchemas, xElement, xmlFieldName)) return null;
            return Parse<IT, T>(xElement.Elements(xmlFieldName));
        }
        #endregion Static Parsers
        public string DumpContent()
        {
            return Content;
        }
        protected static IEnumerable<XElement> GetGroupingXElements(XElement xElement, string xmlGroupingFieldName, string xmlFieldName)
        {
            var xElements = GetXElements(xElement, xmlGroupingFieldName);
            var subXElements = new List<XElement>();
            foreach (XElement subXElement in xElements)
            {
                var subElements = GetXElements(subXElement, xmlFieldName);
                subXElements.AddRange(subElements);
            }
            return subXElements;
        }
        protected static IEnumerable<XElement> GetGroupingXElements(XElement xElement, string xmlGroupingFieldName, List<string> xmlFieldNames)
        {
            var subXElements = new List<XElement>();
            foreach (var xmlFieldName in xmlFieldNames)
            {
                subXElements.AddRange(GetGroupingXElements(xElement, xmlGroupingFieldName, xmlFieldName));
            }
            return subXElements;
        }
        protected static string GetXElementValue(XElement xElement, string attributeName, bool suppressExceptions = true)
        {
            string value = string.Empty;
            try
            {
                var element = xElement.Element(attributeName).Value;
                value = element.ToString() ?? string.Empty;
            }
            catch (Exception ex)
            {
                if (!suppressExceptions) throw;
            }
            return value;
        }
        protected static string GetXElementAttribute(XElement xElement, string attributeName, bool suppressExceptions = true)
        {
            string value = string.Empty;
            try
            {
                var attribute = xElement.Attribute(attributeName);
                value = attribute.ToString() ?? string.Empty;
            }
            catch (Exception ex)
            {
                if (!suppressExceptions) throw;
            }
            return value;
        }
        private static IEnumerable<XElement> GetXElements(XElement xElement, string xmlNodeName)
        {
            var documentRoot = XDocument.Parse(xElement.ToString()).Root;
            var xElements = documentRoot.Elements(xmlNodeName);
            return xElements;
        }
    }
}
