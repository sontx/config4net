namespace Config4Net.UI
{
    public interface IComponent
    {
        string Text { get; set; }

        SizeMode SizeMode { get; set; }

        void Bind();
    }
}