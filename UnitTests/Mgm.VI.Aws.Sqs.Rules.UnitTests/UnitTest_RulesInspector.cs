///////////////////////////////////////////////////////////////////////////
///   Namespace:      Mgm.VI.Aws.Sqs.UnitTests
///   Author:         Nour Douchi                    Date: 1/15/2019
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
using Mgm.VI.Data.Dto;
using Mgm.VI.Aws.Sqs.Rules;
using System.Collections.Generic;
using Amazon.SQS;
using Mgm.VI.Aws.Sqs.Dto;
using Amazon;
using Amazon.SQS.Model;
using System.Threading.Tasks;
using Mgm.VI.Logger;
using Mgm.VI.Aws;

namespace Mgm.VI.Aws.Sqs.Rules.UnitTests
{
    public class UnitTest_RulesInspector /// WORK ON
    {
        private static readonly string logs = @"C:\Temp\Mgm.VI.Aws.Sqs.Processor\Logs\";
        private static readonly ILoggerService Logger = new LoggerService(LoggingMode.ToDatabaseAndFile, logs);
        static class ValidData
        {
            #region 
            public static string XsdDoc = @"<?xml version='1.0' encoding='UTF-8'?><xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema' elementFormDefault='qualified' attributeFormDefault='unqualified'><xs:element name='ServiceRequest'><xs:complexType><xs:sequence><xs:element name='Contracts'><xs:complexType><xs:sequence><xs:element name='Contract'><xs:complexType><xs:sequence><xs:element name='Titles'><xs:complexType><xs:sequence><xs:element name='Title'><xs:complexType><xs:sequence><xs:element name='LineItems'><xs:complexType><xs:sequence><xs:element name='LineItem'><xs:complexType><xs:sequence><xs:element name='Orders'><xs:complexType><xs:sequence><xs:element name='Order' maxOccurs='unbounded'><xs:complexType><xs:sequence><xs:element name='SRDueDate' type='xs:string'></xs:element><xs:element name='EmbargoDate'></xs:element><xs:element name='PrimaryVideo' type='xs:string'></xs:element><xs:element name='SecondaryAudio' type='xs:string'></xs:element><xs:element name='SubtitlesFull' type='xs:string'></xs:element><xs:element name='SubtitlesForced' type='xs:string'></xs:element><xs:element name='ClosedCaptions'></xs:element><xs:element name='Trailer'></xs:element><xs:element name='RebillType' type='xs:string'></xs:element><xs:element name='Amount'></xs:element><xs:element name='MetaData'></xs:element><xs:element name='ArtWork'></xs:element><xs:element name='Document'></xs:element><xs:element name='EOPPO'></xs:element><xs:element name='EOPDueDate'></xs:element><xs:element name='EOPStatus' type='xs:string'></xs:element><xs:element name='EOPResource'></xs:element><xs:element name='EOPNotes'></xs:element><xs:element name='PPSPO' type='xs:int'></xs:element><xs:element name='PPSDueDate' type='xs:string'></xs:element><xs:element name='PPSStatus' type='xs:string'></xs:element><xs:element name='PPSResource'></xs:element><xs:element name='PPSNotes'></xs:element><xs:element name='MaterialNotes' type='xs:string'></xs:element><xs:element name='Other'></xs:element><xs:element name='IPMMedia'></xs:element><xs:element name='Rejections'><xs:complexType><xs:sequence><xs:element name='Rejection'><xs:complexType><xs:attribute name='ID' type='xs:string'></xs:attribute><xs:attribute name='CurrentStatus' type='xs:string'></xs:attribute><xs:attribute name='Asset' type='xs:string'></xs:attribute><xs:attribute name='RejectionCode' type='xs:string'></xs:attribute><xs:attribute name='Issue' type='xs:string'></xs:attribute><xs:attribute name='CommentsHistory' type='xs:string'></xs:attribute><xs:attribute name='RejectedBy' type='xs:string'></xs:attribute><xs:attribute name='RejectionDate' type='xs:string'></xs:attribute><xs:attribute name='Urgency' type='xs:string'></xs:attribute><xs:attribute name='RootCause' type='xs:string'></xs:attribute><xs:attribute name='Document' type='xs:string'></xs:attribute></xs:complexType></xs:element><xs:element name='Redelivered'><xs:complexType><xs:attribute name='ID' type='xs:string'></xs:attribute><xs:attribute name='CurrentStatus' type='xs:string'></xs:attribute><xs:attribute name='Asset' type='xs:string'></xs:attribute><xs:attribute name='RejectionCode' type='xs:string'></xs:attribute><xs:attribute name='Issue' type='xs:string'></xs:attribute><xs:attribute name='CommentsHistory' type='xs:string'></xs:attribute><xs:attribute name='RejectedBy' type='xs:string'></xs:attribute><xs:attribute name='RejectionDate' type='xs:string'></xs:attribute><xs:attribute name='Urgency' type='xs:string'></xs:attribute><xs:attribute name='RootCause' type='xs:string'></xs:attribute><xs:attribute name='Document' type='xs:string'></xs:attribute></xs:complexType></xs:element><xs:element name='Resolved'><xs:complexType><xs:attribute name='ID' type='xs:string'></xs:attribute><xs:attribute name='CurrentStatus' type='xs:string'></xs:attribute><xs:attribute name='Asset' type='xs:string'></xs:attribute><xs:attribute name='RejectionCode' type='xs:string'></xs:attribute><xs:attribute name='Issue' type='xs:string'></xs:attribute><xs:attribute name='CommentsHistory' type='xs:string'></xs:attribute><xs:attribute name='RejectedBy' type='xs:string'></xs:attribute><xs:attribute name='RejectionDate' type='xs:string'></xs:attribute><xs:attribute name='Urgency' type='xs:string'></xs:attribute><xs:attribute name='RootCause' type='xs:string'></xs:attribute><xs:attribute name='Document' type='xs:string'></xs:attribute></xs:complexType></xs:element></xs:sequence></xs:complexType></xs:element></xs:sequence><xs:attribute name='ID' type='xs:int'></xs:attribute><xs:attribute name='ServicingStatus' type='xs:string'></xs:attribute><xs:attribute name='Version' type='xs:string'></xs:attribute></xs:complexType></xs:element></xs:sequence></xs:complexType></xs:element></xs:sequence><xs:attribute name='ID' type='xs:int'></xs:attribute><xs:attribute name='ServicingStatus' type='xs:string'></xs:attribute><xs:attribute name='IPMMedia' type='xs:string'></xs:attribute><xs:attribute name='IPMTerritory' type='xs:string'></xs:attribute><xs:attribute name='IPMLanguage' type='xs:string'></xs:attribute><xs:attribute name='LicenseStart' type='xs:string'></xs:attribute><xs:attribute name='LicenseEnd' type='xs:string'></xs:attribute></xs:complexType></xs:element></xs:sequence></xs:complexType></xs:element></xs:sequence><xs:attribute name='ID' type='xs:string'></xs:attribute><xs:attribute name='Description' type='xs:string'></xs:attribute><xs:attribute name='ServicingStatus' type='xs:string'></xs:attribute><xs:attribute name='IPMStatus' type='xs:string'></xs:attribute><xs:attribute name='ContractualDueDate' type='xs:string'></xs:attribute><xs:attribute name='LicenseStartDate' type='xs:string'></xs:attribute><xs:attribute name='EOPResource' type='xs:string'></xs:attribute><xs:attribute name='PPSResource' type='xs:string'></xs:attribute></xs:complexType></xs:element></xs:sequence></xs:complexType></xs:element></xs:sequence><xs:attribute name='ID' type='xs:int'></xs:attribute><xs:attribute name='Description' type='xs:string'></xs:attribute><xs:attribute name='ServicingStatus' type='xs:string'></xs:attribute></xs:complexType></xs:element></xs:sequence></xs:complexType></xs:element></xs:sequence><xs:attribute name='ID' type='xs:string'></xs:attribute><xs:attribute name='Description' type='xs:string'></xs:attribute><xs:attribute name='TransactionType' type='xs:string'></xs:attribute><xs:attribute name='ServicingStatus' type='xs:string'></xs:attribute><xs:attribute name='RushOrder' type='xs:string'></xs:attribute><xs:attribute name='DueDate' type='xs:string'></xs:attribute><xs:attribute name='BusinessPartnerID' type='xs:string'></xs:attribute><xs:attribute name='BusinessPartner' type='xs:string'></xs:attribute><xs:attribute name='ProfileID' type='xs:string'></xs:attribute><xs:attribute name='ProfileDescription' type='xs:string'></xs:attribute><xs:attribute name='FastTrack' type='xs:string'></xs:attribute><xs:attribute name='CreatedDate' type='xs:string'></xs:attribute><xs:attribute name='CreatedBy' type='xs:string'></xs:attribute><xs:attribute name='CompletedDate' type='xs:string'></xs:attribute></xs:complexType></xs:element></xs:schema>";
            public static string XmlDoc = @"<?xml version='1.0' encoding='utf-8'?><ServiceRequest ID='SR00000874' Description='TURNER US - MARTY - Extended Version' TransactionType='Add' ServicingStatus='Completed'  RushOrder='Y' DueDate='07/01/2019' BusinessPartnerID='TURNER' BusinessPartner='Turner Entertainment Networks' ProfileID='PR00000568' ProfileDescription='Turner Broadcasting (ID# 2561) - United States - Deluxe' FastTrack='N'  CreatedDate='10/02/2019' CreatedBy='apereira' CompletedDate='10/07/2019'><Contracts><Contract ID='17'  Description='2019-2022 Turner Pool shell for 19-00040' ServicingStatus='Completed'><Titles><Title ID='MARTY' Description='MARTY'  ServicingStatus='Completed' IPMStatus='Finalized' ContractualDueDate='10/9/2019' LicenseStartDate='10/9/2019' EOPResource='BTHOMAS' PPSResource='BTHOMAS'><LineItems><LineItem ID='0000011750' ServicingStatus='Completed' IPMMedia='IS-PSOD-CU, IS-PSOD-SA, IS-PTV, TV-PSOD-CU, TV-PTV' IPMTerritory='AFGHAN, CHAD, DJIBOU, MAUITA, SOMALI, SSUDAN' IPMLanguage='ARA, FAS, INHIN, INURD' LicenseStart='08/01/2019' LicenseEnd='07/31/2020'><Orders><Order ID='1' ServicingStatus='Completed' Version='TheatricalVersion'><VendorId>Vubiquity</VendorId><SourceMioAssetId>TestSourceMioAssetId</SourceMioAssetId><ShellMioAssetId>TestShellMioAssetId</ShellMioAssetId><SRDueDate>07/01/2019</SRDueDate><EmbargoDate></EmbargoDate><PrimaryVideo>Y</PrimaryVideo><SecondaryAudio>French Parisian, Music And Effects</SecondaryAudio><SubtitlesFull>Arabic, English, French Parisian</SubtitlesFull><SubtitlesForced> French Parisian</SubtitlesForced><ClosedCaptions></ClosedCaptions><Trailer></Trailer><RebillType>Rebill A</RebillType><Amount></Amount><MetaData></MetaData><ArtWork></ArtWork><Document></Document><EOPPO></EOPPO><EOPDueDate></EOPDueDate><EOPStatus>Not Required</EOPStatus><EOPResource></EOPResource><EOPNotes></EOPNotes><PPSPO>4865863</PPSPO><PPSDueDate>07/01/2019</PPSDueDate><PPSStatus>Completed</PPSStatus><PPSResource></PPSResource><PPSNotes></PPSNotes><MaterialNotes>HD feature file naming conventions should follow the below template: BM-DF-MVXXXXXX.mxf HD trailer filenaming conventions should follow the below template: BR-DF-MVXXXXXX.mxf SD feature file naming conventions should follow the below template: FM-DF-MVXXXXXX.mxf SD trailer filenaming conventions should follow the below template: FR-DF-MVXXXXXX.mxf Here are the individual unique ID #s for each title:  ANNIE HALL MV012999 BLOW OUT MV013001 HOME OF OUR OWN, A MV013000 </MaterialNotes><Other></Other><IPMMedia></IPMMedia><Rejections><Rejection ID='' CurrentStatus='' Asset='' RejectionCode='' Issue='' CommentsHistory=''  RejectedBy='' RejectionDate='' Urgency='' RootCause='' Document=''></Rejection><Redelivered ID='' CurrentStatus='' Asset='' RejectionCode='' Issue='' CommentsHistory=''  RejectedBy='' RejectionDate='' Urgency='' RootCause='' Document=''></Redelivered><Resolved ID='' CurrentStatus='' Asset='' RejectionCode='' Issue='' CommentsHistory=''  RejectedBy='' RejectionDate='' Urgency='' RootCause='' Document=''></Resolved></Rejections></Order><Order ID='2' ServicingStatus='Completed' Version='TheatricalVersion'><VendorId>Vubiquity</VendorId><SourceMioAssetId>TestSourceMioAssetId</SourceMioAssetId><ShellMioAssetId>TestShellMioAssetId</ShellMioAssetId><SRDueDate>07/01/2019</SRDueDate><EmbargoDate></EmbargoDate><PrimaryVideo>Y</PrimaryVideo><SecondaryAudio>French Parisian, Music And Effects</SecondaryAudio><SubtitlesFull>Arabic, English, French Parisian</SubtitlesFull><SubtitlesForced> French Parisian</SubtitlesForced><ClosedCaptions></ClosedCaptions><Trailer></Trailer><RebillType>Rebill A</RebillType><Amount></Amount><MetaData></MetaData><ArtWork></ArtWork><Document></Document><EOPPO></EOPPO><EOPDueDate></EOPDueDate><EOPStatus>Not Required</EOPStatus><EOPResource></EOPResource><EOPNotes></EOPNotes><PPSPO>4865863</PPSPO><PPSDueDate>07/01/2019</PPSDueDate><PPSStatus>Completed</PPSStatus><PPSResource></PPSResource><PPSNotes></PPSNotes><MaterialNotes>HD feature file naming conventions should follow the below template: BM-DF-MVXXXXXX.mxf HD trailer filenaming conventions should follow the below template: BR-DF-MVXXXXXX.mxf SD feature file naming conventions should follow the below template: FM-DF-MVXXXXXX.mxf SD trailer filenaming conventions should follow the below template: FR-DF-MVXXXXXX.mxf Here are the individual unique ID #s for each title:  ANNIE HALL MV012999 BLOW OUT MV013001 HOME OF OUR OWN, A MV013000 </MaterialNotes><Other></Other><IPMMedia></IPMMedia><Rejections><Rejection ID='' CurrentStatus='' Asset='' RejectionCode='' Issue='' CommentsHistory=''  RejectedBy='' RejectionDate='' Urgency='' RootCause='' Document=''></Rejection><Redelivered ID='' CurrentStatus='' Asset='' RejectionCode='' Issue='' CommentsHistory=''  RejectedBy='' RejectionDate='' Urgency='' RootCause='' Document=''></Redelivered><Resolved ID='' CurrentStatus='' Asset='' RejectionCode='' Issue='' CommentsHistory=''  RejectedBy='' RejectionDate='' Urgency='' RootCause='' Document=''></Resolved></Rejections></Order></Orders></LineItem></LineItems></Title></Titles></Contract></Contracts></ServiceRequest>";

