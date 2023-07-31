///////////////////////////////////////////////////////////////////////////
///   Namespace:      Mgm.Mss.Data.AutoMapper.UnitTests
///   Author:         Nour Douchi                    Date: 11/05/2019
///   Notes:          
///   Revision History:
///   Name:           Date:        Description:
///////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using Xunit;
using System.Xml.Schema;
using System.Xml;
using System.IO;
using System.Xml.Linq;
using Mgm.Mss.Data.AutoMapper;
using Mgm.Mss.Data.Dto;
using AutoMapper;


namespace Mgm.Mss.Data.AutoMapper.UnitTests
{
    public class UnitTest_Bootstrapper
    {
        static class ValidData
        {
            #region 
            public static string XsdDoc = @"<?xml version='1.0' encoding='UTF-8'?><xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema' elementFormDefault='qualified' attributeFormDefault='unqualified'><xs:element name='ServiceRequest'><xs:complexType><xs:sequence><xs:element name='Contracts'><xs:complexType><xs:sequence><xs:element name='Contract'><xs:complexType><xs:sequence><xs:element name='Titles'><xs:complexType><xs:sequence><xs:element name='Title'><xs:complexType><xs:sequence><xs:element name='LineItems'><xs:complexType><xs:sequence><xs:element name='LineItem'><xs:complexType><xs:sequence><xs:element name='Orders'><xs:complexType><xs:sequence><xs:element name='Order' maxOccurs='unbounded'><xs:complexType><xs:sequence><xs:element name='VendorId' type='xs:string'></xs:element><xs:element name='SourceMioAssetId' type='xs:string'></xs:element><xs:element name='ShellMioAssetId' type='xs:string'></xs:element><xs:element name='SRDueDate' type='xs:string'></xs:element><xs:element name='EmbargoDate'></xs:element><xs:element name='PrimaryVideo' type='xs:string'></xs:element><xs:element name='SecondaryAudio' type='xs:string'></xs:element><xs:element name='SubtitlesFull' type='xs:string'></xs:element><xs:element name='SubtitlesForced' type='xs:string'></xs:element><xs:element name='ClosedCaptions'></xs:element><xs:element name='Trailer'></xs:element><xs:element name='RebillType' type='xs:string'></xs:element><xs:element name='Amount'></xs:element><xs:element name='MetaData'></xs:element><xs:element name='ArtWork'></xs:element><xs:element name='Document'></xs:element><xs:element name='EOPPO'></xs:element><xs:element name='EOPDueDate'></xs:element><xs:element name='EOPStatus' type='xs:string'></xs:element><xs:element name='EOPResource'></xs:element><xs:element name='EOPNotes'></xs:element><xs:element name='PPSPO' type='xs:int'></xs:element><xs:element name='PPSDueDate' type='xs:string'></xs:element><xs:element name='PPSStatus' type='xs:string'></xs:element><xs:element name='PPSResource'></xs:element><xs:element name='PPSNotes'></xs:element><xs:element name='MaterialNotes' type='xs:string'></xs:element><xs:element name='Other'></xs:element><xs:element name='IPMMedia'></xs:element><xs:element name='Rejections'><xs:complexType><xs:sequence><xs:element name='Rejection'><xs:complexType><xs:attribute name='ID' type='xs:string'></xs:attribute><xs:attribute name='CurrentStatus' type='xs:string'></xs:attribute><xs:attribute name='Asset' type='xs:string'></xs:attribute><xs:attribute name='RejectionCode' type='xs:string'></xs:attribute><xs:attribute name='Issue' type='xs:string'></xs:attribute><xs:attribute name='CommentsHistory' type='xs:string'></xs:attribute><xs:attribute name='RejectedBy' type='xs:string'></xs:attribute><xs:attribute name='RejectionDate' type='xs:string'></xs:attribute><xs:attribute name='Urgency' type='xs:string'></xs:attribute><xs:attribute name='RootCause' type='xs:string'></xs:attribute><xs:attribute name='Document' type='xs:string'></xs:attribute></xs:complexType></xs:element><xs:element name='Redelivered'><xs:complexType><xs:attribute name='ID' type='xs:string'></xs:attribute><xs:attribute name='CurrentStatus' type='xs:string'></xs:attribute><xs:attribute name='Asset' type='xs:string'></xs:attribute><xs:attribute name='RejectionCode' type='xs:string'></xs:attribute><xs:attribute name='Issue' type='xs:string'></xs:attribute><xs:attribute name='CommentsHistory' type='xs:string'></xs:attribute><xs:attribute name='RejectedBy' type='xs:string'></xs:attribute><xs:attribute name='RejectionDate' type='xs:string'></xs:attribute><xs:attribute name='Urgency' type='xs:string'></xs:attribute><xs:attribute name='RootCause' type='xs:string'></xs:attribute><xs:attribute name='Document' type='xs:string'></xs:attribute></xs:complexType></xs:element><xs:element name='Resolved'><xs:complexType><xs:attribute name='ID' type='xs:string'></xs:attribute><xs:attribute name='CurrentStatus' type='xs:string'></xs:attribute><xs:attribute name='Asset' type='xs:string'></xs:attribute><xs:attribute name='RejectionCode' type='xs:string'></xs:attribute><xs:attribute name='Issue' type='xs:string'></xs:attribute><xs:attribute name='CommentsHistory' type='xs:string'></xs:attribute><xs:attribute name='RejectedBy' type='xs:string'></xs:attribute><xs:attribute name='RejectionDate' type='xs:string'></xs:attribute><xs:attribute name='Urgency' type='xs:string'></xs:attribute><xs:attribute name='RootCause' type='xs:string'></xs:attribute><xs:attribute name='Document' type='xs:string'></xs:attribute></xs:complexType></xs:element></xs:sequence></xs:complexType></xs:element></xs:sequence><xs:attribute name='ID' type='xs:string'></xs:attribute><xs:attribute name='ServicingStatus' type='xs:string'></xs:attribute><xs:attribute name='Version' type='xs:string'></xs:attribute></xs:complexType></xs:element></xs:sequence></xs:complexType></xs:element></xs:sequence><xs:attribute name='ID' type='xs:string'></xs:attribute><xs:attribute name='ServicingStatus' type='xs:string'></xs:attribute><xs:attribute name='IPMMedia' type='xs:string'></xs:attribute><xs:attribute name='IPMTerritory' type='xs:string'></xs:attribute><xs:attribute name='IPMLanguage' type='xs:string'></xs:attribute><xs:attribute name='LicenseStart' type='xs:string'></xs:attribute><xs:attribute name='LicenseEnd' type='xs:string'></xs:attribute></xs:complexType></xs:element></xs:sequence></xs:complexType></xs:element></xs:sequence><xs:attribute name='ID' type='xs:string'></xs:attribute><xs:attribute name='Description' type='xs:string'></xs:attribute><xs:attribute name='ServicingStatus' type='xs:string'></xs:attribute><xs:attribute name='IPMStatus' type='xs:string'></xs:attribute><xs:attribute name='ContractualDueDate' type='xs:string'></xs:attribute><xs:attribute name='LicenseStartDate' type='xs:string'></xs:attribute><xs:attribute name='EOPResource' type='xs:string'></xs:attribute><xs:attribute name='PPSResource' type='xs:string'></xs:attribute></xs:complexType></xs:element></xs:sequence></xs:complexType></xs:element></xs:sequence><xs:attribute name='ID' type='xs:string'></xs:attribute><xs:attribute name='Description' type='xs:string'></xs:attribute><xs:attribute name='ServicingStatus' type='xs:string'></xs:attribute></xs:complexType></xs:element></xs:sequence></xs:complexType></xs:element></xs:sequence><xs:attribute name='ID' type='xs:string'></xs:attribute><xs:attribute name='Description' type='xs:string'></xs:attribute><xs:attribute name='TransactionType' type='xs:string'></xs:attribute><xs:attribute name='ServicingStatus' type='xs:string'></xs:attribute><xs:attribute name='RushOrder' type='xs:string'></xs:attribute><xs:attribute name='DueDate' type='xs:string'></xs:attribute><xs:attribute name='BusinessPartnerID' type='xs:string'></xs:attribute><xs:attribute name='BusinessPartner' type='xs:string'></xs:attribute><xs:attribute name='ProfileID' type='xs:string'></xs:attribute><xs:attribute name='ProfileDescription' type='xs:string'></xs:attribute><xs:attribute name='FastTrack' type='xs:string'></xs:attribute><xs:attribute name='CreatedDate' type='xs:string'></xs:attribute><xs:attribute name='CreatedBy' type='xs:string'></xs:attribute><xs:attribute name='CompletedDate' type='xs:string'></xs:attribute></xs:complexType></xs:element></xs:schema>";
            public static string XmlDoc = @"<?xml version='1.0' encoding='utf-8'?><ServiceRequest ID='SR00000874' Description='TURNER US - MARTY - Extended Version' TransactionType='Add' ServicingStatus='Completed'  RushOrder='Y' DueDate='07/01/2019' BusinessPartnerID='TURNER' BusinessPartner='Turner Entertainment Networks' ProfileID='PR00000568' ProfileDescription='Turner Broadcasting (ID# 2561) - United States - Deluxe' FastTrack='N'  CreatedDate='10/02/2019' CreatedBy='apereira' CompletedDate='10/07/2019'><Contracts><Contract ID='17'  Description='2019-2022 Turner Pool shell for 19-00040' ServicingStatus='Completed'><Titles><Title ID='MARTY' Description='MARTY'  ServicingStatus='Completed' IPMStatus='Finalized' ContractualDueDate='10/9/2019' LicenseStartDate='10/9/2019' EOPResource='BTHOMAS' PPSResource='BTHOMAS'><LineItems><LineItem ID='0000011750' ServicingStatus='Completed' IPMMedia='IS-PSOD-CU, IS-PSOD-SA, IS-PTV, TV-PSOD-CU, TV-PTV' IPMTerritory='AFGHAN, CHAD, DJIBOU, MAUITA, SOMALI, SSUDAN' IPMLanguage='ARA, FAS, INHIN, INURD' LicenseStart='08/01/2019' LicenseEnd='07/31/2020'><Orders><Order ID='1' ServicingStatus='Completed' Version='TheatricalVersion'><SRDueDate>07/01/2019</SRDueDate><EmbargoDate></EmbargoDate><PrimaryVideo>Y</PrimaryVideo><SecondaryAudio>French Parisian, Music And Effects</SecondaryAudio><SubtitlesFull>Arabic, English, French Parisian</SubtitlesFull><SubtitlesForced> French Parisian</SubtitlesForced><ClosedCaptions></ClosedCaptions><Trailer></Trailer><RebillType>Rebill A</RebillType><Amount></Amount><MetaData></MetaData><ArtWork></ArtWork><Document></Document><EOPPO></EOPPO><EOPDueDate></EOPDueDate><EOPStatus>Not Required</EOPStatus><EOPResource></EOPResource><EOPNotes></EOPNotes><PPSPO>4865863</PPSPO><PPSDueDate>07/01/2019</PPSDueDate><PPSStatus>Completed</PPSStatus><PPSResource></PPSResource><PPSNotes></PPSNotes><MaterialNotes>HD feature file naming conventions should follow the below template: BM-DF-MVXXXXXX.mxf HD trailer filenaming conventions should follow the below template: BR-DF-MVXXXXXX.mxf SD feature file naming conventions should follow the below template: FM-DF-MVXXXXXX.mxf SD trailer filenaming conventions should follow the below template: FR-DF-MVXXXXXX.mxf Here are the individual unique ID #s for each title:  ANNIE HALL MV012999 BLOW OUT MV013001 HOME OF OUR OWN, A MV013000 </MaterialNotes><Other></Other><IPMMedia></IPMMedia><Rejections><Rejection ID='' CurrentStatus='' Asset='' RejectionCode='' Issue='' CommentsHistory=''  RejectedBy='' RejectionDate='' Urgency='' RootCause='' Document=''></Rejection><Redelivered ID='' CurrentStatus='' Asset='' RejectionCode='' Issue='' CommentsHistory=''  RejectedBy='' RejectionDate='' Urgency='' RootCause='' Document=''></Redelivered><Resolved ID='' CurrentStatus='' Asset='' RejectionCode='' Issue='' CommentsHistory=''  RejectedBy='' RejectionDate='' Urgency='' RootCause='' Document=''></Resolved></Rejections></Order><Order ID='2' ServicingStatus='Completed' Version='TheatricalVersion'><SRDueDate>07/01/2019</SRDueDate><EmbargoDate></EmbargoDate><PrimaryVideo>Y</PrimaryVideo><SecondaryAudio>French Parisian, Music And Effects</SecondaryAudio><SubtitlesFull>Arabic, English, French Parisian</SubtitlesFull><SubtitlesForced> French Parisian</SubtitlesForced><ClosedCaptions></ClosedCaptions><Trailer></Trailer><RebillType>Rebill A</RebillType><Amount></Amount><MetaData></MetaData><ArtWork></ArtWork><Document></Document><EOPPO></EOPPO><EOPDueDate></EOPDueDate><EOPStatus>Not Required</EOPStatus><EOPResource></EOPResource><EOPNotes></EOPNotes><PPSPO>4865863</PPSPO><PPSDueDate>07/01/2019</PPSDueDate><PPSStatus>Completed</PPSStatus><PPSResource></PPSResource><PPSNotes></PPSNotes><MaterialNotes>HD feature file naming conventions should follow the below template: BM-DF-MVXXXXXX.mxf HD trailer filenaming conventions should follow the below template: BR-DF-MVXXXXXX.mxf SD feature file naming conventions should follow the below template: FM-DF-MVXXXXXX.mxf SD trailer filenaming conventions should follow the below template: FR-DF-MVXXXXXX.mxf Here are the individual unique ID #s for each title:  ANNIE HALL MV012999 BLOW OUT MV013001 HOME OF OUR OWN, A MV013000 </MaterialNotes><Other></Other><IPMMedia></IPMMedia><Rejections><Rejection ID='' CurrentStatus='' Asset='' RejectionCode='' Issue='' CommentsHistory=''  RejectedBy='' RejectionDate='' Urgency='' RootCause='' Document=''></Rejection><Redelivered ID='' CurrentStatus='' Asset='' RejectionCode='' Issue='' CommentsHistory=''  RejectedBy='' RejectionDate='' Urgency='' RootCause='' Document=''></Redelivered><Resolved ID='' CurrentStatus='' Asset='' RejectionCode='' Issue='' CommentsHistory=''  RejectedBy='' RejectionDate='' Urgency='' RootCause='' Document=''></Resolved></Rejections></Order></Orders></LineItem></LineItems></Title></Titles></Contract></Contracts></ServiceRequest>";

