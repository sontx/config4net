using Config4Net;
using System;
using System.Drawing;

namespace ExampleApp
{
    internal class AppConfigFactory : IConfigObjectFactory
    {
        public object CreateDefault(Type fromType)
        {
            if (fromType == typeof(AppConfig))
            {
                return new AppConfig
                {
                    Config2 = 1,
                    Config1 = "Some value",
                    Config3 = new SubConfig
                    {
                        Config2 = Color.AliceBlue,
                        Config1 = DateTime.Now
                    }
                };
            }

            return null;
        }
    }

    internal class CustomConfigFactory : IConfigObjectFactory
    {
        public object CreateDefault(Type fromType)
        {
            if (fromType == typeof(CustomConfig))
            {
                return new CustomConfig
                {
                    Config1 = "default value",
                    Config2 = new SubConfig
                    {
                        Config1 = DateTime.Now,
                        Config2 = Color.Red
                    }
                };
            }

            return null;
        }
    }
}