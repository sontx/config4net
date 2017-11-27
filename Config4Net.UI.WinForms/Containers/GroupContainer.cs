using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows.Forms;
using Config4Net.UI.Containers;
using Config4Net.UI.Layout;
using Config4Net.Utils;

namespace Config4Net.UI.WinForms.Containers
{
    public sealed class GroupContainer : GroupBox, IGroupContainer
    {
        private ILayoutManager _layoutManager;

        public IReadOnlyCollection<IComponent> Children => _layoutManager != null ? _layoutManager.RegistedComponents : new ReadOnlyCollection<IComponent>(new List<IComponent>(0));

        public ILayoutManager LayoutManager
        {
            get => _layoutManager;
            set
            {
                if (value != _layoutManager)
                {
                    Controls.Clear();

                    if (value != null)
                    {
                        Precondition.ArgumentCompatibleType(value, typeof(Control), nameof(LayoutManager));

                        var layoutControl = (Control) value;
                        Controls.Add(layoutControl);
                        layoutControl.Dock = DockStyle.Fill;
                        layoutControl.Padding = new System.Windows.Forms.Padding(layoutControl.Padding.All / 2);
                    }
                }

                _layoutManager = value;
            }
        }

        public ContainerAppearance Appearance { get; set; }

        public void AddChild(IComponent component)
        {
            Precondition.ArgumentNotNull(component, nameof(component));
            Precondition.ArgumentCompatibleType(component, typeof(Control), nameof(component));
            
            LayoutManager?.Register(component);
        }

        public void Bind()
        {
            if (_layoutManager == null) return;
            foreach (var component in _layoutManager.RegistedComponents)
            {
                component.Bind();
            }
        }

        public GroupContainer()
        {
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
        }
    }
}