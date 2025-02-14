using OpenAI.Chat;
using OpenAI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ClientModel;
using MdXaml;
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
        private readonly Uri endPoint =
    new Uri(Environment.GetEnvironmentVariable("OPENAI_BASE_URL") ??
        "https://api.openai.com");
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
        public string ModelName
        {
            get => _modelName ?? "gpt-4o";
            set { _modelName = value; OnPropertyChanged(nameof(ModelName)); }
        }
        /// <summary>
        /// Generate AI reply, store the message in the global message storage
        /// </summary>
        /// <returns></returns>
        public async Task GenerateReplyAsync(MarkdownScrollViewer outputBox)
        {
            OpenAIClientOptions options = new()
            {
                Endpoint = endPoint
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
            Service.AI.messages.Add(new AssistantChatMessage(message));
        }
    }
}
