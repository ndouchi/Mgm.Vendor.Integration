using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using AutoMapper;

namespace Mgm.VI.Data.AutoMapper
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
 }
