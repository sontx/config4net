using Config4Net.UI.WinForms.Layout;
using Config4Net.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Config4Net.UI.WinForms.Editors.Helpers
{
    internal partial class ListForm : Form
    {
        private Func<object, Control> _itemContentFactory;
        private FlowLayoutManager _layoutManager;

        public IList<object> ItemValues => (from ListItem listItem in _layoutManager.Controls select listItem.Value).ToList();

        public void InitializeList(IList<object> list)
        {
            if (list == null) return;

            foreach (var itemValue in list)
            {
                AddItem(itemValue);
            }
        }

        public void SetLayoutManager(FlowLayoutManager layoutManager)
        {
            Precondition.ArgumentNotNull(layoutManager, nameof(layoutManager));
            if (_layoutManager != null)
                panel1.Controls.Remove(_layoutManager);

            _layoutManager = layoutManager;
            panel1.Controls.Add(_layoutManager);
            _layoutManager.Dock = DockStyle.Fill;
            _layoutManager.AutoSize = false;
            _layoutManager.AutoScroll = true;
            _layoutManager.WrapContents = false;
            _layoutManager.BringToFront();

            UpdateWidth();
        }

        public void SetItemFactory(Func<object, Control> itemContentFactory)
        {
            Precondition.ArgumentNotNull(itemContentFactory, nameof(itemContentFactory));
            _itemContentFactory = itemContentFactory;
        }

        public ListForm()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (_layoutManager == null) return;
            AddItem(null);
        }

        private void Item_Removed(object sender, EventArgs e)
        {
            var item = (ListItem) sender;
            item.Removed -= Item_Removed;
            _layoutManager?.Controls.Remove(item);
            UpdateWidth();
            UpdateItemText();
        }

        private void AddItem(object value)
        {
            if (_layoutManager == null) return;
            
            var item = new ListItem();
            item.SetEditor((DefaultEditor)_itemContentFactory(value));
            item.Removed += Item_Removed;
            _layoutManager.Controls.Add(item);

            UpdateWidth();
            UpdateItemText();
            _layoutManager.ScrollControlIntoView(item);
        }

        private void UpdateWidth()
        {
            var region = _layoutManager.ComputeWholeRegion();
            Width = region.Width + (_layoutManager.VerticalScroll.Visible ? SystemInformation.VerticalScrollBarWidth : 0);
        }

        private void UpdateItemText()
        {
            for (var i = 0; i < _layoutManager.Controls.Count; i++)
            {
                var control = (ListItem)_layoutManager.Controls[i];
                control.Text = $@"{Text} {i + 1}";
            }
        }
    }
}