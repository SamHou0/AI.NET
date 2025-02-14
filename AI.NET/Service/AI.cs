using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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
        public static List<ChatMessage> messages = new() {new SystemChatMessage
                ("Answer user's request based on memories and chat messages.") };
        /// <summary>
        /// Request AI to generate a reply
        /// </summary>
        /// <param name="message">The user's message</param>
        /// <param name="outputBox">The UI element to update</param>
        public static async Task RequestAIAsync(string message, MarkdownScrollViewer outputBox)
        {
            await HandleMemoryAsync(message);
            messages.Add(new UserChatMessage(message));
            await OpenAI.GenerateReplyAsync(outputBox);
        }

        private static async Task HandleMemoryAsync(string message)
        {
            // First search in memories
            messages.Add(new UserChatMessage(message));
            string memoryString = await Mem0.SearchMemoryAsync(message, "default_user");
            // Add Memory, no need to wait
            Mem0.AddMemoryAsync(message, "default_user");
            messages.Add(new SystemChatMessage("Memories about user: " + memoryString));
        }
        public static void ResetMessages()
        {
            messages = new List<ChatMessage>
            { new SystemChatMessage("Answer user's request based on memories and chat messages.") };
        }
    }
}
