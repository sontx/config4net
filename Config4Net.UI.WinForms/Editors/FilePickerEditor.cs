using System.IO;
using Config4Net.UI.Editors;
using System.Reflection;
using System.Windows.Forms;

namespace Config4Net.UI.WinForms.Editors
{
    public partial class FilePickerEditor : DefaultEditor, IFilePickerEditor
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

        public FilePickerEditor()
        {
            InitializeComponent();
            _editorHelper = new EditorHelper<string>(this);
        }

        private void btnBrowse_Click(object sender, System.EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Description))
                openFileDialog1.Title = Description;

            var fileName = txtContent.Text;
            if (!string.IsNullOrWhiteSpace(fileName))
            {
                openFileDialog1.FileName = Path.GetFileName(fileName);
                openFileDialog1.InitialDirectory = Path.GetDirectoryName(fileName);
            }

            if (openFileDialog1.ShowDialog(FindForm()) == DialogResult.OK)
            {
                txtContent.Text = openFileDialog1.FileName;
            }
        }
    }
}