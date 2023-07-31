using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mgm.VI.IO
{
    public class Error : IError
    {
        #region Private Variables
        private bool _isAnException = false;
        private string _description = string.Empty;
        private Exception _exceptionRaised;
        #endregion Private Variables

        #region Properties
        public bool IsAnException
        {
            get
            {
                return _isAnException;
            }
        }
        public string Description { get; set; }
        public Exception ExceptionRaised { get; private set; }
        #endregion Properties

        #region Constructors
        public Error(string description)
        {
            _description = description;
            _isAnException = false;
        }
        public Error(Exception exceptionRaised)
            : this(exceptionRaised.Message, exceptionRaised)
        { }
        public Error(string description, Exception exceptionRaised)
        {
            _description = description;
            _exceptionRaised = exceptionRaised;
            _isAnException = true;
        }
        #endregion Constructors

        #region Public Methods
        //public string ToString()
        //{
        //    if (_isAnException)
        //    {

        //    }
        //}
        #endregion Public Methods

        #region Static Methods
        //public static Error[] GetErrors(IError[] inputErrors)
        //{
        //    if (inputErrors == null) { return null; }

        //    var errors = new List<Error>(inputErrors.Length);

        //    Error error;
        //    foreach (var inputError in inputErrors)
        //    {
        //        error = new Error(inputError);

        //        errors.Add(error);
        //    }

        //    return errors.ToArray();
        //}
        #endregion Static Methods
    }
}
