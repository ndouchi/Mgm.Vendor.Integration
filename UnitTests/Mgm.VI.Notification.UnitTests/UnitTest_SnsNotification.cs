///////////////////////////////////////////////////////////////////////////
///   Namespace:      Mgm.VI.Notification.UnitTests
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

namespace Mgm.VI.Notification.UnitTests
{
    public class UnitTest_SnsNotification : UnitTest_NotificationBase
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

        #region IsXml()
        [Fact]
        public void Success_Test_IsXml_Valid_Xml()
        {
            var result = XmlProcessor.IsXml(ValidData.XmlDoc);
            Assert.True(result, " Xml Validation: IsXml(...) Successful Test Passed.");
        }
        [Fact]
        public void Failure_Test_IsXml_Malformed_Empty_Xml_With_Exception()
        {
            var result = Assert.Throws<System.Xml.XmlException>(() => XmlProcessor.IsXml(MalformedData.XmlDocMalformedXml, false));
            Assert.NotNull(result);
        }
        [Fact]
        public void Failure_Test_IsXml_Malformed_Empty_Xml()
        {
            var result = XmlProcessor.IsXml(MalformedData.XmlDocEmpty);
            Assert.False(result, " Xml Validation: IsXml(...) Empty Xml Test Passed.");
        }
        [Fact]
        public void Failure_Test_IsXml_Malformed_Xml()
        {
            var result = XmlProcessor.IsXml(MalformedData.XmlDocMalformedXml);
            Assert.False(result, " Xml Validation: IsXml(...) Malformed Xml Test Passed.");
        }
        #endregion IsXml()

        #region LoadFile
        [Fact]
        public void Success_Test_LoadFile_Valid_FilePath_Content()
        {
            var result = XmlProcessor.LoadFile(ValidData.XmlDocPath);
            Assert.True(result != null, " Xml Validation: LoadFile(...) Successful Test Passed.");
        }
        [Fact]
        public void Failure_Test_LoadFile_Valid_FilePath_Content_With_Exception()
        {
            var result = Assert.Throws<System.IO.FileNotFoundException>(() => XmlProcessor.LoadFile(MalformedData.XmlDocPathFail, false, false));
            Assert.NotNull(result);
        }
        [Fact]
        public void Failure_Test_LoadFile_Invalid_FilePath_Content()
        {
            var result = XmlProcessor.LoadFile(MalformedData.XmlDocPathFail);
            Assert.True(result == null, " Xml Validation: LoadFile(...) Invalid FilePath Test Passed.");
        }
        [Fact]
        public void Failure_Test_LoadFile_Malformed_File_Xml_Content()
        {
            var result = XmlProcessor.LoadFile(MalformedData.XmlDocMalformedXml);
            Assert.True(result == null, " Xml Validation: LoadFile(...) Malformed File Content Test Passed.");
        }
        #endregion LoadFile
    }
}
