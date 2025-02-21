using AI.NET.Data;
using MdXaml;
using OpenAI.Chat;

namespace AI.NET.Service
{
    /// <summary>
    /// Place to store AI instances
    /// </summary>
    internal static class AI
    {
        public static Network.AI.OpenAI OpenAI { get; set; } = new();
        public static Network.AI.Mem0 Mem0 { get; set; } = new();
        /// <summary>
        /// The global message storage
        /// </summary>
        public static Messages messages = new();
        /// <summary>
        /// Request AI to generate a reply
        /// </summary>
        /// <param name="message">The user's message</param>
        /// <param name="outputBox">The UI element to update</param>
        public static async Task RequestAIAsync(string message, MarkdownScrollViewer outputBox)
        {
            messages.Add(new UserChatMessage(message));
            if (Mem0.IsEnabled)
                await HandleMemoryAsync(message);
            messages.Add(new AssistantChatMessage(
                await OpenAI.GenerateReplyAsync(outputBox)));
        }
        /// <summary>
        /// Create a new chat session
        /// </summary>
        public static void ResetMessages()
        {
            messages.Reset();
        }
        public static List<ChatMessage> GetChatMessages()
        {
            return messages.MessageList;
        }

        private static async Task HandleMemoryAsync(string message)
        {
            // First search in memories
            string memoryString = await Mem0.SearchMemoryAsync(message, "default_user");
            // Add Memory, no need to wait
            Mem0.AddMemoryAsync(message, "default_user");
            messages.Add(new SystemChatMessage("Memories about user: " + memoryString));
        }

    }
}
