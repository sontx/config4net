using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Config4Net.UI.WinForms.Editors.Helpers
{
    /// <summary>
    ///     https://www.codeproject.com/Articles/5306/The-ColorPicker-WinForms-Control
    /// </summary>
    [DefaultProperty("Color")]
    [DefaultEvent("ColorChanged")]
    public sealed class ColorPicker : Label
    {
        //  Should the control display the color's name?
        private bool _textDisplayed = true;

        private readonly EditorService _editorService;

        //  The event is raised when the Color property changes.
        public event EventHandler ColorChanged;

        private const string DefaultColorName = "Black";

        [Description("The currently selected color.")]
        [Category("Appearance")]
        [DefaultValue(typeof(Color), DefaultColorName)]
        public Color Color
        {
            get => BackColor;
            set
            {
                SetColor(value);
                ColorChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        [Description("True meanse the control displays the currently selected color\'s name, False otherwise.")]
        [Category("Appearance")]
        [DefaultValue(true)]
        public bool TextDisplayed
        {
            get => _textDisplayed;
            set
            {
                _textDisplayed = value;
                SetColor(Color);
            }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override string Text
        {
            get => base.Text;
            set
            {
                base.Text = value;
                UpdateHeight();
            }
        }

        public override Font Font
        {
            get => base.Font;
            set
            {
                base.Font = value;
                UpdateHeight();
            }
        }

        public bool UnknownColorAsHex { get; set; }

        public ColorPicker(Color c)
        {
            TextAlign = ContentAlignment.MiddleCenter;
            SetColor(c);
            BorderStyle = BorderStyle.FixedSingle;

            _editorService = new EditorService(this);
        }

        public ColorPicker() :
            this(Color.FromName(DefaultColorName))
        {
        }

        private void UpdateHeight()
        {
            var size = TextRenderer.MeasureText(Text, Font);
            MinimumSize = new Size(0, size.Height + 5);
        }

        private void SetColor(Color color)
        {
            if (Color.Empty == color)
            {
                Text = _textDisplayed ? "Empty" : string.Empty;
                ForeColor = DefaultForeColor;
            }
            else
            {
                Text = _textDisplayed ? GetColorName(color) : string.Empty;
                ForeColor = GetInvertedColor(color);
            }

            BackColor = color;
        }

        private string GetColorName(Color color)
        {
            if (color.IsNamedColor || color.IsKnownColor) return color.Name;
            return UnknownColorAsHex ? $"#{color.Name}".ToUpper() : $"{color.R}, {color.G}, {color.B}";
        }

        //  Primitive color inversion.
        private Color GetInvertedColor(Color c)
        {
            return c.R + c.G + c.B > 255 * 3 / 2 ? Color.Black : Color.White;
        }

        protected override void OnClick(EventArgs e)
        {
            if (_editorService.IsShowing)
                CloseDropDown();
            else
                ShowDropDown();
            base.OnClick(e);
        }

        private void ShowDropDown()
        {
            try
            {
                //  This is the Color type editor - it displays the drop-down UI calling
                //  our IWindowsFormsEditorService implementation.
                var editor = new System.Drawing.Design.ColorEditor();
                //  Display the UI.
                var newValue = editor.EditValue(_editorService, Color);
                //  If the user didn't cancel the selection, remember the new color.
                if (newValue != null && !_editorService.Canceled)
                    Color = (Color)newValue;
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.ToString());
            }
        }

        private void CloseDropDown()
        {
            _editorService.CloseDropDown();
        }

        //  This is a simple Form descendant that hosts the drop-down control provided
        //  by the ColorEditor class (in the call to DropDownControl).
        private class DropDownForm : Form
        {
            //  did the user cancel the color selection?
            private bool _closeDropDownCalled;

            //  was the form closed by calling the CloseDropDown method?
            public DropDownForm()
            {
                FormBorderStyle = FormBorderStyle.None;
                ShowInTaskbar = false;
                KeyPreview = true;
                StartPosition = FormStartPosition.Manual;
                //  The ColorUI control is hosted by a Panel, which provides the simple border frame
                //  not available for Forms.
                var p = new Panel
                {
                    BorderStyle = BorderStyle.FixedSingle,
                    Dock = DockStyle.Fill
                };
                Controls.Add(p);
            }

            public void SetControl(Control ctl)
            {
                Controls[0].Controls.Add(ctl);
            }

            public bool Canceled { get; private set; }

            public void CloseDropDown()
            {
                _closeDropDownCalled = true;
                Hide();
            }

            protected override void OnKeyDown(KeyEventArgs e)
            {
                base.OnKeyDown(e);
                if (e.Modifiers == 0
                    && e.KeyCode == Keys.Escape)
                    Hide();
            }

            protected override void WndProc(ref Message m)
            {
                //134 = WM_NCACTIVATE
                if (m.Msg == 134)
                {
                    //Check if other app is activating - if so, we do not close
                    if (m.LParam == IntPtr.Zero)
                    {
                        if (m.WParam != IntPtr.Zero)
                        {
                            Close();
                            return;
                        }
                    }
                }

                base.WndProc(ref m);
            }

            protected override void OnDeactivate(EventArgs e)
            {
                //  We set the Owner to Nothing BEFORE calling the base class.
                //  If we wouldn't do it, the Picker form would lose focus after
                //  the dropdown is closed.
                Owner = null;
                base.OnDeactivate(e);
                //  If the form was closed by any other means as the CloseDropDown call, it is because
                //  the user clicked outside the form, or pressed the ESC key.
                if (!_closeDropDownCalled)
                    Canceled = true;

                Close();
            }
        }

        //  This class actually hosts the ColorEditor.ColorUI by implementing the
        //  IWindowsFormsEditorService.
        private class EditorService : IWindowsFormsEditorService, IServiceProvider
        {
            //  The associated color picker control.
            private readonly ColorPicker _picker;

            //  Reference to the drop down, which hosts the ColorUI control.
            private DropDownForm _dropDownHolder;

            public bool IsShowing { get; private set; }

            //  Cached _DropDownHolder.Canceled flag in order to allow it to be inspected
            //  by the owning ColorPicker control.

            public EditorService(ColorPicker owner)
            {
                _picker = owner;
            }

            public bool Canceled { get; private set; }

            public void CloseDropDown()
            {
                _dropDownHolder?.CloseDropDown();
                IsShowing = false;
            }

            public void DropDownControl(Control control)
            {
                IsShowing = true;

                Canceled = false;
                _dropDownHolder = new DropDownForm { Bounds = control.Bounds };
                _dropDownHolder.Closed += _dropDownHolder_Closed;
                _dropDownHolder.SetControl(control);
                //  Lookup a parent form for the Picker control and make the dropdown form to be owned
                //  by it. This prevents to show the dropdown form icon when the user presses the At+Tab system
                //  key while the dropdown form is displayed.
                var pickerForm = GetParentForm(_picker);
                if (pickerForm is Form form)
                    _dropDownHolder.Owner = form;

                //  Ensure the whole drop-down UI is displayed on the screen and show it.
                PositionDropDownHolder();
                _dropDownHolder.ShowDialog();

                //  Remember the cancel flag and get rid of the drop down form.
                Canceled = _dropDownHolder.Canceled;
                _dropDownHolder.Dispose();
                _dropDownHolder.Closed -= _dropDownHolder_Closed;
                _dropDownHolder = null;
            }

            private void _dropDownHolder_Closed(object sender, EventArgs e)
            {
                IsShowing = false;
            }

            public DialogResult ShowDialog(Form dialog)
            {
                throw new NotSupportedException();
            }

            public object GetService(Type serviceType)
            {
                return serviceType == typeof(IWindowsFormsEditorService) ? this : null;
            }

            private void PositionDropDownHolder()
            {
                //  Convert _Picker location to screen coordinates.
                var loc = _picker.Parent.PointToScreen(_picker.Location);
                var screenRect = Screen.FromControl(_picker.Parent).WorkingArea;
                //  Position the dropdown X coordinate in order to be displayed in its entirety.
                if (loc.X < screenRect.X)
                    loc.X = screenRect.X;
                else if (loc.X + _dropDownHolder.Width
                         > screenRect.Right)
                    loc.X = screenRect.Right - _dropDownHolder.Width;

                //  Do the same for the Y coordinate.
                if (loc.Y + _picker.Height + _dropDownHolder.Height
                    > screenRect.Bottom)
                    loc.Offset(0, _dropDownHolder.Height * -1);
                else
                    loc.Offset(0, _picker.Height);

                _dropDownHolder.Location = loc;
            }

            private Control GetParentForm(Control ctl)
            {
                return ctl.FindForm();
            }
        }

        //  No need to display ForeColor and BackColor and Text in the property browser:
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override Color ForeColor
        {
            get => base.ForeColor;
            set => base.ForeColor = value;
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override Color BackColor
        {
            get => base.BackColor;
            set => base.BackColor = value;
        }
    }
}