using Config4Net.Core;
using Config4Net.Types;
using Config4Net.UI;
using Config4Net.UI.Containers;
using Config4Net.UI.Editors;
using Config4Net.UI.Editors.Definations;
using Config4Net.UI.WinForms;
using System;
using System.Drawing;
using System.Windows.Forms;

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
            ConfigPool.Default.Settings.IgnoreMismatchType = true;
            var person = ConfigPool.Default.Get<Person>();
            var window = UiManager.Default.Build<IWindowContainer>(person);

            Application.Run((Form)window);
        }

        private enum Gender
        {
            Male,
            Female,
            Unkown
        }

        private class MyEnumDefination: EnumDefination
        {
            public MyEnumDefination() : base(typeof(Gender))
            {
            }
        }

        private class MyValue
        {
            public string Value1 { get; set; }
            public int Value2 { get; set; }

            public override string ToString()
            {
                return Value1;
            }
        }

        private class MySelectDefination : SelectDefination
        {
            protected override Select GetSelect()
            {
                return new Select.Builder()
                    .AddOption("Last name 1", new MyValue { Value1 = "value 1", Value2 = 1 })
                    .AddOption("Last name 2", new MyValue { Value1 = "value 2", Value2 = 2 })
                    .AddOption("Last name 3", new MyValue { Value1 = "value 3", Value2 = 3 })
                    .Build();
            }
        }

        [Showable("Your personal information")]
        [Config]
        private class Person
        {
            [Showable("First name:", ComponentType = typeof(ITextEditor))]
            [Default("son")]
            public string FirstName { get; set; }

            [Showable("Last name:", ComponentType = typeof(ISelectEditor))]
            [Defination(typeof(MySelectDefination))]
            public MyValue LastName { get; set; }

            [Showable(ComponentType = typeof(IDateEditor))]
            public DateTime Birth { get; set; }

            [Showable(ComponentType = typeof(IColorEditor))]
            public Color SkinColor { get; set; }

            [Showable("Sex:", ComponentType = typeof(IEnumEditor))]
            [Defination(typeof(MyEnumDefination))]
            public Gender Male { get; set; }

            [Showable("Where are you now:")]
            public Address Address { get; set; }

            [Showable("Your job:")]
            public Job Job { get; set; }
        }

        private class Address
        {
            [Showable(ComponentType = typeof(ITextEditor))]
            public string City { get; set; }

            [Showable(ComponentType = typeof(INumberEditor))]
            public int FaxCode { get; set; }

            public string InvisibleField { get; set; }
        }

        private class Job
        {
            [Showable(ComponentType = typeof(ITextEditor))]
            public string Company { get; set; }

            [Showable(ComponentType = typeof(ITextEditor))]
            public string Position { get; set; }

            [Showable(ComponentType = typeof(ITimeEditor))]
            public DateTime WorkTime { get; set; }

            [Showable(ComponentType = typeof(IDateTimeEditor))]
            public DateTime SomeDateTime { get; set; }

            public int Salary { get; set; }
        }
    }
}