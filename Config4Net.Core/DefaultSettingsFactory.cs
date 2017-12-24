using System;

namespace Config4Net.Core
{
    /// <summary>
    /// Default implementation for <see cref="ISettingsFactory"/> that creates a default
    /// <see cref="Settings"/> instance each request.
    /// </summary>
    public sealed class DefaultSettingsFactory : ISettingsFactory
    {
        /// <inheritdoc />
        public Settings Create()
        {
            return new Settings
            {
                AutoRegisterConfigType = true,
                AutoSaveWhenApplicationClosing = true,
                IgnoreMismatchType = true,
                PreventNullReference = false,
                IgnoreLoadFailure = true,
                ConfigDir = Environment.CurrentDirectory,
                AppConfigKey = "app",
                ConfigFileExtension = "json",
                WriteFileTimeout = 5000,
                ApplicationClosingEvent = new DefaultApplicationClosingEvent(),
                StoreService = new PlainTextStoreService()
            };
        }
    }
}