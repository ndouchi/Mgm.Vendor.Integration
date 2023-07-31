using Mgm.VI.Common;

namespace Mgm.VI.Repository
{
    public abstract class UowBase
    {
        public readonly string LoggedTimestamp = System.DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
        public readonly string ApplicationPath = System.IO.Directory.GetCurrentDirectory();
        public readonly string ApplicationName = "MGM-VU Integration";
        protected readonly Storage storage;

    }
}