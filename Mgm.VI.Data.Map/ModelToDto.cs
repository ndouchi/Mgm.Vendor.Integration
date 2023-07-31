using Mgm.VI.Data.Dto;
using Mgm.VI.Data.Model;
using System;
using System.Collections.Generic;

namespace Mgm.VI.Data.Map
{
    public static class ModelToDto
    {
        #region TODO: Enable Generics
        //public static List<IDto> Parse<TModel>(List<TModel> models)
        //{
        //    List<IDto> dtos = new List<IDto>();
        //    models.ForEach(dto => dtos.Add(Parse(dto)));
        //    return dtos;
        //}
        //public static IDto Parse<TModel>(TModel model)
        //{
        //    IDto model = null;

        //    switch (model.GetType().ToString())
        //    {
        //        case "ServiceRequestModel": // TODO: Instead of hard coded strings, use reflection
        //            return Parse(dto as ServiceRequestDto);
        //        case "ContractModel":
        //            return Parse(dto as ContractDto);
        //        case "TitleModel":
        //            return Parse(dto as TitleDto);
        //        case "OrderModel":
        //            return Parse(dto as OrderDto);
        //        case "LineItemModel":
        //            return Parse(dto as LineItemDto);
        //        case "RejectionModel":
        //            return Parse(dto as RejectionDto);
        //    }

        //    return dto;
        //}
        #endregion TODO: Enable Generics

