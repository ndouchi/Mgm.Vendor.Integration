using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Mgm.VI.Common
{
    public interface IErrorMessages : IEnumerable<IErrorMessage>
    {
        List<IErrorMessage> errorMessages { get; set; }
        IEnumerator<IErrorMessage> GetEnumerator();
        void AddToErrors(string errorSource, string errorText, Exception ex = null);
        void AddToErrors(IErrorMessage e);
        void Add(string errorSource, string errorText, Exception ex = null);
        void Add(IErrorMessage e);
        void ClearErrors();
        IEnumerator<IErrorMessage> ForEach(Action<IErrorMessage> a);
        IErrorMessage[] ToArray();
        bool Copy(IErrorMessages source, IErrorMessages target);//Static
        bool CopyFrom(IErrorMessages source);
        bool CopyTo(IErrorMessages target);
    }
}
