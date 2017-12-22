using Config4Net.UI.Editors;
using Config4Net.UI.Editors.Definations;
using Config4Net.Utils;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Config4Net.UI.WinForms.Editors
{
    public partial class EnumEditor : DefaultEditor, IEnumEditor
    {
        private readonly EditorHelper<Enum> _editorHelper;

        private bool _readOnly;
        private Type _definationType;
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
                    () => 
                    {
                        _value = value;
                        cmbContent.SelectedItem = value;
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

                Precondition.ArgumentCompatibleType(value, typeof(EnumDefination), nameof(DefinationType));
                var defination = ((IDefinationType)Activator.CreateInstance(value)).GetDefination();

                var enumerator = (IEnumerable<Enum>)defination;
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