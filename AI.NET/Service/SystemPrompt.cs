using AI.NET.Data;
using AI.NET.File;

namespace AI.NET.Service
{
    internal static class SystemPrompt
    {
        /// <summary>
        /// The global prompt storage
        /// </summary>
        public static SystemPrompts systemPrompts = new();
        public static void AddPrompt(Data.SystemPrompt prompt)
        {
            systemPrompts.AddPrompt(prompt);
            Save();
        }
        public static void RemovePrompt(Data.SystemPrompt prompt)
        {
            systemPrompts.RemovePrompt(prompt);
            Save();
        }

        public static void Save()
        {
            SystemPromptHelper.SavePrompt(systemPrompts);
        }

        public static void LoadPrompts()
        {
            systemPrompts = SystemPromptHelper.LoadPrompt();
        }
    }
}
