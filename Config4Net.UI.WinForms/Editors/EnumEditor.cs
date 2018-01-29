using Config4Net.UI.Editors;
using Config4Net.UI.Editors.Definations;
using Config4Net.Utils;
using System;
using System.Collections.Generic;

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

        private void SetDefinationType(Type definationType)
        {
            Precondition.ArgumentCompatibleType(definationType, typeof(EnumDefination), nameof(definationType));
            var defination = ((IDefinationType)Activator.CreateInstance(definationType)).GetDefination();

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

        public void SetReferenceInfo(ReferenceInfo referenceInfo)
        {
            _editorHelper.SetReferenceInfo(referenceInfo);
        }

        public override void SetSettings(Settings settings)
        {
            base.SetSettings(settings);
            var definationAttribute = settings.Get<DefinationAttribute>();
            if (definationAttribute != null && definationAttribute.Value != null)
                SetDefinationType(definationAttribute.Value);
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