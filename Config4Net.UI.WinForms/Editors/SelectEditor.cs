using Config4Net.Types;
using Config4Net.UI.Editors;
using Config4Net.UI.Editors.Definations;
using Config4Net.Utils;
using System;
using System.Reflection;

namespace Config4Net.UI.WinForms.Editors
{
    public partial class SelectEditor : DefaultEditor, ISelectEditor
    {
        private readonly EditorHelper<object> _editorHelper;

        private bool _readOnly;
        private Type _definationType;
        private object _value;

        public event ValueChangedEventHandler ValueChanged;

        public event ValueChangingEventHandler ValueChanging;

        public object Value
        {
            get => ((Select.Option)cmbContent.SelectedItem)?.Value;
            set
            {
                _value = value;
                _editorHelper.ChangeValue(
                    value,
                    () =>
                    {
                        foreach (Select.Option item in cmbContent.Items)
                        {
                            if (ObjectUtils.DeepEquals(item.Value, value))
                            {
                                cmbContent.SelectedItem = item;
                            }
                        }
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
                cmbContent.Enabled = !value;
                _readOnly = value;
            }
        }

        public DisplayMode DisplayMode { get; set; }

        public override Type DefinationType
        {
            get => _definationType;
            set
            {
                Precondition.PropertyNotNull(value, nameof(DefinationType));
                _definationType = value;

                Precondition.ArgumentCompatibleType(value, typeof(SelectDefination), nameof(DefinationType));
                var defination = ((IDefinationType)Activator.CreateInstance(value)).GetDefination();

                var select = (Select)defination;
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
            get => cmbContent.Height;
            set => base.PreferHeight = value;
        }

        public SelectEditor()
        {
            InitializeComponent();
            _editorHelper = new EditorHelper<object>(this);
        }
    }
}