using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using Config4Net.UI.Layout;
using Config4Net.Utils;
using System.Windows.Forms;

namespace Config4Net.UI.WinForms.Layout
{
    internal sealed class FlowLayoutManager : FlowLayoutPanel, ILayoutManager
    {
        private LayoutOptions _layoutOptions;

        public LayoutOptions LayoutOptions
        {
            get => _layoutOptions;
            set
            {
                if (value != null)
                {
                    ApplyLayoutOptions(value);
                }

                _layoutOptions = value;
            }
        }

        public IReadOnlyCollection<IComponent> RegistedComponents
        {
            get
            {
                var list = new List<IComponent>(Controls.Count);
                list.AddRange(Controls.Cast<IComponent>());
                return new ReadOnlyCollection<IComponent>(list);
            }
        }

        public void Register(IComponent component)
        {
            Precondition.ArgumentNotNull(component, nameof(component));
            Precondition.ArgumentCompatibleType(component, typeof(Control), nameof(component));
            Precondition.PropertyNotNull(LayoutOptions, nameof(LayoutOptions));

            var control = (Control) component;
            control.Margin = Margin;
            control.Padding = Padding;
            Controls.Add(control);
        }

        public Size ComputeWholeRegion()
        {
            var width = 0;
            var height = 0;

            foreach (Control control in Controls)
            {
                width += control.Width;
                height += control.Height;
            }

            width += Measurement.ComputeInnerHorizontalSpace(this);

            height += Measurement.ComputeInnerVerticalSpace(this) +
                      Measurement.ComputeOuterVerticalSpace(this) * Controls.Count;

            return new Size(width, height);
        }

        private void ApplyLayoutOptions(LayoutOptions layoutOptions)
        {
            Padding = Compatibility.ToWinFormPadding(layoutOptions.Padding);
            Margin = Compatibility.ToWinFormPadding(layoutOptions.Margin);

            foreach (Control control in Controls)
            {
                control.Margin = Margin;
            }

            switch (layoutOptions.Orientation)
            {
                case Orientation.Default:
                    FlowDirection = FlowDirection.TopDown;
                    break;
                case Orientation.Horizontal:
                    FlowDirection = FlowDirection.LeftToRight;
                    break;
                case Orientation.Vertical:
                    FlowDirection = FlowDirection.TopDown;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public FlowLayoutManager()
        {
            AutoSize = true;
            Dock = DockStyle.Fill;
        }
    }
}