            public static string XmlDocPath = @"C:\Code\Mgm.VI\UnitTests\Mgm.VI.Aws.Sqs.UnitTests\TestData\SR00000874.xml";
            public static string XsdDocPath = @"C:\Code\Mgm.VI\UnitTests\Mgm.VI.Aws.Sqs.UnitTests\TestData\ServiceRequest_Schema.xsd";

            public static SqsQueueDto _deadLetterQueue = new SqsQueueDto("ServiceRequestTestDeadLetter.fifo", "https://sqs.us-west-1.amazonaws.com/073897193669/ServiceRequestTestDeadLetter.fifo", QType.Fifo);
            public static SqsQueueDto _messageQueue = new SqsQueueDto("ServiceRequestTest.fifo", "https://sqs.us-west-1.amazonaws.com/073897193669/ServiceRequestTest.fifo", QType.Fifo, _deadLetterQueue);
            public static SqsQueueDto _disabledContentBasedDuplicationMessageQueue = new SqsQueueDto("DisabledServiceRequestTest.fifo", "https://sqs.us-west-1.amazonaws.com/073897193669/DisabledServiceRequestTest.fifo", QType.Fifo, _deadLetterQueue);

            public static string _vendorId = "vubiquity"; // Get this from a configuration file
                                                          //private static IConfiguration Configuration;
                                                          //private static System.Configuration.Configuration MyConfiguration;
            public static AwsUser _awsUser = new AwsUser()
            {
                AccessKey = "AKIARCNE5WDC55GZE4N5",
                SecretKey = "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"
            };
            public static AmazonSQSConfig _sqsConfig = new AmazonSQSConfig()
            {
                ServiceURL = "https://sqs.us-west-1.amazonaws.com",
                RegionEndpoint = RegionEndpoint.USWest1
            };
            public static IVendorDto vendorDtoTemp = new VendorDto
            {
                StagingId = 0,
                VendorId = "TestVendorId",
                VendorName = "Test Vendor, Inc.",
                ServiceRequestApiURI = "",
                SQS_StatusUpdatePrimaryURI = "",
                SQS_StatusUpdateDeadLetterURI = "",
                SQS_ServiceURL = "https://sqs.us-west-1.amazonaws.com",
                SQS_RegionEndPoint = "RegionEndpoint.USWest1",
                AWS_AccessKey = "AKIARCNE5WDC55GZE4N5",
                AWS_SecretKey = "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"
            };
            public static string xsdFilePath = @"C:\Code\Mgm.VI\UnitTestsMgm.VI.Aws.Sqs.Processor\Data\ServiceRequest_Schema.xsd";
            public static IRulesInspector _rulesInspector;
            public static List<IRule> _rules;
            public static List<IVendorDto> vendorDtos;
            public static IVendorDto vendorDto;

