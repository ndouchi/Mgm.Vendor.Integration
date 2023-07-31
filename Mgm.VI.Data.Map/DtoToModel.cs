using Mgm.VI.Data.Dto;
using Mgm.VI.Data.Model;
using System;
using System.Collections.Generic;

namespace Mgm.VI.Data.Map
{
    public static class DtoToModel
    {
        #region TODO: Enable Generics
        //public static List<IModel> Parse<TDto>(List<TDto> dtos)
        //{
        //    List<IModel> models = new List<IModel>();
        //    dtos.ForEach(dto => models.Add(Parse(dto)));
        //    return models;
        //}
        //public static IModel Parse<TDto>(TDto dto)
        //{
        //    IModel model = null;

        //    switch (dto.GetType().ToString())
        //    {
        //        case "ServiceRequestDto": // TODO: Instead of hard coded strings, use reflection
        //            return Parse(dto as ServiceRequestDto);
        //        case "ContractDto":
        //            return Parse(dto as ContractDto);
        //        case "TitleDto":
        //            return Parse(dto as TitleDto);
        //        case "OrderDto":
        //            return Parse(dto as OrderDto);
        //        case "LineItemDto":
        //            return Parse(dto as LineItemDto);
        //        case "RejectionDto":
        //            return Parse(dto as RejectionDto);
        //    }

