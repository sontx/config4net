using Config4Net.UI.Containers;
using Config4Net.UI.Layout;
using Config4Net.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Forms;

namespace Config4Net.UI.WinForms.Containers
{
    public sealed class GroupContainer : GroupBox, IGroupContainer
    {
        private ILayoutManager _layoutManager;
        private ContainerAppearance _appearance;
        private SizeMode _sizeMode;

        public IReadOnlyCollection<IComponent> Children => _layoutManager != null
            ? _layoutManager.RegistedComponents
            : new ReadOnlyCollection<IComponent>(new List<IComponent>(0));

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

                        var layoutControl = (Control)value;
                        Controls.Add(layoutControl);
                        layoutControl.Dock = DockStyle.Fill;
#if DEBUG_COLORS
                        layoutControl.BackColor = Color.Aqua;
#endif
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
            UpdateHeightByContent();
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

        private void UpdateHeightByContent()
        {
            var wholeRegion = _layoutManager.ComputeWholeRegion();
            var titleSize = TextRenderer.MeasureText(Text, Font);
            Height = wholeRegion.Height + Measurement.ComputeInnerVerticalSpace(this) + titleSize.Height;
        }

        private void ComputeSize()
        {
            if (_appearance == null || _layoutManager == null) return;

            switch (_sizeMode)
            {
                case SizeMode.Default:
                    ComputeSizeAbsolute();
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
            UpdateHeightByContent();

            foreach (var component in _layoutManager.RegistedComponents)
            {
                component.SizeMode = SizeMode.MatchParent;
            }
        }

        private void ComputeSizeMatchParent()
        {
            if (Parent == null) return;

            AutoSize = false;
            Width = Parent.Width - Parent.Padding.Left - Parent.Padding.Right - Margin.Left - Margin.Right;

            foreach (var component in _layoutManager.RegistedComponents)
            {
                component.SizeMode = SizeMode.MatchParent;
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            ComputeSize();
        }

        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
            ComputeSize();
        }
    }
}