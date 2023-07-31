//////////////////////////////////////////////////////////////////
///
/// Engineer:   Nour Douchi
/// Company:    MGM
/// Project:    MGM-Vubiquity Asset Integration
/// Revision:   10/11/2019 
/// 
//////////////////////////////////////////////////////////////////

using Amazon.SQS.Model;
using Mgm.VI.Aws.Sqs.Dto;
using Mgm.VI.Aws.Sqs.Rules;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mgm.VI.Aws.Sqs.Rules
{
    public class RulesInspector : IRulesInspector
    {
        #region Private vars
        private bool disposed = false;
        private bool _rulesAreMet = true;
        private object _content;
        private List<IRule> _rules;
        public RulesInspector _rulesInspector;
        #endregion Private vars

        #region Properties
        public object Content
        {
            get
            {
                return this._content;
            }
            set
            {
                SetContent(value);
            }
        }
        public List<IRule> Rules
        {
            private get { return this._rules; }
            set
            {
                this._rules = value;
                if (_content != null) Validate();
            }
        }
        public bool RulesAreMet
        {
            get
            {
                Validate();
                return _rulesAreMet;
            }
            private set
            {
                _rulesAreMet = value;
            }
        }
        #endregion Properties

        #region Constructors
        public RulesInspector() { }
        public RulesInspector(List<IRule> rules)
        {
            this.Rules = rules;
        }
        public RulesInspector(Message message) : this ((object) message)
        {
        }
        public RulesInspector(object content, List<IRule> rules = null)
        {
            SetContent(content);
            this.Rules = rules;
        }
        public RulesInspector(string content, List<IRule> rules = null)
        {
            SetContent(content);
            this.Rules = rules;
        }
        public RulesInspector(Message message, List<IRule> rules) : this ((object) MessageDto.Parse(message).Body, rules)
        {
        }
        #endregion Constructors

        #region Public Methods
        public IRulesInspector AddRule(IRule rule)
        {
            _rules = _rules ?? new List<IRule>();
            _rules.Add(rule);
            return this;
        }
        public IRulesInspector SetContent(object content, List<IRule> rules = null)
        {
            _rulesAreMet = true;
            if (content != null)
                _content = content;
            else
                _rulesAreMet = false;

            if (rules != null) Rules = rules;
            
            return this;
        }
        IRulesInspector IRulesInspector.Validate()
        {
            return Validate();
        }
        public IRulesInspector Validate()
        {
            return Validate(this.Rules);
        }
        public IRulesInspector Validate(IRule rule)
        {
            rule.Content = this._content;
            _rulesAreMet = _rulesAreMet && rule.IsMet;
            return this;
        }
        public IRulesInspector Validate(List<IRule> rules)
        {
            rules?.ForEach(r => { Validate(r); });
            return this;
        }
        #endregion  Public Methods

        #region Private Methods
        #endregion Private Methods

        #region Dispose
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                   _content = null;
                }
                this.disposed = true;
            }
        }
        #endregion Dispose

    }
}
