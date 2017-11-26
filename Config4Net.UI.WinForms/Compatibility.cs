namespace Config4Net.UI.WinForms
{
    internal static class Compatibility
    {
        public static System.Windows.Forms.Padding ToWinFormPadding(Padding padding)
        {
            return new System.Windows.Forms.Padding(padding.Left, padding.Top, padding.Right, padding.Bottom);
        }
    }
}