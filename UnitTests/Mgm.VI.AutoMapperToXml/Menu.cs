using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using AutoMapper;

namespace Mgm.VI.AutoMapperToXml
{
    public class Menu
    {
        public Menu()
        {
            SubMenus = new List<Menu>();
            MenuItems = new List<MenuItem>();
        }
        public IList<Menu> SubMenus { get; set; }
        public IList<MenuItem> MenuItems { get; set; }
        public string Name { get; set; }
    }

    public class MenuItem
    {
        public string Name { get; set; }
        public string Action { get; set; }
        public string Parameters { get; set; }
    }

    public static class MapInitializer
    {
        private static readonly Func<XElement, string, string, List<XElement>> MapItems =
            (src, collectionName, elementName) =>
            (src.Element(collectionName) ?? new XElement(collectionName)).Elements(elementName).ToList();

        public static void CreateMenuMap()
        {
            //MenuItem map
            Mapper.CreateMap<XElement, MenuItem>()
                  .ForMember(dest => dest.Name,
                             opt => opt.ResolveUsing<XAttributeResolver<string>>()
                                       .ConstructedBy(() => new XAttributeResolver<string>("name")))
                  .ForMember(dest => dest.Action,
                             opt => opt.ResolveUsing<XAttributeResolver<string>>()
                                       .ConstructedBy(() => new XAttributeResolver<string>("action")))
                  .ForMember(dest => dest.Parameters,
                             opt => opt.ResolveUsing<XAttributeResolver<string>>()
                                       .ConstructedBy(() => new XAttributeResolver<string>("parameters")));
            // Menu map
            Mapper.CreateMap<XElement, Menu>()
                  .ForMember(dest => dest.Name,
                             opt =>
                             opt.ResolveUsing<XAttributeResolver<string>>()
                                    .ConstructedBy(() => new XAttributeResolver<string>("name")))
                  .ForMember(dest => dest.MenuItems,
                             opt => opt.MapFrom(src => MapItems(src, "MenuItems", "MenuItem")))
                  .ForMember(dest => dest.SubMenus,
                             opt => opt.MapFrom(src => MapItems(src, "SubMenus", "Menu")));
        }
    }
}
