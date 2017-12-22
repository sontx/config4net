using Config4Net.UI.Editors.Definations;
using System.Reflection;

namespace Config4Net.UI
{
    public class ReferenceInfo
    {
        public PropertyInfo PropertyInfo { get; set; }
        public object Source { get; set; }
    }

    public abstract class BindInfo
    {
        public string Name { get; set; }
        public SizeOptions SizeOptions { get; set; }
        public ShowableInfo ShowableInfo { get; set; }
    }

    public class ContainerBindInfo : BindInfo
    {
    }

    public class EditorBindInfo : BindInfo
    {
        public DefinationInfo DefinationInfo { get; set; }
        public ReferenceInfo ReferenceInfo { get; set; }
    }
}