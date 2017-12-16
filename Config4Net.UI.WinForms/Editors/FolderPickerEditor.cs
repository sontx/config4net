using Config4Net.UI.Editors;
using System.Reflection;
using System.Windows.Forms;

namespace Config4Net.UI.WinForms.Editors
{
    public partial class FolderPickerEditor : DefaultEditor, IFolderPickerEditor
    {
        private readonly EditorHelper<string> _editorHelper;

        private bool _readOnly;

        public event ValueChangedEventHandler ValueChanged;

        public event ValueChangingEventHandler ValueChanging;

        public string Value
        {
            get => txtContent.Text;
            set
            {
                _editorHelper.ChangeValue(
                    value,
                    () => { txtContent.Text = value; },
                    ValueChanging,
                    ValueChanged);
            }
        }

        public bool ReadOnly
        {
            get => _readOnly;
            set
            {
                txtContent.ReadOnly = value;
                _readOnly = value;
            }
        }

        public void SetReferenceInfo(object source, PropertyInfo propertyInfo)
        {
            _editorHelper.SetReferenceInfo(source, propertyInfo);
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

            folderBrowserDialog1.SelectedPath = txtContent.Text;

            if (folderBrowserDialog1.ShowDialog(FindForm()) == DialogResult.OK)
            {
                txtContent.Text = folderBrowserDialog1.SelectedPath;
            }
        }
    }
}