        public static IServiceRequestDto Parse(IServiceRequestModel model)
        {
            var dto = new ServiceRequestDto
            {
                ID = model.ID,
                Description = model.Description,
                TransactionType = model.TransactionType,
                ServicingStatus = model.ServicingStatus,
                RushOrder = model.RushOrder,
                DueDate = model.DueDate,
                BusinessPartnerID = model.BusinessPartnerID,
                BusinessPartner = model.BusinessPartner,
                ProfileID = model.ProfileID,
                ProfileDescription = model.ProfileDescription,
                FastTrack = model.FastTrack,
                CreatedDate = model.CreatedDate,
                CreatedBy = model.CreatedBy,
                CompletedDate = model.CompletedDate,
                Contracts = ModelToDto.Parse(model.Contracts)
            };

            return dto;
        }
        public static IContractDto Parse(IContractModel model)
        {
            var dto = new ContractDto
            {
                ID = model.ID,
                ServicingStatus = model.ServicingStatus,
                Description = model.Description,
                Titles = ModelToDto.Parse(model.Titles)
            };

            return dto;
        }
        public static ITitleDto Parse(ITitleModel model)
        {
            var dto = new TitleDto
            {
                ID = model.ID,
                Description = model.Description,
                ServicingStatus = model.ServicingStatus,
                IPMStatus = model.IPMStatus,
                ContractualDueDate = model.ContractualDueDate,
                LicenseStartDate = model.LicenseStartDate,
                EOPResource = model.EOPResource,
                PPSResource = model.PPSResource,
                LineItems = ModelToDto.Parse(model.LineItems)
            };

            return dto;
        }
        public static ILineItemDto Parse(ILineItemModel model)
        {
            var dto = new LineItemDto
            {
                ID = model.ID,
                // AssetGroupId = model.AssetGroupId,
                ServicingStatus = model.ServicingStatus,
                IPMMedia = model.IPMMedia,
                IPMTerritory = model.IPMTerritory,
                IPMLanguage = model.IPMLanguage,
                LicenseStart = model.LicenseStart,
                LicenseEnd = model.LicenseEnd,
                Orders = ModelToDto.Parse(model.Orders)
            };

            return dto;
        }
        public static IOrderDto Parse(IOrderModel model)
        {
            var dto = new OrderDto
            {
                ID = model.ID,
                ServicingStatus = model.ServicingStatus,
                Version = model.Version,
                SRDueDate = model.SRDueDate,
                EmbargoDate = model.EmbargoDate,
                PrimaryVideo = model.PrimaryVideo,
                IPMMedia = model.IPMMedia,
                SecondaryAudio = model.SecondaryAudio,
                SubtitlesFull = model.SubtitlesFull,
                SubtitlesForced = model.SubtitlesForced,
                ClosedCaptions = model.ClosedCaptions,
                Trailer = model.Trailer,
                RebillType = model.RebillType,
                Amount = model.Amount,
                MetaData = model.MetaData,
                ArtWork = model.ArtWork,
                Document = model.Document,
                EOPPO = model.EOPPO,
                EOPDueDate = model.EOPDueDate,
                EOPStatus = model.EOPStatus,
                EOPResource = model.EOPResource,
                EOPNotes = model.EOPNotes,
                PPSPO = model.PPSPO,
                PPSStatus = model.PPSStatus,
                PPSResource = model.PPSResource,
                PPSNotes = model.PPSNotes,
                MaterialNotes = model.MaterialNotes,
                Other = model.Other,
                PPSDueDate = model.PPSDueDate,
                Rejections = ModelToDto.Parse(model.Rejections)
            };
         
            return dto;
        }
        public static IRejectionDto Parse(IRejectionModel model)
        {
            var dto = new RejectionDto
            {
                StatusCode = ModelToDto.Parse(model.StatusCode),
                ID = model.ID,
                Issue = model.Issue,
                CurrentStatus = model.CurrentStatus,
                Asset = model.Asset,
                RejectionCode = model.RejectionCode,
                CommentsHistory = model.CommentsHistory,
                RejectedBy = model.RejectedBy,
                RejectionDate = model.RejectionDate,
                RootCause = model.RootCause,
                Document = model.Document
            };

            return dto;
        }
        public static Dto.RejectionStatusCodeEnum Parse(Model.RejectionStatusCodeEnum modelStatusCode)
        {
            Dto.RejectionStatusCodeEnum dtoStatusCode = Dto.RejectionStatusCodeEnum.Rejection;
            switch (modelStatusCode)
            {
                case Model.RejectionStatusCodeEnum.Redelivered:
                    dtoStatusCode = Dto.RejectionStatusCodeEnum.Redelivered;
                    break;
                case Model.RejectionStatusCodeEnum.Rejection:
                    dtoStatusCode = Dto.RejectionStatusCodeEnum.Rejection;
                    break;
                case Model.RejectionStatusCodeEnum.Resolved:
                    dtoStatusCode = Dto.RejectionStatusCodeEnum.Resolved;
                    break;
                default:
                    break;
            }
            return dtoStatusCode;
        }
        public static IServiceRequestHistoryDto Parse(IServiceRequestHistoryModel model)
        {
            var dto = new ServiceRequestHistoryDto
            {
                Id = model.Id,
                VendorId = model.VendorId,
                VendorName = model.VendorName,
                ServiceRequestId = model.ServiceRequestId,
                Comments = model.Comments,
                MessageContent = model.MessageContent,
                SubmissionTimestamp = model.SubmissionTimestamp
            };

            return dto;
        }
        public static IServiceRequestStatusDto Parse(IServiceRequestStatusModel model)
        {
            var dto = new ServiceRequestStatusDto
            {
                Id = model.Id,
                MasterDataName = model.MasterDataName,
                MasterDataCode = model.MasterDataCode,
                MasterDataValue = model.MasterDataValue,
                SequenceOrder = model.SequenceOrder,
                CreatedBy = model.CreatedBy,
                Comments = model.Comments,
                Active = model.Active,
                CreateDate = model.CreateDate
            };

            return dto;
        }
        public static IStatusUpdateDto Parse(IStatusUpdateModel model)
        {
            var dto = new StatusUpdateDto
            {
                StatusUpdateId = model.StatusUpdateId,
                VendorId = model.VendorId,
                VendorName = model.VendorName,
                ServiceRequestId = model.ServiceRequestId,
                Comments = model.Comments,
                MessageContent = model.MessageContent,
                SqsRetrievalTimestamp = model.SqsRetrievalTimestamp,
                IsPersistedToMss = model.IsPersistedToMss
            };

            return dto;
        }
        public static IErrorLogDto Parse(IErrorLogModel model)
        {
            var dto = new ErrorLogDto()
            {
                LogId = model.LogId,
                ApplicationName = model.ApplicationName,
                ApplicationPath = model.ApplicationPath,
                ErrorMessage = model.ErrorMessage,
                ErrorSeverity = model.ErrorSeverity,
                ErrorSource = model.ErrorSource,
                MessageContent = model.MessageContent,
                LoggedTimestamp = model.LoggedTimestamp
            };
            return dto;
        }
        public static IVendorDto Parse(IVendorModel model)
        {
            var dto = new VendorDto
            {
                VendorId = model.VendorId,
                VendorName = model.VendorName,
                StagingId = model.StagingId,
                AWS_AccessKey = model.AWS_AccessKey,
                AWS_SecretKey = model.AWS_SecretKey,
                ServiceRequestApiURI = model.ServiceRequestApiURI,
                SQS_RegionEndPoint = model.SQS_RegionEndPoint,
                SQS_ServiceURL = model.SQS_ServiceURL,
                SQS_StatusUpdateDeadLetterURI = model.SQS_StatusUpdateDeadLetterURI,
                SQS_StatusUpdatePrimaryURI = model.SQS_StatusUpdatePrimaryURI,
                NotificationRecepients = model.NotificationRecepients,
                LastUpdatedBy = model.LastUpdatedBy,
                LastUpdatedOn = model.LastUpdatedOn
            };

            return dto;
        }

