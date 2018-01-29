using Config4Net.UI.Editors;
using Config4Net.UI.Layout;
using Config4Net.UI.WinForms.Editors.Helpers;
using Config4Net.UI.WinForms.Layout;
using Config4Net.Utils;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;

namespace Config4Net.UI.WinForms.Editors
{
    public partial class ListEditor : DefaultEditor, IListEditor
    {
        private readonly EditorHelper<IList<object>> _editorHelper;
        private bool _readOnly;
        private IList<object> _value;
        private Func<IComponent> _itemFactory;
        private Func<object, PropertyInfo, Settings> _settingFactory;
        private ISettingBinder _settingBinder;

        public event ValueChangedEventHandler ValueChanged;

        public event ValueChangingEventHandler ValueChanging;

        public IList<object> Value
        {
            get => _value;
            set
            {
                _editorHelper?.ChangeValue(
                    value,
                    () =>
                    {
                        _value = value;
                        labContent.Text = $@"[{value?.Count ?? 0}] Click to edit...";
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
                labContent.Enabled = !value;
                _readOnly = value;
            }
        }

        public void SetReferenceInfo(ReferenceInfo referenceInfo)
        {
            _editorHelper.SetReferenceInfo(referenceInfo);
        }

        public void Bind()
        {
            _editorHelper?.Bind();
        }

        public void Reset()
        {
            _editorHelper?.Reset();
        }

        public void SetItemFactory(Func<IComponent> itemFactory)
        {
            Precondition.ArgumentNotNull(itemFactory, nameof(itemFactory));
            _itemFactory = itemFactory;
        }

        public void SetSettingBinder(ISettingBinder binder)
        {
            _settingBinder = binder;
        }

        public void SetSettingFactory(Func<object, PropertyInfo, Settings> settingFactory)
        {
            _settingFactory = settingFactory;
        }

        public override int PreferHeight
        {
            get => labContent.Height;
            set => base.PreferHeight = value;
        }

        public ListEditor()
        {
            InitializeComponent();
            _editorHelper = new EditorHelper<IList<object>>(this);
        }

        private void labContent_Click(object sender, EventArgs e)
        {
            if (_itemFactory == null || _settingBinder == null) return;

            var listForm = new ListForm { Text = Text };
            var layoutManagerFactory = GetSettings().Get<ILayoutManagerFactory>();
            listForm.SetItemFactory(CreateItem);
            listForm.SetLayoutManager((FlowLayoutManager)layoutManagerFactory.Create());
            listForm.InitializeList(Value);

            if (listForm.ShowDialog(this) == DialogResult.OK)
            {
                Value = listForm.ItemValues;
            }
        }

        private Control CreateItem(object initValue)
        {
            var memberInfo = typeof(Dummy).GetProperty("Value");
            var settings = _settingFactory(new Dummy(), memberInfo);
            settings.Put("text", Text);
            settings.Put("description", Description);

            var item = _itemFactory();
            _settingBinder.BindEditor(item, memberInfo, settings);
            item.SizeMode = SizeMode;
            EditorWrapper.From(item).Value = initValue;

            return (Control)item;
        }

        private class Dummy
        {
            public object Value { get; set; }
        }
    }
}