            public static string _messageGroupId = String.Empty;
            public static bool _queueWithMessageGroupId = false;
            #endregion
        }
        static class InvalidData
        {
            #region 
            public static string XsdDoc = @"<?xml version='1.0' encoding='UTF-8'?><xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema' elementFormDefault='qualified' attributeFormDefault='unqualified'><xs:element name='ServiceRequest'><xs:complexType><xs:sequence><xs:element name='Contracts'><xs:complexType><xs:sequence><xs:element name='Contract'><xs:complexType><xs:sequence><xs:element name='Titles'><xs:complexType><xs:sequence><xs:element name='Title'><xs:complexType><xs:sequence><xs:element name='LineItems'><xs:complexType><xs:sequence><xs:element name='LineItem'><xs:complexType><xs:sequence><xs:element name='Orders'><xs:complexType><xs:sequence><xs:element name='Order' maxOccurs='unbounded'><xs:complexType><xs:sequence><xs:element name='SRDueDate' type='xs:string'></xs:element><xs:element name='EmbargoDate'></xs:element><xs:element name='PrimaryVideo' type='xs:string'></xs:element><xs:element name='SecondaryAudio' type='xs:string'></xs:element><xs:element name='SubtitlesFull' type='xs:string'></xs:element><xs:element name='SubtitlesForced' type='xs:string'></xs:element><xs:element name='ClosedCaptions'></xs:element><xs:element name='Trailer'></xs:element><xs:element name='RebillType' type='xs:string'></xs:element><xs:element name='Amount'></xs:element><xs:element name='MetaData'></xs:element><xs:element name='ArtWork'></xs:element><xs:element name='Document'></xs:element><xs:element name='EOPPO'></xs:element><xs:element name='EOPDueDate'></xs:element><xs:element name='EOPStatus' type='xs:string'></xs:element><xs:element name='EOPResource'></xs:element><xs:element name='EOPNotes'></xs:element><xs:element name='PPSPO' type='xs:int'></xs:element><xs:element name='PPSDueDate' type='xs:string'></xs:element><xs:element name='PPSStatus' type='xs:string'></xs:element><xs:element name='PPSResource'></xs:element><xs:element name='PPSNotes'></xs:element><xs:element name='MaterialNotes' type='xs:string'></xs:element><xs:element name='Other'></xs:element><xs:element name='IPMMedia'></xs:element><xs:element name='Rejections'><xs:complexType><xs:sequence><xs:element name='Rejection'><xs:complexType><xs:attribute name='ID' type='xs:string'></xs:attribute><xs:attribute name='CurrentStatus' type='xs:string'></xs:attribute><xs:attribute name='Asset' type='xs:string'></xs:attribute><xs:attribute name='RejectionCode' type='xs:string'></xs:attribute><xs:attribute name='Issue' type='xs:string'></xs:attribute><xs:attribute name='CommentsHistory' type='xs:string'></xs:attribute><xs:attribute name='RejectedBy' type='xs:string'></xs:attribute><xs:attribute name='RejectionDate' type='xs:string'></xs:attribute><xs:attribute name='Urgency' type='xs:string'></xs:attribute><xs:attribute name='RootCause' type='xs:string'></xs:attribute><xs:attribute name='Document' type='xs:string'></xs:attribute></xs:complexType></xs:element><xs:element name='Redelivered'><xs:complexType><xs:attribute name='ID' type='xs:string'></xs:attribute><xs:attribute name='CurrentStatus' type='xs:string'></xs:attribute><xs:attribute name='Asset' type='xs:string'></xs:attribute><xs:attribute name='RejectionCode' type='xs:string'></xs:attribute><xs:attribute name='Issue' type='xs:string'></xs:attribute><xs:attribute name='CommentsHistory' type='xs:string'></xs:attribute><xs:attribute name='RejectedBy' type='xs:string'></xs:attribute><xs:attribute name='RejectionDate' type='xs:string'></xs:attribute><xs:attribute name='Urgency' type='xs:string'></xs:attribute><xs:attribute name='RootCause' type='xs:string'></xs:attribute><xs:attribute name='Document' type='xs:string'></xs:attribute></xs:complexType></xs:element><xs:element name='Resolved'><xs:complexType><xs:attribute name='ID' type='xs:string'></xs:attribute><xs:attribute name='CurrentStatus' type='xs:string'></xs:attribute><xs:attribute name='Asset' type='xs:string'></xs:attribute><xs:attribute name='RejectionCode' type='xs:string'></xs:attribute><xs:attribute name='Issue' type='xs:string'></xs:attribute><xs:attribute name='CommentsHistory' type='xs:string'></xs:attribute><xs:attribute name='RejectedBy' type='xs:string'></xs:attribute><xs:attribute name='RejectionDate' type='xs:string'></xs:attribute><xs:attribute name='Urgency' type='xs:string'></xs:attribute><xs:attribute name='RootCause' type='xs:string'></xs:attribute><xs:attribute name='Document' type='xs:string'></xs:attribute></xs:complexType></xs:element></xs:sequence></xs:complexType></xs:element></xs:sequence><xs:attribute name='ID' type='xs:int'></xs:attribute><xs:attribute name='ServicingStatus' type='xs:string'></xs:attribute><xs:attribute name='Version' type='xs:string'></xs:attribute></xs:complexType></xs:element></xs:sequence></xs:complexType></xs:element></xs:sequence><xs:attribute name='ID' type='xs:int'></xs:attribute><xs:attribute name='ServicingStatus' type='xs:string'></xs:attribute><xs:attribute name='IPMMedia' type='xs:string'></xs:attribute><xs:attribute name='IPMTerritory' type='xs:string'></xs:attribute><xs:attribute name='IPMLanguage' type='xs:string'></xs:attribute><xs:attribute name='LicenseStart' type='xs:string'></xs:attribute><xs:attribute name='LicenseEnd' type='xs:string'></xs:attribute></xs:complexType></xs:element></xs:sequence></xs:complexType></xs:element></xs:sequence><xs:attribute name='ID' type='xs:string'></xs:attribute><xs:attribute name='Description' type='xs:string'></xs:attribute><xs:attribute name='ServicingStatus' type='xs:string'></xs:attribute><xs:attribute name='IPMStatus' type='xs:string'></xs:attribute><xs:attribute name='ContractualDueDate' type='xs:string'></xs:attribute><xs:attribute name='LicenseStartDate' type='xs:string'></xs:attribute><xs:attribute name='EOPResource' type='xs:string'></xs:attribute><xs:attribute name='PPSResource' type='xs:string'></xs:attribute></xs:complexType></xs:element></xs:sequence></xs:complexType></xs:element></xs:sequence><xs:attribute name='ID' type='xs:int'></xs:attribute><xs:attribute name='Description' type='xs:string'></xs:attribute><xs:attribute name='ServicingStatus' type='xs:string'></xs:attribute></xs:complexType></xs:element></xs:sequence></xs:complexType></xs:element></xs:sequence><xs:attribute name='ID' type='xs:string'></xs:attribute><xs:attribute name='Description' type='xs:string'></xs:attribute><xs:attribute name='TransactionType' type='xs:string'></xs:attribute><xs:attribute name='ServicingStatus' type='xs:string'></xs:attribute><xs:attribute name='RushOrder' type='xs:string'></xs:attribute><xs:attribute name='DueDate' type='xs:string'></xs:attribute><xs:attribute name='BusinessPartnerID' type='xs:string'></xs:attribute><xs:attribute name='BusinessPartner' type='xs:string'></xs:attribute><xs:attribute name='ProfileID' type='xs:string'></xs:attribute><xs:attribute name='ProfileDescription' type='xs:string'></xs:attribute><xs:attribute name='FastTrack' type='xs:string'></xs:attribute><xs:attribute name='CreatedDate' type='xs:string'></xs:attribute><xs:attribute name='CreatedBy' type='xs:string'></xs:attribute><xs:attribute name='CompletedDate' type='xs:string'></xs:attribute></xs:complexType></xs:element></xs:schema>";
            public static string XmlDoc = @"<?xml version='1.0' encoding='utf-8'?><ServiceRequest ID='SR00000874' Description='TURNER US - MARTY - Extended Version' TransactionType='Add' ServicingStatus='Completed'  RushOrder='Y' DueDate='07/01/2019' BusinessPartnerID='TURNER' BusinessPartner='Turner Entertainment Networks' ProfileID='PR00000568' ProfileDescription='Turner Broadcasting (ID# 2561) - United States - Deluxe' FastTrack='N'  CreatedDate='10/02/2019' CreatedBy='apereira' CompletedDate='10/07/2019'><Contracts><Contract ID='17'  Description='2019-2022 Turner Pool shell for 19-00040' ServicingStatus='Completed'><Titles><Title ID='MARTY' Description='MARTY'  ServicingStatus='Completed' IPMStatus='Finalized' ContractualDueDate='10/9/2019' LicenseStartDate='10/9/2019' EOPResource='BTHOMAS' PPSResource='BTHOMAS'><LineItems><LineItem ID='0000011750' ServicingStatus='Completed' IPMMedia='IS-PSOD-CU, IS-PSOD-SA, IS-PTV, TV-PSOD-CU, TV-PTV' IPMTerritory='AFGHAN, CHAD, DJIBOU, MAUITA, SOMALI, SSUDAN' IPMLanguage='ARA, FAS, INHIN, INURD' LicenseStart='08/01/2019' LicenseEnd='07/31/2020'><Orders><Order ID='1' ServicingStatus='Completed' Version='TheatricalVersion'><VendorId>Vubiquity</VendorId><SourceMioAssetId>TestSourceMioAssetId</SourceMioAssetId><ShellMioAssetId>TestShellMioAssetId</ShellMioAssetId><SRDueDate>07/01/2019</SRDueDate><EmbargoDate></EmbargoDate><PrimaryVideo>Y</PrimaryVideo><SecondaryAudio>French Parisian, Music And Effects</SecondaryAudio><SubtitlesFull>Arabic, English, French Parisian</SubtitlesFull><SubtitlesForced> French Parisian</SubtitlesForced><ClosedCaptions></ClosedCaptions><Trailer></Trailer><RebillType>Rebill A</RebillType><Amount></Amount><MetaData></MetaData><ArtWork></ArtWork><Document></Document><EOPPO></EOPPO><EOPDueDate></EOPDueDate><EOPStatus>Not Required</EOPStatus><EOPResource></EOPResource><EOPNotes></EOPNotes><PPSPO>4865863</PPSPO><PPSDueDate>07/01/2019</PPSDueDate><PPSStatus>Completed</PPSStatus><PPSResource></PPSResource><PPSNotes></PPSNotes><MaterialNotes>HD feature file naming conventions should follow the below template: BM-DF-MVXXXXXX.mxf HD trailer filenaming conventions should follow the below template: BR-DF-MVXXXXXX.mxf SD feature file naming conventions should follow the below template: FM-DF-MVXXXXXX.mxf SD trailer filenaming conventions should follow the below template: FR-DF-MVXXXXXX.mxf Here are the individual unique ID #s for each title:  ANNIE HALL MV012999 BLOW OUT MV013001 HOME OF OUR OWN, A MV013000 </MaterialNotes><Other></Other><IPMMedia></IPMMedia><Rejections><Rejection ID='' CurrentStatus='' Asset='' RejectionCode='' Issue='' CommentsHistory=''  RejectedBy='' RejectionDate='' Urgency='' RootCause='' Document=''></Rejection><Redelivered ID='' CurrentStatus='' Asset='' RejectionCode='' Issue='' CommentsHistory=''  RejectedBy='' RejectionDate='' Urgency='' RootCause='' Document=''></Redelivered><Resolved ID='' CurrentStatus='' Asset='' RejectionCode='' Issue='' CommentsHistory=''  RejectedBy='' RejectionDate='' Urgency='' RootCause='' Document=''></Resolved></Rejections></Order><Order ID='2' ServicingStatus='Completed' Version='TheatricalVersion'><VendorId>Vubiquity</VendorId><SourceMioAssetId>TestSourceMioAssetId</SourceMioAssetId><ShellMioAssetId>TestShellMioAssetId</ShellMioAssetId><SRDueDate>07/01/2019</SRDueDate><EmbargoDate></EmbargoDate><PrimaryVideo>Y</PrimaryVideo><SecondaryAudio>French Parisian, Music And Effects</SecondaryAudio><SubtitlesFull>Arabic, English, French Parisian</SubtitlesFull><SubtitlesForced> French Parisian</SubtitlesForced><ClosedCaptions></ClosedCaptions><Trailer></Trailer><RebillType>Rebill A</RebillType><Amount></Amount><MetaData></MetaData><ArtWork></ArtWork><Document></Document><EOPPO></EOPPO><EOPDueDate></EOPDueDate><EOPStatus>Not Required</EOPStatus><EOPResource></EOPResource><EOPNotes></EOPNotes><PPSPO>4865863</PPSPO><PPSDueDate>07/01/2019</PPSDueDate><PPSStatus>Completed</PPSStatus><PPSResource></PPSResource><PPSNotes></PPSNotes><MaterialNotes>HD feature file naming conventions should follow the below template: BM-DF-MVXXXXXX.mxf HD trailer filenaming conventions should follow the below template: BR-DF-MVXXXXXX.mxf SD feature file naming conventions should follow the below template: FM-DF-MVXXXXXX.mxf SD trailer filenaming conventions should follow the below template: FR-DF-MVXXXXXX.mxf Here are the individual unique ID #s for each title:  ANNIE HALL MV012999 BLOW OUT MV013001 HOME OF OUR OWN, A MV013000 </MaterialNotes><Other></Other><IPMMedia></IPMMedia><Rejections><Rejection ID='' CurrentStatus='' Asset='' RejectionCode='' Issue='' CommentsHistory=''  RejectedBy='' RejectionDate='' Urgency='' RootCause='' Document=''></Rejection><Redelivered ID='' CurrentStatus='' Asset='' RejectionCode='' Issue='' CommentsHistory=''  RejectedBy='' RejectionDate='' Urgency='' RootCause='' Document=''></Redelivered><Resolved ID='' CurrentStatus='' Asset='' RejectionCode='' Issue='' CommentsHistory=''  RejectedBy='' RejectionDate='' Urgency='' RootCause='' Document=''></Resolved></Rejections></Order></Orders></LineItem></LineItems></Title></Titles></Contract></Contracts></ServiceRequest>";