        public static List<IStatusUpdateDto> Parse(List<IStatusUpdateModel> models)
        {
            List<IStatusUpdateDto> dtos = new List<IStatusUpdateDto>();
            models.ForEach(model => dtos.Add(Parse(model)));
            return dtos;
        }
        public static List<IErrorLogDto> Parse(List<IErrorLogModel> models)
        {
            List<IErrorLogDto> dtos = new List<IErrorLogDto>();
            models.ForEach(model => dtos.Add(Parse(model)));
            return dtos;
        }
        public static List<IVendorDto> Parse(List<IVendorModel> models)
        {
            List<IVendorDto> dtos = new List<IVendorDto>();
            models.ForEach(model => dtos.Add(Parse(model)));
            return dtos;
        }

        public static List<IServiceRequestHistoryDto> Parse(List<IServiceRequestHistoryModel> models)
        {
            List<IServiceRequestHistoryDto> dtos = new List<IServiceRequestHistoryDto>();
            models.ForEach(dto => dtos.Add(Parse(dto)));
            return dtos;
        }
        public static List<IServiceRequestStatusDto> Parse(List<IServiceRequestStatusModel> models)
        {
            List<IServiceRequestStatusDto> dtos = new List<IServiceRequestStatusDto>();
            models.ForEach(dto => dtos.Add(Parse(dto)));
            return dtos;
        }
        public static List<IServiceRequestDto> Parse(List<IServiceRequestModel> models)
        {
            List<IServiceRequestDto> dtos = new List<IServiceRequestDto>();
            models.ForEach(dto => dtos.Add(Parse(dto)));
            return dtos;
        }
        public static List<IContractDto> Parse(List<IContractModel> contractModels)
        {
            List<IContractDto> dtos = new List<IContractDto>();
            contractModels.ForEach(dto => dtos.Add(Parse(dto)));
            return dtos;
        }
        public static List<ITitleDto> Parse(List<ITitleModel> titleModels)
        {
            List<ITitleDto> dtos = new List<ITitleDto>();
            titleModels.ForEach(dto => dtos.Add(Parse(dto)));
            return dtos;
        }
        public static List<ILineItemDto> Parse(List<ILineItemModel> lineItemModels)
        {
            List<ILineItemDto> dtos = new List<ILineItemDto>();
            lineItemModels.ForEach(dto => dtos.Add(Parse(dto)));
            return dtos;
        }
        public static List<IOrderDto> Parse(List<IOrderModel> orderModels)
        {
            List<IOrderDto> dtos = new List<IOrderDto>();
            orderModels.ForEach(dto => dtos.Add(Parse(dto)));
            return dtos;
        }
        public static List<IRejectionDto> Parse(List<IRejectionModel> rejectionModels)
        {
            List<IRejectionDto> dtos = new List<IRejectionDto>();
            rejectionModels.ForEach(dto => dtos.Add(Parse(dto)));
            return dtos;
        }
    }
}
