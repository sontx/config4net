using Config4Net;

namespace ExampleApp
{
    [Config]
    internal class CustomConfig
    {
        public string Config1 { get; set; }
        public SubConfig Config2 { get; set; }
    }
}