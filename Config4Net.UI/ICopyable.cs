namespace Config4Net.UI
{
    /// <summary>
    /// Can copy from current instance to a giving source.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICopyable<in T>
    {
        /// <summary>
        /// Copy from current instance to a giving source.
        /// </summary>
        /// <param name="source">Copy to this source.</param>
        void Copy(T source);
    }
}