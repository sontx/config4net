using Config4Net.Types;
using Config4Net.UI.Editors;
using Config4Net.Utils;
using System;

namespace Config4Net.UI.WinForms.Editors
{
    public partial class SelectEditor : DefaultEditor, ISelectEditor
    {
        private readonly EditorHelper<object> _editorHelper;

        private bool _readOnly;
        private object _value;

        public event ValueChangedEventHandler ValueChanged;

        public event ValueChangingEventHandler ValueChanging;

        public object Value
        {
            get => ((Select.Option)cmbContent.SelectedItem)?.Value;
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

            var selectAttribute = settings.Get<SelectAttribute>();
            if (selectAttribute == null) return;
            var selectFactory = (ISelectFactory)Activator.CreateInstance(selectAttribute.SelectFactoryType);
            var select = selectFactory.Create();
            cmbContent.BeginUpdate();
            cmbContent.Items.Clear();
            foreach (var selectOption in select.Options)
            {
                cmbContent.Items.Add(selectOption);
                if (ObjectUtils.DeepEquals(selectOption.Value, _value))
                {
                    cmbContent.SelectedItem = selectOption;
                }
            }
            cmbContent.EndUpdate();
            SetDirectValue(_editorHelper.GetValue());
        }

        private void SetDirectValue(object value)
        {
            foreach (Select.Option item in cmbContent.Items)
            {
                if (ObjectUtils.DeepEquals(item.Value, value))
                {
                    _value = value;
                    cmbContent.SelectedItem = item;
                    break;
                }
            }
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

        public SelectEditor()
        {
            InitializeComponent();
            _editorHelper = new EditorHelper<object>(this);
        }

        private void cmbContent_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_value != cmbContent.SelectedItem)
            {
                Value = cmbContent.SelectedItem;
            }
        }
    }
}