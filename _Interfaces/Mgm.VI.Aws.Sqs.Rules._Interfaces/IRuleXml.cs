//////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////

using System.Xml.Linq;
///
/// Engineer:   Nour Douchi
/// Company:    MGM
/// Project:    MGM-Vubiquity Asset Integration
/// Revision:   10/11/2019 
/// 
namespace Mgm.VI.Aws.Sqs.Rules
{
    public interface IRuleXml : IRule
    {
        XDocument XmlContent { get; set; }
    }
}