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
                ConfigDir = Environment.CurrentDirectory,
                ApplicationClosingEvent = new DefaultApplicationClosingEvent()
            };
        }
    }
}