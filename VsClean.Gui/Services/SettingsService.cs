using System;

using VsClean.Gui.Properties;

namespace VsClean.Gui.Services
{
    internal class SettingsService
    {
        public SettingsService()
        {
            Config = Settings.Default;
        }

        public Settings Config { get; }

        public Action BeforeSave { get; set; }

        public void Save()
        {
            BeforeSave.Invoke();
            Config.Save();
        }
    }
}
