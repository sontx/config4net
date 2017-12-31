namespace Config4Net.UI
{
    /// <summary>
    /// Size mode.
    /// </summary>
    public enum SizeMode
    {
        /// <summary>
        /// Default size mode that depends on the implementation.
        /// </summary>
        Default,

        /// <summary>
        /// The size is computed by the specific size.
        /// </summary>
        Absolute,

        /// <summary>
        /// The size is computed by the parent size, it should be fit to the parent.
        /// </summary>
        MatchParent,

        /// <summary>
        /// The size is computed by the content size, it should be collapse to the content.
        /// </summary>
        WrapContent
    }
}