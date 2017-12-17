using Config4Net.UI.Editors;
using Config4Net.UI.Layout;
using Config4Net.UI.WinForms.Editors.Helpers;
using Config4Net.UI.WinForms.Layout;
using Config4Net.Utils;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using Config4Net.UI.Editors.Definations;

namespace Config4Net.UI.WinForms.Editors
{
    public partial class ListEditor : DefaultEditor, IListEditor
    {
        private readonly EditorHelper<IList<object>> _editorHelper;
        private bool _readOnly;
        private IList<object> _value;
        private Func<IComponent> _itemFactory;
        private Func<ILayoutManager> _layoutManagerFactory;
        private IUiBinder _uiBinder;

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

        public void SetReferenceInfo(object source, PropertyInfo propertyInfo)
        {
            _editorHelper.SetReferenceInfo(source, propertyInfo);
        }

        public void Bind()
        {
            _editorHelper?.Bind();
        }

        public void Reset()
        {
            _editorHelper?.Reset();
        }

        public void SetUiBinder(IUiBinder binder)
        {
            _uiBinder = binder;
        }
        
        public void SetItemFactory(Func<IComponent> itemFactory)
        {
            Precondition.ArgumentNotNull(itemFactory, nameof(itemFactory));
            _itemFactory = itemFactory;
        }

        public void SetLayoutManagerFactory(Func<ILayoutManager> layoutManagerFactory)
        {
            Precondition.ArgumentNotNull(layoutManagerFactory, nameof(layoutManagerFactory));
            _layoutManagerFactory = layoutManagerFactory;
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
            if (_itemFactory == null || _uiBinder == null) return;

            var listForm = new ListForm {Text = Text};

            listForm.SetItemFactory(CreateItem);
            listForm.SetLayoutManager((FlowLayoutManager)_layoutManagerFactory());
            listForm.InitializeList(Value);

            if (listForm.ShowDialog(this) == DialogResult.OK)
            {
                Value = listForm.ItemValues;
            }
        }

        private Control CreateItem(object initValue)
        {
            var item = _itemFactory();
            _uiBinder.BindEditor(item, new EditorBindInfo
            {
                ShowableInfo = new ShowableInfo
                {
                    Label = Text,
                    Description = Description
                },
                DefinationInfo = new DefinationInfo
                {
                    Value = DefinationType
                },
                SizeOptions = new SizeOptions
                {
                    EditorSizeMode = SizeMode
                },
                ReferenceInfo = new ReferenceInfo
                {
                    Source = new Dummy(),
                    PropertyInfo = typeof(Dummy).GetProperty("Value")
                }
            });

            EditorWrapper.From(item).Value = initValue;

            return (Control) item;
        }

        private class Dummy
        {
            public object Value { get; set; }
        }
    }
}