using NLog;

namespace Blobify.Shared.Helpers
{
    public abstract class OptionsBase
    {
        public OptionsBase()
        {
            LogLevel = LogLevel.Info;
        }

        public string ParamsFile { get; set; }

        public LogLevel LogLevel { get; set; }

        public bool IsValid => GetIsValid();

        protected abstract bool GetIsValid();
    }
}
