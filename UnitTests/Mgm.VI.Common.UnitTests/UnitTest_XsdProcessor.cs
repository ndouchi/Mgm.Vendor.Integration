///////////////////////////////////////////////////////////////////////////
///   Namespace:      Mgm.VI.Common.UnitTests
///   Author:         Nour Douchi                    Date: 10/29/2019
///   Notes:          
///   Revision History:
///   Name:           Date:        Description:
///////////////////////////////////////////////////////////////////////////

using System;
using Xunit;
using Mgm.VI.Common;
using System.Xml.Schema;
using System.Xml;
using System.IO;
using System.Xml.Linq;

namespace Mgm.VI.Common.UnitTests
{
    public class UnitTest_XsdProcessor : UnitTest_XmlBase
    {
        #region Validate()
        [Fact]
        public void Success_Test_Validate_XsdDocPath_XmlString_HappyPath()
        {
            var xsdProcessor = new XsdProcessor();
            var result = xsdProcessor.Validate(ValidData.XsdDocPath, ValidData.XmlDoc);
            Assert.True(result, " Xml/Xsd Validation: Happy Path Test Passed.");
        }
        [Fact]
        public void Success_Test_Validate_XsdXmlHappyPathStringValues()
        {
            var xsdProcessor = new XsdProcessor();
            //XmlSchema Xschema = XmlSchema.Read("", XmlReader.Create(new StringReader(ValidData.XsdDocPath)));
            XmlSchemaSet schemas = new XmlSchemaSet();
            schemas.Add("", XmlReader.Create(new StringReader(ValidData.XsdDoc)));
            var result = xsdProcessor.Validate(schemas, ValidData.XmlDoc);
            Assert.True(result, " Xml/Xsd Validation: Happy Path Test Passed.");
        }
        [Fact]
        public void Failure_Test_Validate_XsdDocPathFail_XmlString_Exception()
        {
            var xsdProcessor = new XsdProcessor();
            var result = Assert.Throws<System.Xml.XmlException>(() => xsdProcessor.ValidateContent(MalformedData.XsdDocPathFail, ValidData.XmlDoc, false));
            Assert.NotNull(result);
        }
        [Fact]
        public void Failure_Test_Validate_XsdDocPath_EmptyXmlString()
        {
            var xsdProcessor = new XsdProcessor();
            var result = xsdProcessor.ValidateContent(ValidData.XsdDocPath, MalformedData.XmlDocEmpty, false);
            Assert.False(result);
        }
        #endregion Validate()
        #region ValidateContent()
        [Fact]
        public void Success_Test_ValidateContent_Valid_Xsd_Xml_String_Values()
        {
            var xsdProcessor = new XsdProcessor();
            var result = xsdProcessor.ValidateContent(ValidData.XsdDoc, ValidData.XmlDoc);
            Assert.True(result, " Xml/Xsd Validation: Xsd and Xml Strings Successful Test Passed.");
        }
        [Fact]
        public void Success_Test_ValidateContent_Valid_Xsd_String_And_XDoc()
        {
            var xsdProcessor = new XsdProcessor();
            XDocument xDoc = XDocument.Parse(ValidData.XmlDoc);
            var result = xsdProcessor.ValidateContent(ValidData.XsdDoc, xDoc);
            Assert.True(result, " Xml/Xsd Validation: Xsd String and XDoc Successful Test Passed.");
        }
        [Fact]
        public void Failure_Test_ValidateContent_EmptyXsd()
        {
            var xsdProcessor = new XsdProcessor();
            var result = xsdProcessor.ValidateContent(MalformedData.XsdDocEmpty, ValidData.XmlDoc);
            Assert.False(result, "Xml/Xsd Validation: Empty XSD Failure Test Passed.");
        }
        [Fact]
        public void Failure_Test_ValidateContent_EmptyXml()
        {
            var xsdProcessor = new XsdProcessor();
            var result = xsdProcessor.ValidateContent(ValidData.XsdDoc, MalformedData.XmlDocEmpty);
            Assert.False(result, " Xml/Xsd Validation: Empty Xml Failure Test Passed.");
        }
        [Fact]
        public void Failure_Test_ValidateContent_MalformedXml()
        {
            var xsdProcessor = new XsdProcessor();
            var result = xsdProcessor.ValidateContent(ValidData.XsdDoc, MalformedData.XmlDocMalformedXml);
            Assert.False(result, " Xml/Xsd Validation: Malformed Xml String 1 Test Passed.");
        }
        [Fact]
        public void Failure_Test_ValidateContent_MalformedXml_With_Exception()
        {
            var xsdProcessor = new XsdProcessor();
            var result = Assert.Throws<System.Xml.XmlException>(() => xsdProcessor.ValidateContent(ValidData.XsdDoc, MalformedData.XmlDocMalformedXml, false));
            Assert.NotNull(result);
        }