        //    return model;
        //}
        #endregion TODO: Enable Generics
        public static IServiceRequestModel Parse(IServiceRequestDto dto)
        {
            var model = new ServiceRequestModel
            {
                ID = dto.ID,
                Description = dto.Description,
                TransactionType = dto.TransactionType,
                ServicingStatus = dto.ServicingStatus,
                RushOrder = dto.RushOrder,
                DueDate = dto.DueDate,
                BusinessPartnerID = dto.BusinessPartnerID,
                BusinessPartner = dto.BusinessPartner,
                ProfileID = dto.ProfileID,
                ProfileDescription = dto.ProfileDescription,
                FastTrack = dto.FastTrack,
                CreatedDate = dto.CreatedDate,
                CreatedBy = dto.CreatedBy,
                CompletedDate = dto.CompletedDate,
                Contracts = DtoToModel.Parse(dto.Contracts)
            };

            return model;
        }
        public static IContractModel Parse(IContractDto dto)
        {
            var model = new ContractModel
            {
                ID = dto.ID,
                ServicingStatus = dto.ServicingStatus,
                Description = dto.Description,
                Titles = DtoToModel.Parse(dto.Titles)
            };

            return model;
        }
        public static ITitleModel Parse(ITitleDto dto)
        {
            var model = new TitleModel
            {
                ID = dto.ID,
                Description = dto.Description,
                ServicingStatus = dto.ServicingStatus,
                IPMStatus = dto.IPMStatus,
                ContractualDueDate = dto.ContractualDueDate,
                LicenseStartDate = dto.LicenseStartDate,
                EOPResource = dto.EOPResource,
                PPSResource = dto.PPSResource,
                LineItems = DtoToModel.Parse(dto.LineItems)
            };

            return model;
        }
        public static ILineItemModel Parse(ILineItemDto dto)
        {
            var model = new LineItemModel
            {
                ID = dto.ID,
                // AssetGroupId dto.AssetGroupId,
                ServicingStatus = dto.ServicingStatus,
                IPMMedia = dto.IPMMedia,
                IPMTerritory = dto.IPMTerritory,
                IPMLanguage = dto.IPMLanguage,
                LicenseStart = dto.LicenseStart,
                LicenseEnd = dto.LicenseEnd,
                Orders = DtoToModel.Parse(dto.Orders)
            };

            return model;
        }
        public static IOrderModel Parse(IOrderDto dto)
        {
            var model = new OrderModel
            {
                ID = dto.ID,
                ServicingStatus = dto.ServicingStatus,
                Version = dto.Version,
                SRDueDate = dto.SRDueDate,
                EmbargoDate = dto.EmbargoDate,
                PrimaryVideo = dto.PrimaryVideo,
                IPMMedia = dto.IPMMedia,
                SecondaryAudio = dto.SecondaryAudio,
                SubtitlesFull = dto.SubtitlesFull,
                SubtitlesForced = dto.SubtitlesForced,
                ClosedCaptions = dto.ClosedCaptions,
                Trailer = dto.Trailer,
                RebillType = dto.RebillType,
                Amount = dto.Amount,
                MetaData = dto.MetaData,
                ArtWork = dto.ArtWork,
                Document = dto.Document,
                EOPPO = dto.EOPPO,
                EOPDueDate = dto.EOPDueDate,
                EOPStatus = dto.EOPStatus,
                EOPResource = dto.EOPResource,
                EOPNotes = dto.EOPNotes,
                PPSPO = dto.PPSPO,
                PPSStatus = dto.PPSStatus,
                PPSResource = dto.PPSResource,
                PPSNotes = dto.PPSNotes,
                MaterialNotes = dto.MaterialNotes,
                Other = dto.Other,
                PPSDueDate = dto.PPSDueDate,
                Rejections = DtoToModel.Parse(dto.Rejections)
            };

            return model;
        }
        public static IRejectionModel Parse(IRejectionDto dto)
        {
            var model = new RejectionModel
            {
                StatusCode = DtoToModel.Parse(dto.StatusCode),
                ID = dto.ID,
                Issue = dto.Issue,
                CurrentStatus = dto.CurrentStatus,
                Asset = dto.Asset,
                RejectionCode = dto.RejectionCode,
                CommentsHistory = dto.CommentsHistory,
                RejectedBy = dto.RejectedBy,
                RejectionDate = dto.RejectionDate,
                RootCause = dto.RootCause,
                Document = dto.Document
            };

            return model;
        }
        public static Model.RejectionStatusCodeEnum Parse(Dto.RejectionStatusCodeEnum dtoStatusCode)
        {
            Model.RejectionStatusCodeEnum modelStatusCode = Model.RejectionStatusCodeEnum.Rejection;
            switch (dtoStatusCode)
            {
                case Dto.RejectionStatusCodeEnum.Redelivered:
                    modelStatusCode = Model.RejectionStatusCodeEnum.Redelivered;
                    break;
                case Dto.RejectionStatusCodeEnum.Rejection:
                    modelStatusCode = Model.RejectionStatusCodeEnum.Rejection;
                    break;
                case Dto.RejectionStatusCodeEnum.Resolved:
                    modelStatusCode = Model.RejectionStatusCodeEnum.Resolved;
                    break;
                default:
                    break;
            }
            return modelStatusCode;
        }
        public static IServiceRequestHistoryModel Parse(IServiceRequestHistoryDto dto)
        {
            var model = new ServiceRequestHistoryModel
            {
                Id = dto.Id,
                ServiceRequestId = dto.ServiceRequestId,
                VendorId = dto.VendorId,
                VendorName = dto.VendorName,
                Comments = dto.Comments,
                MessageContent = dto.MessageContent,
                SubmissionTimestamp = dto.SubmissionTimestamp
            };

            return model;
        }
        public static IServiceRequestStatusModel Parse(IServiceRequestStatusDto dto)
        {
            var model = new ServiceRequestStatusModel
            {
                Id = dto.Id,
                MasterDataName = dto.MasterDataName,
                MasterDataCode = dto.MasterDataCode,
                MasterDataValue = dto.MasterDataValue,
                SequenceOrder = dto.SequenceOrder,
                CreatedBy = dto.CreatedBy,
                Comments = dto.Comments,
                Active = dto.Active,
                CreateDate = dto.CreateDate
            };

            return model;
        }
        public static IStatusUpdateModel Parse(IStatusUpdateDto dto)
        {
            var model = new StatusUpdateModel
            {
                StatusUpdateId = dto.StatusUpdateId,
                VendorId = dto.VendorId,
                VendorName = dto.VendorName,
                ServiceRequestId = dto.ServiceRequestId,
                Comments = dto.Comments,
                MessageContent = dto.MessageContent,
                SqsRetrievalTimestamp = dto.SqsRetrievalTimestamp,
                IsPersistedToMss = dto.IsPersistedToMss
            };

            return model;
        }
        public static IErrorLogModel Parse(IErrorLogDto dto)
        {
            var model = new ErrorLogModel()
            {
                LogId = dto.LogId,
                ApplicationName = dto.ApplicationName,
                ApplicationPath = dto.ApplicationPath,
                ErrorMessage = dto.ErrorMessage,
                ErrorSeverity = dto.ErrorSeverity,
                ErrorSource = dto.ErrorSource,
                MessageContent = dto.MessageContent,
                LoggedTimestamp = dto.LoggedTimestamp
            };
            return model;
        }
        public static IVendorModel Parse(IVendorDto dto)
        {
            var model = new VendorModel
            {
                VendorId = dto.VendorId,
                VendorName = dto.VendorName,
                StagingId = dto.StagingId,
                AWS_AccessKey = dto.AWS_AccessKey,
                AWS_SecretKey = dto.AWS_SecretKey,
                ServiceRequestApiURI = dto.ServiceRequestApiURI,
                SQS_RegionEndPoint = dto.SQS_RegionEndPoint,
                SQS_ServiceURL = dto.SQS_ServiceURL,
                SQS_StatusUpdateDeadLetterURI = dto.SQS_StatusUpdateDeadLetterURI,
                SQS_StatusUpdatePrimaryURI = dto.SQS_StatusUpdatePrimaryURI,
                NotificationRecepients = dto.NotificationRecepients,
                LastUpdatedBy = dto.LastUpdatedBy,
                LastUpdatedOn = dto.LastUpdatedOn
            };

            return model;
        }

