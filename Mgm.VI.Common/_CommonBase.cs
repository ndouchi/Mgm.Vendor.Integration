namespace Mgm.VI.Common
{
    public class CommonBase
    {
        public IErrorMessages ErrorMessages { get; private set; }
        public CommonBase (IErrorMessages errorMessages = null)
        {
            ErrorMessages = errorMessages ?? new Mgm.VI.Common.ErrorMessages();
        }
    }
}