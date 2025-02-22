using MdXaml;
using OpenAI.Chat;
using System.ComponentModel;

namespace AI.NET.Data
{
    internal class Messages : INotifyPropertyChanged
    {
        private readonly string markdownNewLine =
            Environment.NewLine + Environment.NewLine;
        private List<Chat> _messageList = new();
        private SystemPrompt _systemPrompt;
        private DateTime _latestChat;
        public List<Chat> MessageList
        {
            get => _messageList; set
            {
                _messageList = value;
                OnPropertyChanged(nameof(MessageList));
            }
        }
        /// <summary>
        /// SystemPrompt, as well as its corresponding message
        /// </summary>
        public SystemPrompt SystemPrompt
        {
            get => _systemPrompt;
            set
            {
                MessageList[0] = new()
                {
                    Role = ChatMessageRole.System,
                    Content = value.Content
                };
                _systemPrompt = value;
                OnPropertyChanged(nameof(SystemPrompt));
                OnPropertyChanged(nameof(Description));
            }
        }
        /// <summary>
        /// Get full markdown of the chat
        /// </summary>
        public async Task<string> GetMarkdownAsync()
        {
            string markdown = "";
            await Task.Run(() =>
             {
                 foreach (Chat message in MessageList)
                 {
                     switch (message.Role)
                     {
                         case ChatMessageRole.User:
                             markdown += "User: ";
                             markdown += message.Content + markdownNewLine;
                             break;
                         case ChatMessageRole.Assistant:
                             markdown += "AI: ";
                             markdown += message.Content + markdownNewLine;
                             break;
                     }
                 }
             });
            return markdown;
        }
        public string Description
        {
            get
            {
                return LatestChat + " " + SystemPrompt.Name;
            }
        }
        /// <summary>
        /// The time the chat was updated
        /// </summary>
        public DateTime LatestChat
        {
            get => _latestChat;
            set
            {
                _latestChat = value;
                OnPropertyChanged(nameof(LatestChat));
                OnPropertyChanged(nameof(Description));
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        /// <summary>
        /// Constructor with a default system prompt, can be used for 
        /// IO purpose (load system prompts)
        /// </summary>
        public Messages()
        {
            SystemPrompt message = new();
            MessageList = new List<Chat>
            {
                message.ToChat()
            };
            _systemPrompt = message;
        }
        /// <summary>
        /// Constructor with a system prompt
        /// </summary>
        /// <param name="systemPrompt"></param>
        public Messages(SystemPrompt systemPrompt)
        {
            MessageList = new List<Chat>
            {systemPrompt.ToChat()  };
            _systemPrompt = systemPrompt;
        }
        public void Add(Chat message)
        {
            MessageList.Add(message);
            LatestChat = DateTime.Now;
        }
        public void Reset()
        {
            MessageList.Clear();
            MessageList.Add(_systemPrompt.ToChat());
        }
        /// <summary>
        /// Convert the messages to a list of chats, for IO purpose
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidCastException"></exception>
        public List<ChatMessage> ToChatMessageList()
        {
            List<ChatMessage> chats = new();
            foreach (Chat message in MessageList)
            {
                ChatMessage chatMessage = message.Role switch
                {
                    ChatMessageRole.User => new UserChatMessage(message.Content),
                    ChatMessageRole.Assistant => new AssistantChatMessage(message.Content),
                    ChatMessageRole.System => new SystemChatMessage(message.Content),
                    _ => throw new InvalidCastException("Invalid ChatMessageRole")
                };
                chats.Add(chatMessage);
            }
            return chats;
        }
    }
}