            public static string XmlDocPath = @"C:\Code\MGM\Aws\UnitTests\Mgm.Mss.Data.AutoMapper.UnitTests\TestData\SR00000874.xml";
            public static string XsdDocPath = @"C:\Code\MGM\Aws\UnitTests\Mgm.Mss.Data.AutoMapper.UnitTests\TestData\ServiceRequest_Schema.xsd";
            #endregion
            public static Action<IMapperConfigurationExpression> MapperConfigureMockAction = cfg => cfg.CreateMap<OrderDto, OrderDto>();
            public static List<Action<IMapperConfigurationExpression>> MapperConfigureMockActions;
            static ValidData()
            {
                Initialize();
            }

            private static void Initialize()
            {
                MapperConfigureMockActions = new List<Action<IMapperConfigurationExpression>>();
                MapperConfigureMockActions.Add(MapperConfigureMockAction);
            }
        }
        static class MalformedData
        {
            public static string XmlDocEmpty = @"";
            public static string XmlDocIncorrectIds = @"<?xml version='1.0' encoding='utf-8' ?><ServiceRequest ID='SR00000874hh' Description='TURNER US - MARTY - Extended Version' TransactionType='Add' ServicingStatus='Completed'  RushOrder='Y' DueDate='07/01/2019' BusinessPartnerID='TURNER' BusinessPartner='Turner Entertainment Networks' ProfileID='PR00000568' ProfileDescription='Turner Broadcasting (ID# 2561) - United States - Deluxe' FastTrack='N'  CreatedDate='10/02/2019' CreatedBy='apereira' CompletedDate='10/07/2019'><Contracts><Contract ID='fasdfadsf17'  Description='2019-2022 Turner Pool shell for 19-00040' ServicingStatus='Completed'><Titles><Title ID='MARTY' Description='MARTY'  ServicingStatus='Completed' IPMStatus='Finalized' ContractualDueDate='10/9/2019' LicenseStartDate='10/9/2019' EOPResource='BTHOMAS' PPSResource='BTHOMAS'><LineItems><LineItem ID='0000011750' ServicingStatus='Completed' IPMMedia='IS-PSOD-CU, IS-PSOD-SA, IS-PTV, TV-PSOD-CU, TV-PTV' IPMTerritory='AFGHAN, CHAD, DJIBOU, MAUITA, SOMALI, SSUDAN' IPMLanguage='ARA, FAS, INHIN, INURD' LicenseStart='08/01/2019' LicenseEnd='07/31/2020'><Orders><Order ID='1adsfasdf' ServicingStatus='Completed' Version='TheatricalVersion'><SRDueDate>07/01/2019</SRDueDate><EmbargoDate></EmbargoDate><PrimaryVideo>Y</PrimaryVideo><SecondaryAudio>French Parisian, Music And Effects</SecondaryAudio><SubtitlesFull>Arabic, English, French Parisian</SubtitlesFull><SubtitlesForced> French Parisian</SubtitlesForced><ClosedCaptions></ClosedCaptions><Trailer></Trailer><RebillType>Rebill A</RebillType><Amount></Amount><MetaData></MetaData><ArtWork></ArtWork><Document></Document><EOPPO></EOPPO><EOPDueDate></EOPDueDate><EOPStatus>Not Required</EOPStatus><EOPResource></EOPResource><EOPNotes></EOPNotes><PPSPO>4865863</PPSPO><PPSDueDate>07/01/2019</PPSDueDate><PPSStatus>Completed</PPSStatus><PPSResource></PPSResource><PPSNotes></PPSNotes><MaterialNotes>HD feature file naming conventions should follow the below template: BM-DF-MVXXXXXX.mxf HD trailer filenaming conventions should follow the below template: BR-DF-MVXXXXXX.mxf SD feature file naming conventions should follow the below template: FM-DF-MVXXXXXX.mxf SD trailer filenaming conventions should follow the below template: FR-DF-MVXXXXXX.mxf Here are the individual unique ID #s for each title:  ANNIE HALL MV012999 BLOW OUT MV013001 HOME OF OUR OWN, A MV013000 </MaterialNotes><Other></Other><IPMMedia></IPMMedia><Rejections><Rejection ID='' CurrentStatus='' Asset='' RejectionCode='' Issue='' CommentsHistory=''  RejectedBy='' RejectionDate='' Urgency='' RootCause='' Document=''></Rejection><Redelivered ID='' CurrentStatus='' Asset='' RejectionCode='' Issue='' CommentsHistory=''  RejectedBy='' RejectionDate='' Urgency='' RootCause='' Document=''></Redelivered><Resolved ID='' CurrentStatus='' Asset='' RejectionCode='' Issue='' CommentsHistory=''  RejectedBy='' RejectionDate='' Urgency='' RootCause='' Document=''></Resolved></Rejections></Order><Order ID='2' ServicingStatus='Completed' Version='TheatricalVersion'><SRDueDate>07/01/2019</SRDueDate><EmbargoDate></EmbargoDate><PrimaryVideo>Y</PrimaryVideo><SecondaryAudio>French Parisian, Music And Effects</SecondaryAudio><SubtitlesFull>Arabic, English, French Parisian</SubtitlesFull><SubtitlesForced> French Parisian</SubtitlesForced><ClosedCaptions></ClosedCaptions><Trailer></Trailer><RebillType>Rebill A</RebillType><Amount></Amount><MetaData></MetaData><ArtWork></ArtWork><Document></Document><EOPPO></EOPPO><EOPDueDate></EOPDueDate><EOPStatus>Not Required</EOPStatus><EOPResource></EOPResource><EOPNotes></EOPNotes><PPSPO>4865863</PPSPO><PPSDueDate>07/01/2019</PPSDueDate><PPSStatus>Completed</PPSStatus><PPSResource></PPSResource><PPSNotes></PPSNotes><MaterialNotes>HD feature file naming conventions should follow the below template: BM-DF-MVXXXXXX.mxf HD trailer filenaming conventions should follow the below template: BR-DF-MVXXXXXX.mxf SD feature file naming conventions should follow the below template: FM-DF-MVXXXXXX.mxf SD trailer filenaming conventions should follow the below template: FR-DF-MVXXXXXX.mxf Here are the individual unique ID #s for each title:  ANNIE HALL MV012999 BLOW OUT MV013001 HOME OF OUR OWN, A MV013000 </MaterialNotes><Other></Other><IPMMedia></IPMMedia><Rejections><Rejection ID='' CurrentStatus='' Asset='' RejectionCode='' Issue='' CommentsHistory=''  RejectedBy='' RejectionDate='' Urgency='' RootCause='' Document=''></Rejection><Redelivered ID='' CurrentStatus='' Asset='' RejectionCode='' Issue='' CommentsHistory=''  RejectedBy='' RejectionDate='' Urgency='' RootCause='' Document=''></Redelivered><Resolved ID='' CurrentStatus='' Asset='' RejectionCode='' Issue='' CommentsHistory=''  RejectedBy='' RejectionDate='' Urgency='' RootCause='' Document=''></Resolved></Rejections></Order></Orders></LineItem></LineItems></Title></Titles></Contract></Contracts></ServiceRequest>";
            public static string XmlDocMalformedXml = @"<?xml version='1.0' encoding='utf-8' ?><ServiceRequest ID='74' Description='TURNER US - MARTY - Extended Version' TransactionType='Add' ServicingStatus='Completed'  RushOrder='Y' DueDate='07/01/2019' BusinessPartnerID='TURNER' BusinessPartner='Turner Entertainment Networks' ProfileID='PR00000568' ProfileDescription='Turner Broadcasting (ID# 2561) - United States - Deluxe' FastTrack='N'  CreatedDate='10/02/2019' CreatedBy='apereira' CompletedDate='10/07/2019'><Contracts><Contract ID='17'  Description='2019-2022 Turner Pool shell for 19-00040' ServicingStatus='Completed'><Titles><Title ID='MARTY' Description='MARTY'  ServicingStatus='Completed' IPMStatus='Finalized' ContractualDueDate='10/9/2019' LicenseStartDate='10/9/2019' EOPResource='BTHOMAS' PPSResource='BTHOMAS'><LineItems><LineItem ID='0000011750' ServicingStatus='Completed' IPMMedia='IS-PSOD-CU, IS-PSOD-SA, IS-PTV, TV-PSOD-CU, TV-PTV' IPMTerritory='AFGHAN, CHAD, DJIBOU, MAUITA, SOMALI, SSUDAN' IPMLanguage='ARA, FAS, INHIN, INURD' LicenseStart='08/01/2019' LicenseEnd='07/31/2020'><Orders><Order ID='1' ServicingStatus='Completed' Version='TheatricalVersion'><SRDueDate>07/01/2019</SRDueDate><EmbargoDate></EmbargoDate><PrimaryVideo>Y</PrimaryVideo><SecondaryAudio>French Parisian, Music And Effects</SecondaryAudio></Title></Titles></Contract></Contracts></ServiceRequest>";
            public static string XmlDocMissingAccounts = @"<?xml version='1.0' encoding='utf-8' ?><ServiceRequest ID='74' Description='TURNER US - MARTY - Extended Version' TransactionType='Add' ServicingStatus='Completed'  RushOrder='Y' DueDate='07/01/2019' BusinessPartnerID='TURNER' BusinessPartner='Turner Entertainment Networks' ProfileID='PR00000568' ProfileDescription='Turner Broadcasting (ID# 2561) - United States - Deluxe' FastTrack='N'  CreatedDate='10/02/2019' CreatedBy='apereira' CompletedDate='10/07/2019'></ServiceRequest>";
            public static string XmlDocMissingOrders = @"<?xml version='1.0' encoding='utf-8' ?><ServiceRequest ID='SR00000874' Description='TURNER US - MARTY - Extended Version' TransactionType='Add' ServicingStatus='Completed'  RushOrder='Y' DueDate='07/01/2019' BusinessPartnerID='TURNER' BusinessPartner='Turner Entertainment Networks' ProfileID='PR00000568' ProfileDescription='Turner Broadcasting (ID# 2561) - United States - Deluxe' FastTrack='N'  CreatedDate='10/02/2019' CreatedBy='apereira' CompletedDate='10/07/2019'><Contracts><Contract ID='17'  Description='2019-2022 Turner Pool shell for 19-00040' ServicingStatus='Completed'><Titles><Title ID='MARTY' Description='MARTY'  ServicingStatus='Completed' IPMStatus='Finalized' ContractualDueDate='10/9/2019' LicenseStartDate='10/9/2019' EOPResource='BTHOMAS' PPSResource='BTHOMAS'><LineItems><LineItem ID='0000011750' ServicingStatus='Completed' IPMMedia='IS-PSOD-CU, IS-PSOD-SA, IS-PTV, TV-PSOD-CU, TV-PTV' IPMTerritory='AFGHAN, CHAD, DJIBOU, MAUITA, SOMALI, SSUDAN' IPMLanguage='ARA, FAS, INHIN, INURD' LicenseStart='08/01/2019' LicenseEnd='07/31/2020'></LineItem></LineItems></Title></Titles></Contract></Contracts></ServiceRequest>";
            public static string XmlDocPathFail = @"C:\Code\MGM\Aws\UnitTests\Mgm.Mss.Common.UnitTests\TestData\SR00000874FAIL.xml";

