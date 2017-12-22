namespace Config4Net.UI
{
    public interface IComponent
    {
        string Name { get; set; }

        string Text { get; set; }

        string Description { get; set; }

        SizeMode SizeMode { get; set; }

        void Bind();
    }
}