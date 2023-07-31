using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Mgm.VI.Common
{
    public class ErrorMessages : IErrorMessages, IEnumerable<IErrorMessage>
    {
        public List<IErrorMessage> errorMessages { get; set; }
        public ErrorMessages() { }
        public ErrorMessages(List<IErrorMessage> myErrorMessages) {
            errorMessages = myErrorMessages;
        }
        public IEnumerator<IErrorMessage> GetEnumerator()
        {
            foreach (ErrorMessage e in errorMessages)
            {
                yield return e;
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public void AddToErrors(string errorSource, string errorText, Exception ex = null)
        {
            //AddToErrors(new IErrorMessage(errorSource, errorText, ex));
            AddToErrors(new ErrorMessage(errorSource, errorText, ex));
        }
        public void AddToErrors(IErrorMessage e)
        {
            if (e == null) return;

            if (errorMessages == null) errorMessages = new List<IErrorMessage>();

            errorMessages.Add(e);
        }
        public void Add(string errorSource, string errorText, Exception ex = null)
        {
            AddToErrors(errorSource, errorText, ex);
        }
        public void Add(IErrorMessage e)
        {
            AddToErrors(e);
        }
        public void ClearErrors()
        {
            errorMessages.Clear();
        }
        public IEnumerator<IErrorMessage> ForEach(Action<IErrorMessage> a)
        {
            foreach (IErrorMessage e in errorMessages)
            {
                yield return e;
            }
        }
        public IErrorMessage[] ToArray()
        {
            return errorMessages.ToArray();
        }
        public /*static*/ bool Copy(IErrorMessages source, IErrorMessages target)
        {
            target.errorMessages = new List<IErrorMessage>(source.errorMessages.ToArray());
            return true;
        }
        public bool CopyFrom(IErrorMessages source)
        {
            return Copy(source, this);
        }
        public bool CopyTo(IErrorMessages target)
        {
            return Copy(this, target);
        }
        public void LogErrors() //TODO: Add a logger interface dependency injection first
        {
            //IoUtility.AppendToFile(errorLogFilename, LogErrors, true);
        }
    }
}