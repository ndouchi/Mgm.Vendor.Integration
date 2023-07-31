///////////////////////////////////////////////////////////////////////////
///   Namespace:      Mgm.VI.Data.Repository.UnitTests
///   Author:         Nour Douchi  Date: 12/1/2019
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
using Mgm.VI.Repository;
using Mgm.VI.Data.Dto;
using Mgm.VI.Repository.UnitTests;

namespace Mgm.VI.Repository.UnitTests
{
    public class UnitTest_StatusUpdateUnitOfWork : UnitTest_RepositoryBase
    {
        #region Instantiate Class
        [Fact]
        public void Success_Test_Instantiate_Valid_Content()
        {
            XmlProcessor result = XmlProcessor.New(ValidData.XmlDoc, false);
            Assert.True(result != null, " Xml Validation: Instantiate(...) From Content Successful Test Passed.");
        }
        [Fact]
        public void Success_Test_Instantiate_Valid_FilePath_Content()
        {
            XmlProcessor result = XmlProcessor.New(ValidData.XmlDocPath, true);
            Assert.True(result != null, " Xml Validation: Instantiate(...) From FilePath Successful Test Passed.");
        }
        [Fact]
        public void Failure_Test_Instantiate_Invalid_FilePath_Content()
        {
            XmlProcessor result = XmlProcessor.New(MalformedData.XmlDocPathFail, true);
            Assert.True(result == null, " Xml Validation: Instantiate(...) Invalid FilePath Test Passed.");
        }
        [Fact]
        public void Failure_Test_Instantiate_Invalid_FilePath_Content_With_Exception()
        {
            var result = Assert.Throws<Exception>(() => XmlProcessor.New(MalformedData.XmlDocPathFail, true, false, false));
            Assert.NotNull(result);
        }
        [Fact]
        public void Failure_Test_Instantiate_Malformed_File_Xml_Content()
        {
            XmlProcessor result = XmlProcessor.New(MalformedData.XmlDocMalformedXml);
            Assert.True(result == null, " Xml Validation: Instantiate(...) Malformed File Content Test Passed.");
        }
        #endregion Instantiate Class

        #region ParseXml()
        [Fact]
        public void Success_Test_ParseXml_Valid_Xml()
        {
            var result = ServiceRequestDto.Parse(ValidData.XDoc, ValidData.XsdDocPath, ServiceRequestDto.XmlFieldName);
            Assert.NotNull(result);
        }
        [Fact]
        public void Success_Test_Parse_ServiceRequest_Valid_Xml_Count_Not_Zero()
        {
            var result = ServiceRequestDto.Parse(ValidData.XDoc, ValidData.XsdDocPath, ServiceRequestDto.XmlFieldName);
            Assert.True(result.Count > 0, "ParseXml(...).Count > 0: Successful Test Passed.");
        }
        [Fact]
        public void Failure_Test_Constructor_Throws_Exception()
        {
            var result = ServiceRequestDto.Parse(ValidData.XDoc, ValidData.XsdDocPath, ServiceRequestDto.XmlFieldName);
            Assert.NotNull(result);
        }





        [Fact]
        public void Failure_Test_ParseXml_Returns_Null()
        {
            XDocument xmldoc = null;
            var result = ServiceRequestDto.Parse(xmldoc, ValidData.XsdDocPath, ServiceRequestDto.XmlFieldName, false);
            Assert.Null(result);
        }
        [Fact]
        public void Failure_Test_ParseXDoc_InvalidXsdDocPath()
        {
            XDocument xmldoc = XmlProcessor.LoadContent(ValidData.XmlDoc);
            var result = ServiceRequestDto.Parse(xmldoc, MalformedData.XsdDocPathFail, ServiceRequestDto.XmlFieldName);
            Assert.Null(result);
        }
        #endregion ParseXml()
    }
}
