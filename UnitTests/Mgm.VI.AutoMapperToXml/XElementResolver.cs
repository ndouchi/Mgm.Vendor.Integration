using System;
using System.Xml.Linq;
using AutoMapper;

namespace Mgm.VI.AutoMapperToXml
{
    public class XElementResolver<T, TMember> : IValueResolver<XElement, T, TMember>
    {
        public TMember Resolve(XElement source, T destination, TMember destMember, ResolutionContext context)
        {
            if (source == null || string.IsNullOrEmpty(source.Value))
                return default(TMember);

            return (TMember)Convert.ChangeType(source.Value, typeof(TMember));
        }
    }

    public class XAttributeResolver<T, TMember> : IValueResolver<XElement, T, TMember>
    {
        public XAttributeResolver(string attributeName)
        {
            Name = attributeName;
        }

        public string Name { get; set; }

        //       protected override T ResolveCore(XElement source)
        public TMember Resolve(XElement source, T destination, TMember destMember, ResolutionContext context)
        {
            if (source == null)
                return default(TMember);
            var attribute = source.Attribute(Name);
            if (attribute == null || String.IsNullOrEmpty(attribute.Value))
                return default(TMember);

            return (TMember)Convert.ChangeType(attribute.Value, typeof(TMember));
        }
    }
}