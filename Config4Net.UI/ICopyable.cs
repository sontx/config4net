namespace Config4Net.UI
{
    public interface ICopyable<in T>
    {
        void Copy(T source);
    }
}