//////////////////////////////////////////////////////////////////
///
/// Engineer:   Nour Douchi
/// Company:    MGM
/// Project:    MGM-Vubiquity Asset Integration
/// Revision:   10/11/2019 
/// 
//////////////////////////////////////////////////////////////////

using Amazon.SQS.Model;
using Mgm.VI.Aws.Sqs.Rules;
using Mgm.VI.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mgm.VI.Aws.Sqs.Rules
{
    public abstract class Rule : IRule
    {
        private object _content;

        protected bool __isMet;
        protected List<string> __reasonsNotMet;
        public abstract bool IsMet { get; } // Consider removing the abstract to enable private set
        public List<string> ReasonsNotMet { get { return __reasonsNotMet; } } // Consider removing the abstract to enable private set
        public IErrorMessages ErrorMessages { get; private set; }
        public object Content
        {
            get
            {
                return this._content;
            }
            set
            {
                this._content = value;
            }
        }

        public Rule(object content, IErrorMessages errorMessages = null) 
        {
            #region Instantiate Variables
            if (content != null) _content = content;
            ErrorMessages = errorMessages ?? new ErrorMessages();
            __isMet = true;
            __reasonsNotMet = new List<string>();
            #endregion Instantiate Variables
        }
        public Rule(IErrorMessages errorMessages = null) : this(null, errorMessages)
        {}
        protected void AddToReasonsNotMet(string reason)
        {
            __isMet = false;
            __reasonsNotMet.Add(reason);
        }
    }
}
