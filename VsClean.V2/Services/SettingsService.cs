using System;
using VsClean.V2.Properties;

namespace VsClean.V2.Services
{
    internal class SettingsService
    {
        public SettingsService()
        {
            Settings.Default.Upgrade();
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
