using AI.NET.Data;
using AI.NET.Logger;
using HandyControl.Controls;
using System.IO;
using System.Text.Json;

namespace AI.NET.File
{
    internal static class SystemPromptHelper
    {
        private static readonly string path = AppDomain.CurrentDomain.BaseDirectory
            + "SystemPrompts.json";
        public static void SavePrompt(SystemPrompts prompts)
        {
            // Save the prompt to the file system
            try
            {
                using (StreamWriter sw = new(path))
                {
                    sw.Write(JsonSerializer.Serialize(prompts));
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error saving prompt", ex);
                Growl.Error(new() { StaysOpen = true, Message = "Error saving prompt" });
            }
        }
        public static SystemPrompts LoadPrompt()
        {
            // Load the prompt from the file system
            try
            {
                if (!System.IO.File.Exists(path))
                    return new();
                using (StreamReader sr = new(path))
                {
                    return JsonSerializer.Deserialize
                        <SystemPrompts>(sr.ReadToEnd()) ??
                        throw new InvalidDataException("Error reading prompts");
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error loading prompt", ex);
                Growl.Error(new() { StaysOpen = true, Message = "Error loading prompt" });
            }
            return new();
        }
    }
}
