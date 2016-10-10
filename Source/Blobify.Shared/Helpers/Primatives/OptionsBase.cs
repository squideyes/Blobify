namespace Blobify.Shared.Helpers
{
    public abstract class OptionsBase
    {
        public LogLevel LogLevel { get; set; } = LogLevel.Info;

        [FileName("PARAMS", false)]
        public string ParamsFile { get; set; }

        protected virtual void Normalize()
        {
        }

        public abstract void ValidateDependencies();
    }
}
