namespace Blobify.Shared.Helpers
{
    public abstract class OptionsBase
    {
        public LogLevel LogLevel { get; set; } = LogLevel.Info;

        public string ParamsFile { get; set; }

        public abstract void Validate();

        protected virtual void Normalize()
        {
        }
    }
}
