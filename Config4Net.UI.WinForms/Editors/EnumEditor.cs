using Config4Net.UI.Editors;
using Config4Net.Utils;
using System;

namespace Config4Net.UI.WinForms.Editors
{
    public partial class EnumEditor : DefaultEditor, IEnumEditor
    {
        private readonly EditorHelper<Enum> _editorHelper;

        private bool _readOnly;
        private Enum _value;

        public event ValueChangedEventHandler ValueChanged;

        public event ValueChangingEventHandler ValueChanging;

        public Enum Value
        {
            get => _value;
            set
            {
                _editorHelper.ChangeValue(
                    value,
                    () => { SetDirectValue(value); },
                    ValueChanging,
                    ValueChanged);
            }
        }

        public bool ReadOnly
        {
            get => _readOnly;
            set
            {
                cmbContent.Enabled = !value;
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

            var enumAttribute = settings.Get<EnumAttribute>();
            if (enumAttribute == null) return;
            var enumValues = Enum.GetValues(enumAttribute.EnumType);
            var enumerator = WrapperUtils.GetEnumerable<Enum>(enumValues.GetEnumerator());
            cmbContent.BeginUpdate();
            cmbContent.Items.Clear();
            foreach (var @enum in enumerator)
            {
                cmbContent.Items.Add(@enum);
                if (@enum.Equals(_value))
                {
                    cmbContent.SelectedItem = @enum;
                }
            }
            cmbContent.EndUpdate();
            SetDirectValue(_editorHelper.GetValue());
        }

        private void SetDirectValue(Enum value)
        {
            _value = value;
            cmbContent.SelectedItem = value;
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
            get => cmbContent.Height;
            set => base.PreferHeight = value;
        }

        public EnumEditor()
        {
            InitializeComponent();
            _editorHelper = new EditorHelper<Enum>(this);
        }

        private void cmbContent_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!Equals(_value, cmbContent.SelectedItem))
                Value = cmbContent.SelectedItem as Enum;
        }
    }
}