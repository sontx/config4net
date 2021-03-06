﻿using Config4Net.UI.Editors;
using System;
using System.IO;
using System.Windows.Forms;

namespace Config4Net.UI.WinForms.Editors
{
    public partial class FilePickerEditor : DefaultEditor, IFilePickerEditor
    {
        private readonly EditorHelper<string> _editorHelper;

        private bool _readOnly;
        private string _value;
        private FilePickerAttribute _filePickerAttribute;

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

                        if (_filePickerAttribute != null && _filePickerAttribute.ShowFileName && value != null)
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
                if (_filePickerAttribute == null || _filePickerAttribute.TextEditable)
                    txtContent.ReadOnly = value;

                btnBrowse.Enabled = !value;
                _readOnly = value;
            }
        }

        public void SetReferenceInfo(ReferenceInfo referenceInfo)
        {
            _editorHelper.SetReferenceInfo(referenceInfo);
        }

        public override void SetSettings(Settings settings)
        {
            base.SetSettings(settings);

            var filePickerAttribute = settings.Get<FilePickerAttribute>();

            if (filePickerAttribute == null) return;

            txtContent.ReadOnly = !filePickerAttribute.TextEditable;
            openFileDialog1.Filter = filePickerAttribute.FileFilter;
            _filePickerAttribute = filePickerAttribute;
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

            var fileName = _value;
            if (!string.IsNullOrWhiteSpace(fileName))
            {
                openFileDialog1.FileName = Path.GetFileName(fileName);
                openFileDialog1.InitialDirectory = Path.GetDirectoryName(fileName);
            }

            if (openFileDialog1.ShowDialog(FindForm()) == DialogResult.OK)
            {
                Value = openFileDialog1.FileName;
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