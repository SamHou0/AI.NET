using AI.NET.Data;
using AI.NET.File;
using HandyControl.Controls;
using MdXaml;
using OpenAI.Chat;

namespace AI.NET.Service
{
    /// <summary>
    /// Place to store AI instances
    /// </summary>
    internal static class AI
    {
        private static readonly string markdownNewLine =
            Environment.NewLine + Environment.NewLine;
        public static Network.AI.OpenAI OpenAI { get; set; } = new();
        public static Network.AI.Mem0 Mem0 { get; set; } = new();
        /// <summary>
        /// The global message storage
        /// </summary>
        public static Topics Topics { get; set; } = new();
        /// <summary>
        /// Request AI to generate a reply
        /// </summary>
        /// <param name="message">The user's message</param>
        /// <param name="outputBox">The UI element to update</param>
        public static async Task RequestAIAsync(string message, MarkdownScrollViewer outputBox)
        {
            Topics.CurrentTopic.Add(new Chat() { Content = message, Role = ChatMessageRole.User });
            outputBox.Markdown += "User:" + message + markdownNewLine;
            outputBox.Markdown += "AI:";
            if (Mem0.IsEnabled)
                await HandleMemoryAsync(message);
            Topics.CurrentTopic.Add(new Chat()
            {
                Content = await OpenAI.GenerateReplyAsync(outputBox),
                Role = ChatMessageRole.Assistant
            });
            //Save topics to file. No need to wait.
            TopicsHelper.SaveTopicsAsync(Topics);
        }
        /// <summary>
        /// Delete a chat session
        /// </summary>
        public static void DeleteMessages()
        {
            Topics.RemoveAt(Topics.CurrentTopicIndex);
            TopicsHelper.SaveTopicsAsync(Topics);
        }
        public static void NewChat(Data.SystemPrompt prompt)
        {
            Topics.Add(new(prompt));
        }
        public static List<ChatMessage> GetChatMessages()
        {
            return Topics.CurrentTopic.ToChatMessageList();
        }

        private static async Task HandleMemoryAsync(string message)
        {
            // First search in memories
            string memoryString = await Mem0.SearchMemoryAsync(message, "default_user");
            // Add Memory, no need to wait
            Mem0.AddMemoryAsync(message, "default_user");
            Topics.CurrentTopic.Add(new Chat()
            {
                Role = ChatMessageRole.System,
                Content = "Memories about user: " + memoryString
            });
        }
        public static void LoadTopics()
        {
            Topics = TopicsHelper.GetTopics();
        }
    }
}