            public static string XmlDocPath = @"C:\Code\Mgm.VI\UnitTests\Mgm.VI.Aws.Sqs.UnitTests\TestData\NoSuchFile.xml";
            public static string XsdDocPath = @"C:\Code\Mgm.VI\UnitTests\Mgm.VI.Aws.Sqs.UnitTests\TestData\NoSuchFile.xsd";

            public static SqsQueueDto _nonExistingQueue = new SqsQueueDto("NoSuchQueue.fifo", "https://sqs.us-west-1.amazonaws.com/073897193669/NoSuchQueue.fifo", QType.Fifo);
            public static SqsQueueDto _falseQueueNameQueue = new SqsQueueDto("FalseQueueName.fifo", "https://sqs.us-west-1.amazonaws.com/073897193669/ServiceRequestTest.fifo", QType.Fifo);
            public static SqsQueueDto _falseQueuePathQueue = new SqsQueueDto("ServiceRequestTest.fifo", "https://sqs.us-west-1.amazonaws.com/073897193669/FalseQueuePath.fifo", QType.Fifo);

            public static string _vendorId = "noSuchVendor";
            public static AwsUser _awsUserWithBadAccessKey = new AwsUser()
            {
                AccessKey = "BadBadAccessKey",
                SecretKey = "JtJuebWTer7a/ZJ8d66aohIDdXZAmYw3Ipls4/Zi"
            };
            public static AwsUser _awsUserWithBadSecretKey = new AwsUser()
            {
                AccessKey = "AKIARCNE5WDC55GZE4N5",
                SecretKey = "BadSecretKey"
            };
            public static AmazonSQSConfig _sqsConfigWithBadServiceUrl = new AmazonSQSConfig()
            {
                ServiceURL = "https://sqs.us-west-1.amazonaws.com.BadBad",
                RegionEndpoint = RegionEndpoint.USWest1
            };
            public static AmazonSQSConfig _sqsConfigWithBadRegionEndPoint = new AmazonSQSConfig()
            {
                ServiceURL = "https://sqs.us-west-1.amazonaws.com",
                RegionEndpoint = RegionEndpoint.USGovCloudEast1
            };
            public static IVendorDto vendorDtoTemp = new VendorDto
            {
                StagingId = -1,
                VendorId = "BadVendorId",
                VendorName = "Bad Vendor, Inc.",
                ServiceRequestApiURI = "",
                SQS_StatusUpdatePrimaryURI = "",
                SQS_StatusUpdateDeadLetterURI = "",
                SQS_ServiceURL = "https://sqs.us-west-1.amazonaws.com.BadBad",
                SQS_RegionEndPoint = "RegionEndpoint.BadPoint",
                AWS_AccessKey = "BadAccessKey",
                AWS_SecretKey = "BadSecretKey"
            };
            public static string xsdFilePathBad = @"C:\Code\Mgm.VI\UnitTestsMgm.VI.Aws.Sqs.Processor\Data\BadBad.xsd";
            #endregion
        }
        private static IQueueAccess ConstructQueueAccess(bool isDisabledContentBasedDuplication = false)
        {
            IQueueAccess qa = new QueueAccess(ValidData._awsUser,
                                              ValidData._sqsConfig,
                                              isDisabledContentBasedDuplication? 
                                                    ValidData._disabledContentBasedDuplicationMessageQueue : 
                                                    ValidData._messageQueue,
                                              ValidData._deadLetterQueue,
                                              ValidData._queueWithMessageGroupId,
                                              ValidData._messageGroupId,
                                              Logger,
                                              ValidData._rulesInspector);
            return qa;
        }
        private static Task<bool> SendMessage(IQueueAccess qa, string messageGroupId, string messageBody)
        {
            Message message = new Message() { Body = messageBody };
            var sendMessage = qa.SendMessage(qa.MessageQueue, message, messageGroupId);
            while (!sendMessage.IsCompleted)
            {
                System.Threading.Thread.Sleep(500);
            }
            return sendMessage;
        }
        private static Task<Message> GetNextMessage(IQueueAccess qa, bool logAndRequeueUnreadableMessages = true)
        {
            var message = qa.GetNextMessage(qa.MessageQueue, logAndRequeueUnreadableMessages);
            while (!message.IsCompleted)
            {
                System.Threading.Thread.Sleep(500);
            }
            return message;
        }
        #region Success
        [Fact]
        public void Success_Test_Instantiate_Valid_QueueAccess()
        {
            IQueueAccess qa = ConstructQueueAccess();
            Assert.True(qa != null, "QueueAccess:new(...) From Success_Test_Instantiate_Valid_QueueAccess Successful Test Passed.");
        }
        [Fact]
        public void Success_Test_SendMessage()
        {
            IQueueAccess qa = ConstructQueueAccess();
            Task<bool> sendMessage = SendMessage(qa, "JustTestingGroupId1234", "Success_Test_SendMessage");
            Assert.True(sendMessage.Result, "QueueAccess:SendMessage(...) Successful Test Passed.");
        }
        [Fact]
        public void Success_Test_GetNextMessage()
        {
            IQueueAccess qa = ConstructQueueAccess();
            Task<bool> sendMessage = SendMessage(qa, "JustTestingGroupId1234", "Success_Test_SendMessage"); //To ensure that, at least, one message exists
            if (sendMessage.Result)
            {
                Task<Message> message = GetNextMessage(qa, true);
                Assert.True(!string.IsNullOrEmpty(message.Result.MessageId), "QueueAccess:GetNextMessage(...) Successful Test Passed.");
            }
            else
            {
                Assert.True(sendMessage.Result, "QueueAccess:GetNextMessage(...) Failed because a setup condition (SendMessage) didn't complete.");
            }
        }
        #endregion Success

