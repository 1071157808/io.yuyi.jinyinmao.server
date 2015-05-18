// FileInformation: nyanya/Infrastructure.Lib.CQRS/ConfigSource.cs
// CreatedTime: 2014/06/09   2:27 PM
// LastUpdatedTime: 2014/06/09   2:33 PM

namespace Infrastructure.Lib.CQRS.Config
{
    internal class ConfigSource : IConfigSource
    {
        private static readonly ConfigSection configSection = new ConfigSection();
        private static readonly LogSection logSection = new LogSection();

        #region IConfigSource Members

        public ConfigSection Config
        {
            get { return configSection; }
        }

        public LogSection Loggers
        {
            get { return logSection; }
        }

        #endregion IConfigSource Members
    }
}