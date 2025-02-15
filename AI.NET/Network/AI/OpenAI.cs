using MdXaml;
using OpenAI;
using OpenAI.Chat;
using System.ClientModel;
using System.ComponentModel;

namespace AI.NET.Network.AI
{
    /// <summary>
    /// OpenAI API network. Can be used as data context for the UI
    /// </summary>
    internal class OpenAI : INotifyPropertyChanged
    {
        /// <summary>
        /// Event handler for property changed
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;
        /// <summary>
        /// Notify property changed
        /// </summary>
        /// <param name="propertyName"></param>
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        /// <summary>
        /// Custom AI endpoint for environment variable, default is OpenAI's endpoint
        /// </summary>
        private Uri? _endPoint;
        // Model info
        private ApiKeyCredential? _aiCredential;
        private string? _modelName;
        public string AiCredential
        {
            get
            {
                if (_aiCredential is null) return "";
                string key;
                _aiCredential.Deconstruct(out key);
                return key;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    if (_aiCredential is not null)
                        _aiCredential.Update(value);
                    else
                        _aiCredential = new(value);
                OnPropertyChanged(nameof(AiCredential));
                // No need to update if the input key is empty
            }
        }
        public string EndPoint
        {
            get
            {
                if (_endPoint is null) return "https://api.openai.com/v1";
                return _endPoint.AbsoluteUri;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    _endPoint = new(value);
                OnPropertyChanged(nameof(AiCredential));
                // No need to update if the input endpoint is empty
            }
        }
        public string ModelName
        {
            get => _modelName ?? "gpt-4o";
            set { _modelName = value; OnPropertyChanged(nameof(ModelName)); }
        }
        /// <summary>
        /// Generate AI reply, store the message in the global message storage
        /// </summary>
        /// <returns></returns>
        public async Task<string> GenerateReplyAsync(MarkdownScrollViewer outputBox)
        {
            OpenAIClientOptions options = new()
            {
                Endpoint = _endPoint
            };
            ChatClient client = new(ModelName, _aiCredential, options);
            string message = "";
            await foreach (StreamingChatCompletionUpdate update in client.CompleteChatStreamingAsync(Service.AI.messages))
            {
                if (update.ContentUpdate.Count > 0)
                {
                    outputBox.Markdown += update.ContentUpdate[0].Text;
                    message += update.ContentUpdate[0].Text;
                }
            }
            return message;
        }
    }
}
