using OpenAI.Chat;

namespace AI.NET.Data
{
    /// <summary>
    /// Chat message for IO
    /// </summary>
    internal class Chat
    {
        public required ChatMessageRole Role { get; set; }
        public required string Content { get; set; }
        public ChatMessage ToChatMessage()
        {
            switch (Role)
            {
                case ChatMessageRole.System:
                    return new SystemChatMessage(Content);
                case ChatMessageRole.User:
                    return new UserChatMessage(Content);
                case ChatMessageRole.Assistant:
                    return new AssistantChatMessage(Content);
                case ChatMessageRole.Tool:
                    return new ToolChatMessage(Content);
                default:
                    throw new InvalidCastException("Invalid ChatMessageRole");
            }
        }
    }
}
