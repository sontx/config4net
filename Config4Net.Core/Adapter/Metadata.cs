using System;

namespace Config4Net.Core.Adapter
{
    /// <inheritdoc />
    public class Metadata : IMetadata
    {
        /// <inheritdoc />
        public string Key { get; set; }

        /// <inheritdoc />
        public string TypeId { get; set; }

        /// <inheritdoc />
        public DateTime Modified { get; set; }
    }
}