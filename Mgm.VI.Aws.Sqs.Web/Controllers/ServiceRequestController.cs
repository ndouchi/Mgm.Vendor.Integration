using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mgm.VI.Aws.Sqs.Models;
using Microsoft.Extensions.Options;
using Mgm.VI.Aws.Sqs.Helpers;
using Amazon;
using Mgm.VI.Facade;
using Mgm.VI.Common;
using System.Xml.Linq;

namespace Mgm.VI.Aws.Sqs.Web.Controllers
{
    public class ServiceRequestController : Controller
    {
        static string xmlString = @"<?xml version='1.0' encoding='utf-8'?><ServiceRequest ID='SR00000874' Description='TURNER US - MARTY - Extended Version' TransactionType='Add' ServicingStatus='Completed'  RushOrder='Y' DueDate='07/01/2019' BusinessPartnerID='TURNER' BusinessPartner='Turner Entertainment Networks' ProfileID='PR00000568' ProfileDescription='Turner Broadcasting (ID# 2561) - United States - Deluxe' FastTrack='N'  CreatedDate='10/02/2019' CreatedBy='apereira' CompletedDate='10/07/2019'><Contracts><Contract ID='17'  Description='2019-2022 Turner Pool shell for 19-00040' ServicingStatus='Completed'><Titles><Title ID='MARTY' Description='MARTY'  ServicingStatus='Completed' IPMStatus='Finalized' ContractualDueDate='10/9/2019' LicenseStartDate='10/9/2019' EOPResource='BTHOMAS' PPSResource='BTHOMAS'><LineItems><LineItem ID='0000011750' ServicingStatus='Completed' IPMMedia='IS-PSOD-CU, IS-PSOD-SA, IS-PTV, TV-PSOD-CU, TV-PTV' IPMTerritory='AFGHAN, CHAD, DJIBOU, MAUITA, SOMALI, SSUDAN' IPMLanguage='ARA, FAS, INHIN, INURD' LicenseStart='08/01/2019' LicenseEnd='07/31/2020'><Orders><Order ID='1' ServicingStatus='Completed' Version='TheatricalVersion'><VendorId>Vubiquity</VendorId><SourceMioAssetId>TestSourceMioAssetId</SourceMioAssetId><ShellMioAssetId>TestShellMioAssetId</ShellMioAssetId><SRDueDate>07/01/2019</SRDueDate><EmbargoDate></EmbargoDate><PrimaryVideo>Y</PrimaryVideo><SecondaryAudio>French Parisian, Music And Effects</SecondaryAudio><SubtitlesFull>Arabic, English, French Parisian</SubtitlesFull><SubtitlesForced> French Parisian</SubtitlesForced><ClosedCaptions></ClosedCaptions><Trailer></Trailer><RebillType>Rebill A</RebillType><Amount></Amount><MetaData></MetaData><ArtWork></ArtWork><Document></Document><EOPPO></EOPPO><EOPDueDate></EOPDueDate><EOPStatus>Not Required</EOPStatus><EOPResource></EOPResource><EOPNotes></EOPNotes><PPSPO>4865863</PPSPO><PPSDueDate>07/01/2019</PPSDueDate><PPSStatus>Completed</PPSStatus><PPSResource></PPSResource><PPSNotes></PPSNotes><MaterialNotes>HD feature file naming conventions should follow the below template: BM-DF-MVXXXXXX.mxf HD trailer filenaming conventions should follow the below template: BR-DF-MVXXXXXX.mxf SD feature file naming conventions should follow the below template: FM-DF-MVXXXXXX.mxf SD trailer filenaming conventions should follow the below template: FR-DF-MVXXXXXX.mxf Here are the individual unique ID #s for each title:  ANNIE HALL MV012999 BLOW OUT MV013001 HOME OF OUR OWN, A MV013000 </MaterialNotes><Other></Other><IPMMedia></IPMMedia><Rejections><Rejection ID='' CurrentStatus='' Asset='' RejectionCode='' Issue='' CommentsHistory=''  RejectedBy='' RejectionDate='' Urgency='' RootCause='' Document=''></Rejection><Redelivered ID='' CurrentStatus='' Asset='' RejectionCode='' Issue='' CommentsHistory=''  RejectedBy='' RejectionDate='' Urgency='' RootCause='' Document=''></Redelivered><Resolved ID='' CurrentStatus='' Asset='' RejectionCode='' Issue='' CommentsHistory=''  RejectedBy='' RejectionDate='' Urgency='' RootCause='' Document=''></Resolved></Rejections></Order><Order ID='2' ServicingStatus='Completed' Version='TheatricalVersion'><VendorId>Vubiquity</VendorId><SourceMioAssetId>TestSourceMioAssetId</SourceMioAssetId><ShellMioAssetId>TestShellMioAssetId</ShellMioAssetId><SRDueDate>07/01/2019</SRDueDate><EmbargoDate></EmbargoDate><PrimaryVideo>Y</PrimaryVideo><SecondaryAudio>French Parisian, Music And Effects</SecondaryAudio><SubtitlesFull>Arabic, English, French Parisian</SubtitlesFull><SubtitlesForced> French Parisian</SubtitlesForced><ClosedCaptions></ClosedCaptions><Trailer></Trailer><RebillType>Rebill A</RebillType><Amount></Amount><MetaData></MetaData><ArtWork></ArtWork><Document></Document><EOPPO></EOPPO><EOPDueDate></EOPDueDate><EOPStatus>Not Required</EOPStatus><EOPResource></EOPResource><EOPNotes></EOPNotes><PPSPO>4865863</PPSPO><PPSDueDate>07/01/2019</PPSDueDate><PPSStatus>Completed</PPSStatus><PPSResource></PPSResource><PPSNotes></PPSNotes><MaterialNotes>HD feature file naming conventions should follow the below template: BM-DF-MVXXXXXX.mxf HD trailer filenaming conventions should follow the below template: BR-DF-MVXXXXXX.mxf SD feature file naming conventions should follow the below template: FM-DF-MVXXXXXX.mxf SD trailer filenaming conventions should follow the below template: FR-DF-MVXXXXXX.mxf Here are the individual unique ID #s for each title:  ANNIE HALL MV012999 BLOW OUT MV013001 HOME OF OUR OWN, A MV013000 </MaterialNotes><Other></Other><IPMMedia></IPMMedia><Rejections><Rejection ID='' CurrentStatus='' Asset='' RejectionCode='' Issue='' CommentsHistory=''  RejectedBy='' RejectionDate='' Urgency='' RootCause='' Document=''></Rejection><Redelivered ID='' CurrentStatus='' Asset='' RejectionCode='' Issue='' CommentsHistory=''  RejectedBy='' RejectionDate='' Urgency='' RootCause='' Document=''></Redelivered><Resolved ID='' CurrentStatus='' Asset='' RejectionCode='' Issue='' CommentsHistory=''  RejectedBy='' RejectionDate='' Urgency='' RootCause='' Document=''></Resolved></Rejections></Order></Orders></LineItem></LineItems></Title></Titles></Contract></Contracts></ServiceRequest>";
        private readonly string vendorId = "vubiquity";
        private readonly string vendorUserId = "vubiquity";

        public string ServiceRequestSubmitId { get; private set; }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult List()
        {

            ViewData["Message"] = "Service Requests";
            return View();
        }
        public IActionResult Submit()
        {
            ViewData["Message"] = "Service Request Submission.";

            var submissionResult = SubmitServiceRequest(xmlString);
            if (string.IsNullOrEmpty(submissionResult.ServiceRequestSubmitId)) ServiceRequestSubmitId = submissionResult.ServiceRequestSubmitId;
            else ViewData["SubmissionError"] = submissionResult.ErrorMessage;
            return View();
        }
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        private ServiceRequestSubmitResult SubmitServiceRequest(string xmlContent, bool validateXmlFirst = false)
        {
            XDocument xmlDoc = XmlProcessor.ConvertToXdoc(xmlContent, validateXmlFirst);
            IServiceRequestFacade srf = new ServiceRequestFacade(xmlDoc, vendorUserId, vendorId);
            srf.SubmitServiceRequestToVendor().PauseTilCompleted();                                                            //srf.PauseTilCompleted(srf.SubmitToVendor(vendorUser, vendorId, xmlDoc));
            return srf.SubmissionResult;
        }
    }
}