        #region Failure 
        [Fact]
        public void Failure_Test_For_Disabled_Content_Based_Duplication_Exception()
        {
            IQueueAccess qa = ConstructQueueAccess(true);
            qa.SuppressExceptions = false;
            Message message = new Message() { Body = "Failure_Test_For_Disabled_Content_Based_Duplication_Exception" };
            //Task<bool> sendMessage = SendMessage(qa, "JustTestingGroupId1234", "Failure_Test_For_Disabled_Content_Based_Duplication");
            //if (qa.ErrorMessages.errorMessages.Count > 0)
            //{ }
            var result = Assert.ThrowsAsync<AmazonSQSException>(() =>
                            SendMessage(qa, "JustTestingGroupId1234", "Failure_Test_For_Disabled_Content_Based_Duplication"));
            Assert.NotNull(result);
        }
        [Fact]
        public void Failure_Test_For_Disabled_Content_Based_Duplication()
        {
            IQueueAccess qa = ConstructQueueAccess(true);
            Task<bool> sendMessage = SendMessage(qa, "JustTestingGroupId1234", "Failure_Test_For_Disabled_Content_Based_Duplication");
            Assert.False(sendMessage.Result, "QueueAccess:SendMessage(...) from Failure_Test_For_Disabled_Content_Based_Duplication  Test Passed.");
        }
        #endregion Failure 
    }
}