            public static string XsdDocEmpty = @"";
            public static string XsdDocPathFail = @"C:\Code\MGM\Aws\UnitTests\Mgm.Mss.Common.UnitTests\TestData\ServiceRequest_SchemaFAIL.xsd";
        }
        private static MapperConfiguration GetMapperConfiguration()
        {
            MapperConfiguration config;
            try
            {
                config = new MapperConfiguration(ValidData.MapperConfigureMockAction);
            }
            catch
            {
                config = null;
            }

            return config;
        }

        #region Instantiate Class
        [Fact]
        public void Success_Test_Instantiate_Boostrapper_NotNull_MapperNotSet()
        {
            Bootstrapper result = Bootstrapper.New();
            Assert.NotNull(result);
            Assert.False(result.MapperIsSet);
        }
        [Fact]
        public void Success_Test_Instantiate_Boostrapper_NotNull_SetMapper_Action()
        {
            Bootstrapper result = Bootstrapper.New();
            Assert.NotNull(result);
            result.SetMapper(ValidData.MapperConfigureMockAction);
            Assert.True(result.MapperIsSet);
        }
        [Fact]
        public void Success_Test_Instantiate_Boostrapper_NotNull_SetMapper_Configuration()
        {
            Bootstrapper result = Bootstrapper.New();
            Assert.NotNull(result);
            result.SetMapper(GetMapperConfiguration());
            Assert.True(result.MapperIsSet);
        }
        [Fact]
        public void Success_Test_Instantiate_Boostrapper_With_Action_NotNull()
        {
            Bootstrapper result = Bootstrapper.New(ValidData.MapperConfigureMockAction);
            Assert.NotNull(result);
        }
        [Fact]
        public void Success_Test_Instantiate_Boostrapper_With_Config_NotNull()
        {
            Bootstrapper result = Bootstrapper.New(GetMapperConfiguration());
            Assert.NotNull(result);
        }
        [Fact]
        public void Success_Test_Instantiate_Boostrapper_Valid_Configuration()
        {
            var result = Bootstrapper.New();
            
            //config.AssertConfigurationIsValid();
        }
        #endregion Instantiate Class
        [Fact]
        public void Success_Test_CreateDtoMaps()
        {
            var result = Bootstrapper.CreateDtoMaps();
            Assert.NotNull(result);
        }
        [Fact]
        public void Success_Test_ParseXml_Valid_Xml2()
        {
            //var result = ServiceRequestStatusUpdate.ExtractServiceRequest(ValidData.XmlDoc);
            object result = null;
            Assert.NotNull(result);
        }
    }
}
