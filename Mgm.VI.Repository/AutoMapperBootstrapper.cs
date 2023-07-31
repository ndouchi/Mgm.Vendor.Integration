using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using AutoMapper;

namespace Mgm.Mss.Sqs.Repository
{
    public static class AutoMapperBootstrapper
    {
        public static void AddMappings()
        {
            MapInitializer.CreateMenuMap();
            var xml = XDocument.Load(@".\Menu.xml");
            var menu = Mapper.Map<XElement, Menu>(xml.Element("Menu"));
            foreach (var sm in menu.SubMenus)
            {
                ShowMenu(sm);
            }
            foreach (var mi in menu.MenuItems)
            {
                ShowMenuItem(mi);
            }
        }
    }
    public class XElementResolver<T> : ValueResolver<XElement, T>
    {
        protected override T ResolveCore(XElement source)
        {
            if (source == null || string.IsNullOrEmpty(source.Value))
                return default(T);

            return (T)Convert.ChangeType(source.Value, typeof(T));
        }
    }
    public class XAttributeResolver<T> : ValueResolver<XElement, T>
    {
        public XAttributeResolver(string attributeName)
        {
            Name = attributeName;
        }

        public string Name { get; set; }

        protected override T ResolveCore(XElement source)
        {
            if (source == null)
                return default(T);
            var attribute = source.Attribute(Name);
            if (attribute == null || String.IsNullOrEmpty(attribute.Value))
                return default(T);

            return (T)Convert.ChangeType(attribute.Value, typeof(T));
        }
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