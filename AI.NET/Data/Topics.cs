using System.Collections.ObjectModel;
using System.ComponentModel;

namespace AI.NET.Data
{
    /// <summary>
    /// Representing a list of messages. Each messages object is defined as a topic
    /// Can be used as data context for the UI
    /// </summary>
    internal class Topics : INotifyPropertyChanged
    {
        // Must be observable to display UI
        private ObservableCollection<Messages> _topicList = new();
        public ObservableCollection<Messages> TopicList
        {
            get => _topicList;
            private set
            {
                _topicList = value;
                OnPropertyChanged(nameof(TopicList));
            }
        }
        private int _currentTopicIndex = 0;
        public int CurrentTopicIndex
        {
            get
            {
                if (_currentTopicIndex >= 0 && _currentTopicIndex < TopicList.Count)
                {
                    return _currentTopicIndex;
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                _currentTopicIndex = value;
                OnPropertyChanged(nameof(CurrentTopicIndex));
                OnPropertyChanged(nameof(CurrentTopic));
            }
        }
        public Messages CurrentTopic
        {
            get => TopicList[CurrentTopicIndex];
        }
        public Topics()
        {
            TopicList = new() {};
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public void Add(Messages message)
        {
            TopicList.Add(message);
            OnPropertyChanged(nameof(TopicList));
        }
        public void RemoveAt(int index)
        {
            TopicList.RemoveAt(index);
            OnPropertyChanged(nameof(TopicList));
        }
        public void Clear()
        {
            TopicList.Clear();
        }
    }
}
