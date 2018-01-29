namespace Config4Net.UI
{
    /// <summary>
    /// Can copy from souce to instance.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICopyable<in T>
    {
        /// <summary>
        /// Copy from souce to current instance.
        /// </summary>
        /// <param name="source">Copy from this source.</param>
        void Copy(T source);
    }
}