        public static List<IServiceRequestHistoryModel> Parse(List<IServiceRequestHistoryDto> dtos)
        {
            List<IServiceRequestHistoryModel> models = new List<IServiceRequestHistoryModel>();
            dtos.ForEach(dto => models.Add(Parse(dto)));
            return models;
        }
        public static List<IServiceRequestStatusModel> Parse(List<IServiceRequestStatusDto> dtos)
        {
            List<IServiceRequestStatusModel> models = new List<IServiceRequestStatusModel>();
            dtos.ForEach(dto => models.Add(Parse(dto)));
            return models;
        }
        public static List<IStatusUpdateModel> Parse(List<IStatusUpdateDto> dtos)
        {
            List<IStatusUpdateModel> models = new List<IStatusUpdateModel>();
            dtos.ForEach(dto => models.Add(Parse(dto)));
            return models;
        }
        public static List<IErrorLogModel> Parse(List<IErrorLogDto> dtos)
        {
            List<IErrorLogModel> models = new List<IErrorLogModel>();
            dtos.ForEach(dto => models.Add(Parse(dto)));
            return models;
        }
        public static List<IVendorModel> Parse(List<IVendorDto> dtos)
        {
            List<IVendorModel> models = new List<IVendorModel>();
            dtos.ForEach(dto => models.Add(Parse(dto)));
            return models;
        }
        public static List<IServiceRequestModel> Parse(List<IServiceRequestDto> dtos)
        {
            List<IServiceRequestModel> models = new List<IServiceRequestModel>();
            dtos.ForEach(dto => models.Add(Parse(dto)));
            return models;
        }
        public static List<IContractModel> Parse(List<IContractDto> contractDtos)
        {
            List<IContractModel> models = new List<IContractModel>();
            contractDtos.ForEach(dto => models.Add(Parse(dto)));
            return models;
        }
        public static List<ITitleModel> Parse(List<ITitleDto> titleDtos)
        {
            List<ITitleModel> models = new List<ITitleModel>();
            titleDtos.ForEach(dto => models.Add(Parse(dto)));
            return models;
        }
        public static List<ILineItemModel> Parse(List<ILineItemDto> lineItemDtos)
        {
            List<ILineItemModel> models = new List<ILineItemModel>();
            lineItemDtos.ForEach(dto => models.Add(Parse(dto)));
            return models;
        }
        public static List<IOrderModel> Parse(List<IOrderDto> orderDtos)
        {
            List<IOrderModel> models = new List<IOrderModel>();
            orderDtos.ForEach(dto => models.Add(Parse(dto)));
            return models;
        }
        public static List<IRejectionModel> Parse(List<IRejectionDto> rejectionDtos)
        {
            List<IRejectionModel> models = new List<IRejectionModel>();
            rejectionDtos.ForEach(dto => models.Add(Parse(dto)));
            return models;
        }
    }
}
