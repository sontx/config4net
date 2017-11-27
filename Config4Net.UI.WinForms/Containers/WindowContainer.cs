using Config4Net.UI.Containers;
using Config4Net.UI.Layout;
using Config4Net.Utils;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Forms;

namespace Config4Net.UI.WinForms.Containers
{
    public class WindowContainer : ChooserForm, IWindowContainer
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
                    WorkspacePanel.Controls.Clear();

                    if (value != null)
                    {
                        Precondition.ArgumentCompatibleType(value, typeof(Control), nameof(LayoutManager));

                        var layoutControl = (Control)value;
                        WorkspacePanel.Controls.Add(layoutControl);
                        layoutControl.Dock = DockStyle.Fill;

                        AdjustButtons(value.LayoutOptions.Padding, value.LayoutOptions.Margin);
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

        public void Show(object owner)
        {
            if (owner != null)
            {
                Precondition.ArgumentCompatibleType(owner, typeof(IWin32Window), nameof(owner));
                base.Show((IWin32Window)owner);
            }
            else
            {
                base.Show();
            }
        }

        public bool ShowDialog(object owner)
        {
            Precondition.ArgumentNotNull(owner, nameof(owner));
            Precondition.ArgumentCompatibleType(owner, typeof(IWin32Window), nameof(owner));

            return base.ShowDialog((IWin32Window)owner) == DialogResult.OK;
        }

        public void Bind()
        {
            if (_layoutManager == null) return;
            foreach (var component in _layoutManager.RegistedComponents)
            {
                component.Bind();
            }
        }

        protected override void OnAccept()
        {
            Bind();
        }
    }
}