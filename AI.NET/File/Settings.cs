﻿using AI.NET.Logger;
using System.IO;
using System.Text.Json;

namespace AI.NET.File
{
    internal static class Settings
    {
        private static readonly string settingsPath =
            AppDomain.CurrentDomain.BaseDirectory + "\\settings.json";
        public static bool SettingsExist => System.IO.File.Exists(settingsPath);
        /// <summary>
        /// Get the current settings
        /// </summary>
        /// <returns></returns>
        public static SettingsModel GetCurrentSettings()
        {
            SettingsModel settings = new()
            {
                Mem0 = Service.AI.Mem0,
                OpenAI = Service.AI.OpenAI
            };
            return settings;
        }
        /// <summary>
        /// Apply the settings
        /// </summary>
        /// <param name="model"></param>
        public static void ApplySettings(SettingsModel model)
        {
            Service.AI.Mem0 = model.Mem0;
            Service.AI.OpenAI = model.OpenAI;
        }
        /// <summary>
        /// Save settings to file
        /// </summary>
        public static void SaveSettings(SettingsModel model)
        {
            // Save settings to file
            try
            {

                using (StreamWriter writer = new(settingsPath))
                {
                    writer.Write(JsonSerializer.Serialize(model));
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error saving settings", ex);
                throw;
            }
        }
        /// <summary>
        /// Read settings from file
        /// </summary>
        /// <returns></returns>
        /// <exception cref="FileFormatException">The setting file is invalid</exception>
        public static SettingsModel ReadSettings()
        {
            try
            {
                using (StreamReader reader = new(settingsPath))
                {
                    SettingsModel model =
                        JsonSerializer.Deserialize<SettingsModel>(reader.ReadToEnd()) ??
                        throw new FileFormatException(new Uri(settingsPath), "Error reading settings");
                    return model;
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error reading settings", ex);
                throw;
            }
        }
    }

    internal class SettingsModel
    {
        public required Network.AI.OpenAI OpenAI { get; set; }
        public required Network.AI.Mem0 Mem0 { get; set; }
    }
}
