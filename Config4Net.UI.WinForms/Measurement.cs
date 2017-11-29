using System.Windows.Forms;

namespace Config4Net.UI.WinForms
{
    internal static class Measurement
    {
        public static int ComputeInnerHorizontalSpace(Control parent)
        {
            return parent.Padding.Left + parent.Padding.Right;
        }

        public static int ComputeInnerVerticalSpace(Control parent)
        {
            return parent.Padding.Top + parent.Padding.Bottom;
        }

        public static int ComputeOuterHorizontalSpace(Control parent)
        {
            return parent.Margin.Left + parent.Margin.Right;
        }

        public static int ComputeOuterVerticalSpace(Control parent)
        {
            return parent.Margin.Top + parent.Margin.Bottom;
        }
    }
}