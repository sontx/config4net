using System;
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
        private SizeMode _sizeMode;
        private ContainerAppearance _appearance;

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

        public ContainerAppearance Appearance
        {
            get => _appearance;
            set
            {
                _appearance = value; 
                ComputeSize();
            }
        }

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

        public string Description { get; set; }

        public SizeMode SizeMode
        {
            get => _sizeMode;
            set
            {
                _sizeMode = value;
                ComputeSize();
            }
        }
        
        public void Bind()
        {
            if (_layoutManager == null) return;
            foreach (var component in _layoutManager.RegistedComponents)
            {
                component.Bind();
            }
        }

        public void SetSettings(Settings settings)
        {
        }

        protected override void OnAccept()
        {
            Bind();
        }

        private void ComputeSize()
        {
            if (_appearance == null || _layoutManager == null) return;

            Width = Appearance.Width;

            switch (_sizeMode)
            {
                case SizeMode.Default:
                    ComputeSizeWrapContent();
                    break;
                case SizeMode.Absolute:
                    ComputeSizeAbsolute();
                    break;
                case SizeMode.MatchParent:
                    ComputeSizeMatchParent();
                    break;
                case SizeMode.WrapContent:
                    ComputeSizeWrapContent();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ComputeSizeWrapContent()
        {
            // enable auto size mode of winform
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
        }

        private void ComputeSizeAbsolute()
        {
            AutoSize = false;

            Width = _appearance.Width;
            foreach (var component in _layoutManager.RegistedComponents)
            {
                component.SizeMode = SizeMode.MatchParent;
            }
        }

        private void ComputeSizeMatchParent()
        {
            ComputeSizeWrapContent();
        }   
    }
}