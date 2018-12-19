using System;
using System.IO;
using CalcLib.BusinessLogic.Interfaces;
using CalcLib.Data;
using Newtonsoft.Json;

namespace CalcLib.BusinessLogic.Implementation
{
    public class SettingsReader : ISettingsReader
    {
        public Settings GetSettings()
        {
            var dir = AppDomain.CurrentDomain.BaseDirectory;
            var fileName = Path.Combine(dir, "Settings.json");

            if (!File.Exists(fileName))
            {
                return null;
            }


            string source;
            try
            {
                source = File.ReadAllText(fileName);
            }
            catch (Exception e)
            {
                return null;
            }

            Settings settings = JsonConvert.DeserializeObject<Settings>(source);
            return settings;
        }
    }
}