namespace Config4Net.UI
{
    /// <summary>
    /// Load configurations for a specific flatform.
    /// </summary>
    public interface IFlatformLoader
    {
        /// <summary>
        /// Loads configs for <see cref="UI.UiManager.Default"/>.
        /// </summary>
        void Load();

        /// <summary>
        /// Loads configs for a specific <see cref="UiManager"/>.
        /// </summary>
        /// <param name="uiManager"></param>
        void Load(UiManager uiManager);
    }
}