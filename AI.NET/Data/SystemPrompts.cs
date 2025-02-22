using System.Collections.ObjectModel;
using System.ComponentModel;

namespace AI.NET.Data
{
    internal class SystemPrompts : INotifyPropertyChanged
    {
        private ObservableCollection<SystemPrompt> _prompts;
        public ObservableCollection<SystemPrompt> Prompts
        {
            get => _prompts;
            set
            {
                _prompts = value;
                OnPropertyChanged(nameof(Prompts));
            }
        }
        public SystemPrompts()
        {
            _prompts = new();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public void AddPrompt(SystemPrompt prompt)
        {
            if (!Prompts.Contains(prompt))
            {
                Prompts.Add(prompt);
            }
        }
        public void RemovePrompt(SystemPrompt prompt)
        {
            Prompts.Remove(prompt);
        }
        public int FindPromptIndex(SystemPrompt prompt)
        {
            return Prompts.IndexOf(prompt);
        }
    }
}
