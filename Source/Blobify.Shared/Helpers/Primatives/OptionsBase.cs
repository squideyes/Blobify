namespace Blobify.Shared.Helpers
{
    public abstract class OptionsBase
    {
        public LogLevel LogLevel { get; set; } = LogLevel.Info;

        [FileName("ARGS", false)]
        public string ArgsFile { get; set; }

        protected virtual void Normalize()
        {
        }

        public abstract void ValidateDependencies();
    }
}