        //[Fact]
        //public void Failure_Test_ValidateContent_MalformedXml_Incorrect_Ids()
        //{
        //    var xsdProcessor = new XsdProcessor();
        //    var result = xsdProcessor.ValidateContent(ValidData.XsdDoc, MalformedData.XmlDocIncorrectIds);
        //    Assert.False(result, " Xml/Xsd Validation: Malformed Xml String, Incorrect-IDs Failure Test Passed.");
        //}

        [Fact]
        public void Failure_Test_ValidateContent_MalformedXml_Missing_Accounts()
        {
            var xsdProcessor = new XsdProcessor();
            var result = xsdProcessor.ValidateContent(ValidData.XsdDoc, MalformedData.XmlDocMissingAccounts);
            Assert.False(result, " Xml/Xsd Validation: Malformed Xml String - Missing-Accounts Failure Test Passed.");
        }
        [Fact]
        public void Failure_Test_ValidateContent_MalformedXml_Missing_Orders()
        {
            var xsdProcessor = new XsdProcessor();
            var result = xsdProcessor.ValidateContent(ValidData.XsdDoc, MalformedData.XmlDocMissingOrders);
            Assert.False(result, " Xml/Xsd Validation: Missing-Orders Failure Test Passed.");
        }
        #endregion ValidateContent()
        #region ValidateDocument
        [Fact]
        public void Success_Test_ValidateDocument_Schema_And_XmlPath()
        {
            var xsdProcessor = new XsdProcessor();
            XmlSchemaSet schemas = new XmlSchemaSet();
            schemas.Add("", XmlReader.Create(new StringReader(ValidData.XsdDoc)));
            var result = xsdProcessor.ValidateDocument(schemas, ValidData.XmlDocPath);
            Assert.True(result, " Xml/Xsd Validation - ValidateDocument: Valid Schema and XmlPath Test Passed.");
        }
        [Fact]
        public void Failure_Test_ValidateDocument_Schema_And_Malformed_XmlPath()
        {
            var xsdProcessor = new XsdProcessor();
            XmlSchemaSet schemas = new XmlSchemaSet();
            schemas.Add("", XmlReader.Create(new StringReader(ValidData.XsdDoc)));
            var result = xsdProcessor.ValidateDocument(schemas, MalformedData.XmlDocPathFail);
            Assert.False(result, " Xml/Xsd Validation - ValidateDocument - Valid Schema and Bad Xml Path Test Passed.");
        }
        [Fact]
        public void Failure_Test_ValidateDocument_Null_Schema_And_XmlPath()
        {
            var xsdProcessor = new XsdProcessor();
            XmlSchemaSet schemas = null;// new XmlSchemaSet();
            var result = xsdProcessor.ValidateDocument(schemas, ValidData.XmlDocPath);
            Assert.False(result, " Xml/Xsd Validation - ValidateDocument - Null schema and valid XmlPath Test Passed.");
        }
        [Fact]
        public void Success_Test_ValidateDocument_XsdPath_And_XmlPath()
        {
            var xsdProcessor = new XsdProcessor();
            var result = xsdProcessor.ValidateDocument(ValidData.XsdDocPath, ValidData.XmlDocPath);
            Assert.True(result, " Xml/Xsd Validation - ValidateDocument: Valid XsdPath and XmlPath Test Passed.");
        }
        [Fact]
        public void Failure_Test_ValidateDocument_MalformedData_XsdPath_And_XmlPath()
        {
            var xsdProcessor = new XsdProcessor();
            var result = xsdProcessor.ValidateDocument(MalformedData.XsdDocPathFail, ValidData.XmlDocPath);
            Assert.False(result, " Xml/Xsd Validation - ValidateDocument: Malformed XsdPath and Valid XmlPath Test Passed.");
        }
        [Fact]
        public void Failure_Test_ValidateDocument_XsdPath_And_MalformedData_XmlPath()
        {
            var xsdProcessor = new XsdProcessor();
            var result = xsdProcessor.ValidateDocument(ValidData.XsdDocPath, MalformedData.XmlDocPathFail);
            Assert.False(result, " Xml/Xsd Validation - ValidateDocument: Valid XsdPath and Malformed XmlPath Test Passed.");
        }
        #endregion ValidateDocument
        #region Instantiate_xsdProcessor
        [Fact]
        public void Success_Test_Instantiate_XsdProcessor_With_XsdDocPath()
        {
            var xsdProcessor = XsdProcessor.New(ValidData.XsdDocPath);
            Assert.NotNull(xsdProcessor);
        }
        [Fact]
        public void Failure_Test_Instantiate_XsdProcessor_With_Malformed_XsdDocPath()
        {
            var xsdProcessor = XsdProcessor.New(MalformedData.XsdDocPathFail);
            Assert.Null(xsdProcessor);
        }
        #endregion Instantiate_XsdProcessor
        #region LoadXsd
        [Fact]
        public void Success_Test_LoadXsd_XsdDocPath()
        {
            var xsdProcessor = new XsdProcessor();
            var result = xsdProcessor.LoadXsd(ValidData.XsdDocPath);
            Assert.True(result, " UnitTest_XsdProcessor - Test_LoadXsd_XsdDocPath: Happy Path Test Passed.");
        }
        [Fact]
        public void Failure_Test_LoadXsd_XsdDocPath()
        {
            var xsdProcessor = new XsdProcessor();
            var result = xsdProcessor.LoadXsd(MalformedData.XsdDocPathFail);
            Assert.False(result, " UnitTest_XsdProcessor - Test_LoadXsd_XsdDocPath: Sad Path Test Passed.");
        }
        #endregion LoadXsd
        #region GenerateSchema Methods
        [Fact]
        public void Success_Test_GenerateSchema_XsdDocPath()
        {
            var result = XsdProcessor.GenerateSchema(ValidData.XsdDocPath);
            Assert.NotNull(result);
        }
        [Fact]
        public void Failure_Test_GenerateSchema_XsdDocPath()
        {
            var result = XsdProcessor.GenerateSchema(MalformedData.XsdDocPathFail);
            Assert.Null(result);
        }
        [Fact]
        public void Success_Test_GenerateSchemaFromDocument_Valid_XsdDocPath()
        {
            var result = XsdProcessor.GenerateSchemaFromDocument(ValidData.XsdDocPath);
            Assert.NotNull(result);
        }
        [Fact]
        public void Failure_Test_GenerateSchemaFromDocument_Malformed_XsdDocPath()
        {
            var result = XsdProcessor.GenerateSchemaFromDocument(MalformedData.XsdDocPathFail);
            Assert.Null(result);
        }
        [Fact]
        public void Success_Test_GenerateSchemaFromString_Valid_XsdDoc()
        {
            var result = XsdProcessor.GenerateSchemaFromString(ValidData.XsdDoc);
            Assert.NotNull(result);
        }
        [Fact]
        public void Failure_Test_GenerateSchemaFromString_Empty_XsdDoc()
        {
            var result = XsdProcessor.GenerateSchemaFromString(MalformedData.XsdDocEmpty);
            Assert.Null(result);
        }
        [Fact]
        public void Failure_Test_GenerateSchemaFromString_Missing_Attribute_ComplextType_ClosingTags_XsdDoc()
        {
            var result = XsdProcessor.GenerateSchemaFromString(MalformedData.XsdDocMissing_Attribute_ComplexType_ClosingTags);
            Assert.Null(result);
        }
        #endregion GenerateSchema Methods
    }
}
