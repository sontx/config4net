using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Config4Net.UI.WinForms.Editors
{
    /// <summary>
    /// https://stackoverflow.com/a/27924873
    /// </summary>
    public class NoPaddingLabel : Label
    {
        private TextFormatFlags _flags = TextFormatFlags.SingleLine | TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.NoPadding;

        [Browsable(true)]
        public bool RightAlignment
        {
            get => (_flags & TextFormatFlags.Right) == TextFormatFlags.Right;
            set
            {
                if (value)
                {
                    _flags = _flags |= TextFormatFlags.Right;
                }
                else
                {
                    _flags = _flags &= ~TextFormatFlags.Right;
                }
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            TextRenderer.DrawText(e.Graphics, this.Text, this.Font, ClientRectangle, this.ForeColor, Color.Transparent, _flags);
        }
    }
}