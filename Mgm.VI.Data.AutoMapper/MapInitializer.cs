using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using AutoMapper;
using Mgm.VI.Data.Dto;

namespace Mgm.VI.Data.AutoMapper
{
    public static class MapInitializer
    {
        public static Action<IMapperConfigurationExpression> MapperConfigureAction;
        public static Action<IMapperConfigurationExpression> MapperConfigureMockAction = cfg => cfg.CreateMap<OrderDto, OrderDto>();
        public static List<Action<IMapperConfigurationExpression>> MapperConfigureMockActions;

        public static IMapper CreateDtoMaps()
        {
            MapperConfigureAction = cfg =>
            {
                cfg.AddProfile(new DtoMappingProfile());
            };

            var config = new MapperConfiguration(MapperConfigureAction);
            var mapper = config.CreateMapper();
            return mapper;
        }
        private static readonly Func<XElement, string, string, List<XElement>> MapItems =
            (src, collectionName, elementName) =>
            (src.Element(collectionName) ?? new XElement(collectionName)).Elements(elementName).ToList();

        public static IMapper CreateOrderMap()
        {
            /*
            [XmlElement("ID")]
            public string ID { get; set; }
            [XmlElement("ServicingStatus")]
            public string ServicingStatus { get; set; }
            [XmlElement("Version")]
            public string Version { get; set; }
            [XmlArray("Rejections")]
            [XmlArrayItem("RejectionDto")]
            public List<RejectionDto> Rejections;
             * */
            MapperConfigureAction = cfg => cfg.CreateMap<XElement, OrderDto>()
                  //.ForMember(dest => dest.Name,
                  //           opt => opt.ResolveUsing<XAttributeResolver<string>>()
                  //                     .ConstructedBy(() => new XAttributeResolver<string>("name")))
                  //.ForMember(dest => dest.Action,
                  //           opt => opt.ResolveUsing<XAttributeResolver<string>>()
                  //                     .ConstructedBy(() => new XAttributeResolver<string>("action")))
                  //.ForMember(dest => dest.Parameters,
                  //           opt => opt.ResolveUsing<XAttributeResolver<string>>()
                  //                     .ConstructedBy(() => new XAttributeResolver<string>("parameters")))
                  ;

            var config = new MapperConfiguration(MapperConfigureAction);
            var mapper = config.CreateMapper();
            return mapper;
        }
        public static IMapper CreateLineItemMap()
        {
            /*
        [XmlElement("ID")]
        public string ID { get; set; }
        [XmlElement("ServicingStatus")]
        public string ServicingStatus { get; set; }
        [XmlElement("IPMMedia")]
        public string IPMMedia { get; set; }
        [XmlElement("IPMTerritory")]
        public string IPMTerritory { get; set; }
        [XmlElement("IPMLanguage")]
        public string IPMLanguage { get; set; }
        [XmlElement("LicenseStart")]
        public string LicenseStart { get; set; }
        [XmlElement("LicenseEnd")]
        public string LicenseEnd { get; set; }
        [XmlArray("Orders")]
        [XmlArrayItem("OrderDto")]
        public List<OrderDto> Orders { get; set; }
             * */
            MapperConfigureAction = cfg => cfg.CreateMap<XElement, LineItemDto>()
                  //.ForMember(dest => dest.Name,
                  //           opt =>
                  //           opt.ResolveUsing<XAttributeResolver<string>>()
                  //                  .ConstructedBy(() => new XAttributeResolver<string>("name")))
                  //.ForMember(dest => dest.MenuItems,
                  //           opt => opt.MapFrom(src => MapItems(src, "MenuItems", "MenuItem")))
                  //.ForMember(dest => dest.SubMenus,
                  //           opt => opt.MapFrom(src => MapItems(src, "SubMenus", "Menu")))
                  ;
            var config = new MapperConfiguration(MapperConfigureAction);
            var mapper = config.CreateMapper();
            return mapper;
        }
        public static IMapper CreateRejectionMap()
        {
            /*
        [XmlElement("StatusCode")]
        public RejectionStatusCodeEnum StatusCode { get; set; }
        [XmlElement("ID")]
        public string ID { get; set; }
        [XmlElement("CurrentStatus")]
        public string CurrentStatus { get; set; }
        [XmlElement("Asset")]
        public string Asset { get; set; }
        [XmlElement("RejectionCode")]
        public string RejectionCode { get; set; }
        [XmlElement("Issue")]
        public string Issue { get; set; }
        [XmlElement("CommentsHistory")]
        public string CommentsHistory { get; set; }
        [XmlElement("RejectedBy")]
        public string RejectedBy { get; set; }
        [XmlElement("RejectionDate")]
        public string RejectionDate { get; set; }
        [XmlElement("Urgency")]
        public string Urgency { get; set; }
        [XmlElement("RootCause")]
        public string RootCause { get; set; }
        [XmlElement("Document")]
        public string Document { get; set; }
        * */
            MapperConfigureAction = cfg => cfg.CreateMap<XElement, RejectionDto>()
                  //.ForMember(dest => dest.Name,
                  //           opt => opt.ResolveUsing<XAttributeResolver<string>>()
                  //                     .ConstructedBy(() => new XAttributeResolver<string>("name")))
                  //.ForMember(dest => dest.Action,
                  //           opt => opt.ResolveUsing<XAttributeResolver<string>>()
                  //                     .ConstructedBy(() => new XAttributeResolver<string>("action")))
                  //.ForMember(dest => dest.Parameters,
                  //           opt => opt.ResolveUsing<XAttributeResolver<string>>()
                  //                     .ConstructedBy(() => new XAttributeResolver<string>("parameters")))
                  ;

            var config = new MapperConfiguration(MapperConfigureAction);
            var mapper = config.CreateMapper();
            return mapper;
        }
        public static IMapper CreateServiceRequestMap()
        {
            /*
        [XmlElement("ID")]
        public string ID { get; set; }
        [XmlElement("Description")]
        public string Description { get; set; }
        [XmlElement("TransactionType")]
        public string TransactionType { get; set; }
        [XmlElement("ServicingStatus")]
        public string ServicingStatus { get; set; }
        [XmlElement("RushOrder")]
        public string RushOrder { get; set; }
        [XmlElement("DueDate")]
        public string DueDate { get; set; }
        [XmlElement("BusinessPartnerID")]
        public string BusinessPartnerID { get; set; }
        [XmlElement("BusinessPartner")]
        public string BusinessPartner { get; set; }
        [XmlElement("ProfileID")]
        public string ProfileID { get; set; }
        [XmlElement("ProfileDescription")]
        public string ProfileDescription { get; set; }
        [XmlElement("FastTrack")]
        public string FastTrack { get; set; }
        [XmlElement("CreatedDate")]
        public string CreatedDate { get; set; }
        [XmlElement("CreatedBy")]
        public string CreatedBy { get; set; }
        [XmlElement("CompletedDate")]
        public string CompletedDate { get; set; }
        [XmlArray("Contracts")]
        [XmlArrayItem("ContractDto")]
        public List<ContractDto> Contracts { get; set; }
        */
            MapperConfigureAction = cfg => cfg.CreateMap<XElement, RejectionDto>()
                  //.ForMember(dest => dest.Name,
                  //           opt => opt.ResolveUsing<XAttributeResolver<string>>()
                  //                     .ConstructedBy(() => new XAttributeResolver<string>("name")))
                  //.ForMember(dest => dest.Action,
                  //           opt => opt.ResolveUsing<XAttributeResolver<string>>()
                  //                     .ConstructedBy(() => new XAttributeResolver<string>("action")))
                  //.ForMember(dest => dest.Parameters,
                  //           opt => opt.ResolveUsing<XAttributeResolver<string>>()
                  //                     .ConstructedBy(() => new XAttributeResolver<string>("parameters")))
                  ;

            var config = new MapperConfiguration(MapperConfigureAction);
            var mapper = config.CreateMapper();
            return mapper;
        }
        public static IMapper CreateContractMap()
        {
        /*
        [XmlElement("ID")]
        string ID { get; set; }
        [XmlElement("ServicingStatus")]
        string ServicingStatus { get; set; }
        [XmlElement("Description")]
        string Description { get; set; }
        [XmlArray("Titles")]
        [XmlArrayItem("TitleDto")]
        List<TitleDto> Titles { get; set; }
        */
        MapperConfigureAction = cfg => cfg.CreateMap<XElement, ContractDto>()
        //.ForMember(dest => dest.Name,
        //           opt => opt.ResolveUsing<XAttributeResolver<string>>()
        //                     .ConstructedBy(() => new XAttributeResolver<string>("name")))
        //.ForMember(dest => dest.Action,
        //           opt => opt.ResolveUsing<XAttributeResolver<string>>()
        //                     .ConstructedBy(() => new XAttributeResolver<string>("action")))
        //.ForMember(dest => dest.Parameters,
        //           opt => opt.ResolveUsing<XAttributeResolver<string>>()
        //                     .ConstructedBy(() => new XAttributeResolver<string>("parameters")))
        ;
            
            var config = new MapperConfiguration(MapperConfigureAction);
            var mapper = config.CreateMapper();
            return mapper;
        }
        public static IMapper CreateTitleMap()
        {
        /*
        [XmlElement("ID")]
        public string ID { get; set; }
        [XmlElement("Description")]
        public string Description { get; set; }
        [XmlElement("ServicingStatus")]
        public string ServicingStatus { get; set; }
        [XmlElement("IPMStatus")]
        public string IPMStatus { get; set; }
        [XmlElement("ContractualDueDate")]
        public string ContractualDueDate { get; set; }
        [XmlElement("LicenseStartDate")]
        public string LicenseStartDate { get; set; }
        [XmlElement("EOPResource")]
        public string EOPResource { get; set; }
        [XmlElement("PPSResource")]
        public string PPSResource { get; set; }
        [XmlArray("LineItems")]
        [XmlArrayItem("LineItemDto")]
        public List<LineItemDto> LineItems { get; set; }
        */
            MapperConfigureAction = cfg => cfg.CreateMap<XElement, TitleDto>()
                  //.ForMember(dest => dest.Name,
                  //           opt => opt.ResolveUsing<XAttributeResolver<string>>()
                  //                     .ConstructedBy(() => new XAttributeResolver<string>("name")))
                  //.ForMember(dest => dest.Action,
                  //           opt => opt.ResolveUsing<XAttributeResolver<string>>()
                  //                     .ConstructedBy(() => new XAttributeResolver<string>("action")))
                  //.ForMember(dest => dest.Parameters,
                  //           opt => opt.ResolveUsing<XAttributeResolver<string>>()
                  //                     .ConstructedBy(() => new XAttributeResolver<string>("parameters")))
                  ;

            var config = new MapperConfiguration(MapperConfigureAction);
            var mapper = config.CreateMapper();
            return mapper;
        }
    }
}
