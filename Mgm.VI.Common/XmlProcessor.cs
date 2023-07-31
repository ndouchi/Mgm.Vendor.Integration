using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Mgm.VI.Common
{
    public class XmlProcessor : XProcessorBase
    {
        private XmlProcessor(IErrorMessages errors = null) : base(errors) { }
        public static XmlProcessor Instantiate(string data, bool loadFromFile = false, bool validateFirst = true, bool suppressExceptions = true, IErrorMessages errorMessages = null)
        {
            return New(data, loadFromFile, validateFirst, suppressExceptions, errorMessages);
        }
        public static XmlProcessor New(string data, bool loadFromFile = false, bool validateFirst = true, bool suppressExceptions = true, IErrorMessages errorMessages = null)
        {
            XmlProcessor instance = new XmlProcessor(errorMessages);
            try
            {
                if (loadFromFile)
                    instance.XDoc = XmlProcessor.LoadFile(data, validateFirst, suppressExceptions);
                else
                    instance.XDoc = XmlProcessor.LoadContent(data, false);

                if (instance.XDoc == null)
                    instance = null;
            }
            catch
            {
                instance = null;
                if (!suppressExceptions) throw new Exception("Could not create a new instance because of faulty data");
            }

            return instance;
        }
        public static bool IsXml(string xmlContent, bool suppressExceptions = true)
        {
            if (String.IsNullOrEmpty(xmlContent)) return false;
            bool isXml = true;
            try
            {
                XDocument doc = XDocument.Parse(xmlContent);
            }
            catch (Exception ex)
            {
                isXml = false;
                //ErrorMessages.Add("XmlProcessor::IsXml(...)", ex.Message, ex);
                if (!suppressExceptions) throw;
            }
            return isXml;
        }
        public static XDocument LoadFile(string filePath, bool suppressExceptions = true, bool validateFirst = true)
        {
            if (validateFirst && !File.Exists(filePath)) return null;

            XDocument xdoc = null;
            try
            {
                xdoc = XDocument.Load(filePath);
            }
            catch (Exception ex)
            {
                if (!suppressExceptions) throw;
            }
            return xdoc;
        }
        public static XDocument LoadContent(string xmlContent, bool suppressExceptions = true, bool validateFirst = true)
        {
            if (validateFirst && !IsXml(xmlContent)) return null;

            XDocument xdoc = null;
            try
            {
                xdoc = XDocument.Parse(xmlContent);
            }
            catch (Exception ex)
            {
                if (!suppressExceptions) throw;
            }
            return xdoc;
        }
        public static XDocument ConvertToXdoc(string xmlContent, bool validateXmlFirst)
        {
            if (validateXmlFirst && !XmlProcessor.IsXml(xmlContent)) throw new Exception("XmlProcessor XmlContent Error: The XmlContent you provided isn't of a valid XML format.");
            XDocument xmlDoc = XmlProcessor.LoadContent(xmlContent, true, false);
            if (validateXmlFirst && xmlDoc == null) throw new Exception("XmlProcessor Xml-to-XDoc Conversion Error: the XDoc is null.");
            return xmlDoc;
        }
        public static XDocument XmlDocToXDoc(XmlDocument xmlDoc, bool suppressExceptions = true)
        {
            XDocument xdoc = null;
            try
            {
                xdoc = XDocument.Parse(xmlDoc.OuterXml);
            }
            catch (Exception ex)
            {
                if (!suppressExceptions) throw;
            }
            return xdoc;
        }
        public static XDocument XmlDocumentToXDocument(XmlDocument xmlDoc, bool suppressExceptions = true)
        {
            return XmlDocToXDoc(xmlDoc, suppressExceptions);
        }
        public static XDocument LoadXmlNavigator(XmlDocument xmlDoc, bool suppressExceptions = true)
        {
            
            XDocument xdoc = null;
            try
            {
                xdoc = XDocument.Load(xmlDoc.CreateNavigator().ReadSubtree());
            }
            catch (Exception ex)
            {
                if (!suppressExceptions) throw;
            }
            return xdoc;
        }
        public static XDocument LoadXmlNodeReader(XmlDocument xmlDoc, bool suppressExceptions)
        {
            XDocument xdoc = null;
            try
            {
                xdoc = XDocument.Load(new XmlNodeReader(xmlDoc));
            }
            catch (Exception ex)
            {
                if (!suppressExceptions) throw;
            }
            return xdoc;
        }
        public XDocument LoadXmlContent(string xmlContent, bool validateFirst = true, bool suppressExceptions = true)
        {
            XDocument xdoc = null;
            try
            {
                xdoc = LoadContent(xmlContent, validateFirst, suppressExceptions);
            }
            catch (Exception e)
            {
                if (!suppressExceptions) throw;
                else ErrorMessages.Add("XmlProcessor.cs::LoadXml(...)", e.Message, e);
            }
            return xdoc;
        }
        public XDocument LoadXmlFromFile(string filePath, bool validateFirst = true, bool suppressExceptions = true)
        {
            XDocument xdoc = null;
            try
            {
                xdoc = LoadFile(filePath, validateFirst, suppressExceptions);
            }
            catch (Exception e)
            {
                if (!suppressExceptions) throw;
                else ErrorMessages.Add("XmlProcessor.cs::LoadXmlFromFile(...)", e.Message, e);
            }
            return xdoc;
        }
        public void Parse(string xmlPath, string xmlContent)
        {
            XDocument quotesDoc = XDocument.Parse(xmlPath);
            //List<Custom.Quote> quotes = quotesDoc.Root
            //     .Elements("quote")
            //     .Select(x => new ServiceRequest
            //     {
            //         Content = (string)x.Attribute("Content"),
            //         Author = (string)x.Attribute("Author")
            //     })
            //     .ToList<Custom.Quote>();

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlContent);
            //Display all the Activities alone in the XML  
            XmlNodeList elemList = doc.GetElementsByTagName("Activity");
            for (int i = 0; i < elemList.Count; i++)
            {
            }
        }
        public static T Deserialize<T>(string input) where T : class
        {
            System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(T));

            using (StringReader sr = new StringReader(input))
            {
                return (T)ser.Deserialize(sr);
            }
        }
        public static string Serialize<T>(T ObjectToSerialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(ObjectToSerialize.GetType());

            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, ObjectToSerialize);
                return textWriter.ToString();
            }
        }
        public static Object ObjectToXML(string xml, Type objectType)
        {
            StringReader strReader = null;
            XmlSerializer serializer = null;
            XmlTextReader xmlReader = null;
            Object obj = null;
            try
            {
                strReader = new StringReader(xml);
                serializer = new XmlSerializer(objectType);
                xmlReader = new XmlTextReader(strReader);
                obj = serializer.Deserialize(xmlReader);
            }
            catch (Exception ex)
            {
                //Handle Exception Code
            }
            finally
            {
                if (xmlReader != null)
                {
                    xmlReader.Close();
                }
                if (strReader != null)
                {
                    strReader.Close();
                }
            }
            return obj;
        }
    }
}
