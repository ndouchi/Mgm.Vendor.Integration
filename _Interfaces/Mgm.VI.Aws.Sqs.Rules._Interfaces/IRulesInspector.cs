//////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;

namespace Mgm.VI.Aws.Sqs.Rules
{
    public interface IRulesInspector : IDisposable
    {
         object Content { get; set; }
         List<IRule> Rules { set; }
         bool RulesAreMet { get;  }

        #region Public Methods
        IRulesInspector AddRule(IRule rule);
        IRulesInspector SetContent(object content, List<IRule> rules = null);
        IRulesInspector Validate();
        IRulesInspector Validate(IRule rule);
        IRulesInspector Validate(List<IRule> rules);
        #endregion  Public Methods
    }
}