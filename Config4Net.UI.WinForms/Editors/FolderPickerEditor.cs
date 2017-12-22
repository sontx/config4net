using System;
using System.IO;
using Config4Net.UI.Editors;
using System.Reflection;
using System.Windows.Forms;

namespace Config4Net.UI.WinForms.Editors
{
    public partial class FolderPickerEditor : DefaultEditor, IFolderPickerEditor
    {
        private readonly EditorHelper<string> _editorHelper;

        private bool _readOnly;
        private string _value;
        private FolderPickerAttribute _folderPickerAttribute;

        public event ValueChangedEventHandler ValueChanged;

        public event ValueChangingEventHandler ValueChanging;

        public string Value
        {
            get => _value;
            set
            {
                _editorHelper.ChangeValue(
                    value,
                    () =>
                    {
                        _value = value;

                        if (_folderPickerAttribute != null && _folderPickerAttribute.ShowFolderName && value != null)
                            txtContent.Text = Path.GetFileName(value);
                        else
                            txtContent.Text = value;
                    },
                    ValueChanging,
                    ValueChanged);
            }
        }

        public bool ReadOnly
        {
            get => _readOnly;
            set
            {
                if (_folderPickerAttribute == null || _folderPickerAttribute.TextEditable)
                    txtContent.ReadOnly = value;

                btnBrowse.Enabled = !value;
                _readOnly = value;
            }
        }

        public void SetReferenceInfo(object source, PropertyInfo propertyInfo)
        {
            _editorHelper.SetReferenceInfo(source, propertyInfo);

            var folderPickerAttribute = propertyInfo.GetCustomAttribute<FolderPickerAttribute>();

            if (folderPickerAttribute == null) return;

            txtContent.ReadOnly = !folderPickerAttribute.TextEditable;
            _folderPickerAttribute = folderPickerAttribute;

            Value = _value;
        }

        public void Bind()
        {
            _editorHelper.Bind();
        }

        public void Reset()
        {
            _editorHelper.Reset();
        }

        public override int PreferHeight
        {
            get => txtContent.Height;
            set => base.PreferHeight = value;
        }

        public FolderPickerEditor()
        {
            InitializeComponent();
            _editorHelper = new EditorHelper<string>(this);
        }

        private void btnBrowse_Click(object sender, System.EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Description))
                folderBrowserDialog1.Description = Description;

            folderBrowserDialog1.SelectedPath = _value;

            if (folderBrowserDialog1.ShowDialog(FindForm()) == DialogResult.OK)
            {
                Value = folderBrowserDialog1.SelectedPath;
                ChangeValueIfNecessary();
            }
        }

        private void ChangeValueIfNecessary()
        {
            if (string.Compare(txtContent.Text, _value, StringComparison.InvariantCultureIgnoreCase) != 0)
            {
                Value = txtContent.Text;
            }
        }

        private void txtContent_Leave(object sender, EventArgs e)
        {
            ChangeValueIfNecessary();
        }
    }
}