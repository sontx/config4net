﻿namespace Config4Net.UI
{
    public interface IFlatformLoader
    {
        void Load();
        void Load(UiManager uiManager);
    }
}