using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Config4Net.UI.WinForms.Editors
{
    public class UserControlDesigner : ParentControlDesigner
    {
        public override void Initialize(System.ComponentModel.IComponent component)
        {
            base.Initialize(component);

            if (this.Control is DefaultEditor editor)
            {
                this.EnableDesignMode(editor.WorkingArea, "WorkingArea");
            }
        }
        
    }
}