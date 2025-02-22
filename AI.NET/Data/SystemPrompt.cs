using OpenAI.Chat;
using System.ComponentModel;

namespace AI.NET.Data
{
    /// <summary>
    /// The system prompt model
    /// </summary>
    internal class SystemPrompt : INotifyPropertyChanged
    {
        private string _name = "Default message";
        private string _content = "Answer user's request based on memories and chat messages.";
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public string Content
        {
            get => _content;
            set
            {
                _content = value;
                OnPropertyChanged(nameof(Content));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public SystemChatMessage ToChatMessage()
        {
            return new SystemChatMessage(Content);
        }
        public Chat ToChat()
        {
            return new Chat() { Content = Content, Role = ChatMessageRole.System };
        }
    }
}
