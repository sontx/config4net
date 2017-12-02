using Config4Net.UI;
using Config4Net.UI.Containers;
using Config4Net.UI.Editors;
using Config4Net.UI.WinForms;
using System;
using System.Drawing;
using System.Windows.Forms;
using Config4Net.Core;

namespace ExampleApp
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new MainForm());

            new WinFormFlatformLoader().Load();
            var person = ConfigPool.Get<Person>();
            var window = UiManager.Default.Build<IWindowContainer>(person);

            Application.Run((Form) window);
        }

        [Showable("Your personal information")]
        [Config]
        class Person
        {
            [Showable("First name:", ComponentType = typeof(ITextEditor))]
            public string FirstName { get; set; }

            [Showable("Last name:", ComponentType = typeof(ITextEditor))]
            public string LastName { get; set; }

            [Showable(ComponentType = typeof(INumberEditor))]
            public int Age { get; set; }

            [Showable(ComponentType = typeof(IColorEditor))]
            public Color SkinColor { get; set; }

            [Showable("Sex:", ComponentType = typeof(ICheckboxEditor), Description = "You are male?")]
            public bool Male { get; set; }

            [Showable("Where are you now:")]
            public Address Address { get; set; }

            [Showable("Your job:")]
            public Job Job { get; set; }
        }

        class Address
        {
            [Showable(ComponentType = typeof(ITextEditor))]
            public string City { get; set; }

            [Showable(ComponentType = typeof(INumberEditor))]
            public int FaxCode { get; set; }

            public string InvisibleField { get; set; }
        }

        class Job
        {
            [Showable(ComponentType = typeof(ITextEditor))]
            public string Company { get; set; }

            [Showable(ComponentType = typeof(ITextEditor))]
            public string Position { get; set; }

            public int Salary { get; set; }
        }
    }
}