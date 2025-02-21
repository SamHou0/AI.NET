using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenAI.Chat;

namespace AI.NET.Data
{
    class Messages : INotifyPropertyChanged
    {
        private SystemChatMessage _systemPrompt;
        public List<ChatMessage> MessageList { get; private set; }
        public string SystemPrompt
        {
            get => _systemPrompt.Content[0].Text ?? "";
            set
            {
                SystemChatMessage message = new(value);
                MessageList[0] = message;
                _systemPrompt = message;
                OnPropertyChanged(nameof(SystemPrompt));
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public Messages()
        {
            SystemChatMessage message = new("Answer user's request based on memories and chat messages.");
            MessageList = new List<ChatMessage>
            { message };
            _systemPrompt = message;
        }
        public void Add(ChatMessage message)
        {
            MessageList.Add(message);
        }
        public void Reset()
        {
            MessageList.Clear();
            MessageList.Add(_systemPrompt);
        }
    }
}
