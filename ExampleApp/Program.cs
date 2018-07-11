﻿using Config4Net.Core;
using Config4Net.Types;
using Config4Net.UI;
using Config4Net.UI.Containers;
using Config4Net.UI.Editors;
using Config4Net.UI.WinForms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ExampleApp
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Load winform implementation
            new WinFormFlatformLoader().Load();
            // Load config from file or memory
            var girlfriend = Config.Default.Get<GirlfriendSettings>();
            // Build UI from config instance
            var window = UiManager.Default.Build<IWindowContainer>(girlfriend);
            Config.Default.Get<GirlfriendSettings>();
            Application.Run((Form)window);
        }

        [Config]
        private class GirlfriendSettings
        {
            [Showable]
            public string Name { get; set; }

            [Showable]
            public int Age { get; set; }

            [Showable]
            public Color HairColor { get; set; }

            [Showable]
            public Contact Contact { get; set; }

            [Showable]
            public Note Note { get; set; }
        }

        private class Contact
        {
            [Showable]
            [Memo(VerticalScroll = true, Height = 50)]
            public string Address { get; set; }

            [Showable(Description = "Don't forget it!!")]
            public string Phone { get; set; }
        }

        private class Note
        {
            [Showable(ComponentType = typeof(IFilePickerEditor))]
            public string Avatar { get; set; }

            [Showable(ComponentType = typeof(ISelectEditor))]
            [Select(typeof(FavoriteSelectFactory))]
            public Favorite FavoriteType { get; set; }

            [Showable]
            public bool DoesSheComplainEveryday { get; set; }

            [Showable]
            public bool IsSheLazy { get; set; }

            [Showable(ComponentType = typeof(ITextEditor))]
            public IList<string> Hate { get; set; }
        }

        private class Favorite
        {
            public string Name { get; set; }
            public string Note { get; set; }

            public override string ToString()
            {
                return Name;
            }
        }

        private class FavoriteSelectFactory : ISelectFactory
        {
            public Select Create()
            {
                return new Select.Builder<Favorite>()
                    .AddOption(new Favorite { Name = "Makeup", Note = "Always compliment her beautiful" })
                    .AddOption(new Favorite { Name = "Eating", Note = "Remember all the sidewalk cafes" })
                    .AddOption(new Favorite { Name = "Sport", Note = "Take her to the gym every day" })
                    .AddOption(new Favorite { Name = "Shopping", Note = "Hummmn!!! What should I do?" })
                    .Build();
            }
        }
    }
}