using System;

namespace Config4Net.Core.Adapter
{
    /// <summary>
    ///     Metadata for savable config.
    /// </summary>
    public interface IMetadata
    {
        /// <summary>
        ///     Config key.
        /// </summary>
        string Key { get; set; }

        /// <summary>
        ///     Represents config data type.
        /// </summary>
        string TypeId { get; set; }

        /// <summary>
        ///     Date and time that the config was modified.
        /// </summary>
        DateTime Modified { get; set; }
    }
}