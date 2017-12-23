using System;

namespace Config4Net.Core
{
    public sealed class DefaultSettingsFactory : ISettingsFactory
    {
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