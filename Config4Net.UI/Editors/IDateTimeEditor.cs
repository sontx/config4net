﻿using System;

namespace Config4Net.UI.Editors
{
    public interface IDateTimeEditor : IEditor<DateTime>
    {
        string DateFormat { get; set; }
    }
}