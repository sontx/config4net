using Config4Net;
using System;
using System.Drawing;
using Config4Net.Core;

namespace ExampleApp
{
    [Config(IsAppConfig = true)]
    public class AppConfig
    {
        public string Config1 { get; set; }
        public int Config2 { get; set; }
        public SubConfig Config3 { get; set; }
    }

    public class SubConfig
    {
        public DateTime Config1 { get; set; }
        public Color Config2 { get; set; }
    }
}