using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using AutoMapper;
using Mgm.VI.Data.Dto;

namespace Mgm.VI.Data.AutoMapper
{
    public class DtoMappingProfile : Profile
    {
        public DtoMappingProfile()
        {
            CreateMap<XElement, ServiceRequestDto>();
            CreateMap<XElement, LineItemDto>();
            CreateMap<XElement, TitleDto>();
            CreateMap<XElement, ContractDto>();
            CreateMap<XElement, RejectionDto>();
            CreateMap<XElement, OrderDto>();
        }
    }
}
