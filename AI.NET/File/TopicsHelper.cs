using AI.NET.Data;
using AI.NET.Logger;
using HandyControl.Controls;
using System.Globalization;
using System.IO;
using System.Text.Json;

namespace AI.NET.File
{
    internal static class TopicsHelper
    {
        private static readonly string path = AppDomain.CurrentDomain.BaseDirectory + "messages";
        /// <summary>
        /// Save messages to file
        /// </summary>
        /// <param name="messages">The messages object containing chats</param>
        public static async void SaveTopicsAsync(Topics topics)
        {
            CreateDir();
            await Task.Run(() =>
            {
                ClearTopics();
                try
                {
                    foreach (Messages messages in topics.TopicList)
                    {
                        string messagesPath =
                            path + "\\" + messages.LatestChat.ToString("yyyy-MM-d-HH-mm-ss", CultureInfo.InvariantCulture) + ".json";
                        if (!System.IO.File.Exists(messagesPath))
                            using (StreamWriter sw = new(messagesPath))
                            {
                                sw.Write(JsonSerializer.Serialize(messages));
                            }
                    }
                }
                catch (Exception ex)
                {
                    Log.Error("Error saving topics", ex);
                    Growl.Error(new() { StaysOpen = true, Message = "Error saving messages" });
                }
            });
        }
        private static void ClearTopics()
        {
            foreach (string file in Directory.GetFiles(path))
            {
                try
                {
                    System.IO.File.Delete(file);
                }
                catch (Exception ex)
                {
                    Log.Error("Error deleting messages", ex);
                    Growl.Error(new() { StaysOpen = true, Message = "Error deleting messages" });
                }
            }
        }
        private static void CreateDir()
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
        /// <summary>
        /// Get all saved topics
        /// </summary>
        /// <returns></returns>
        public static Topics GetTopics()
        {
            Topics topics = new();
            if (!Directory.Exists(path))
            {
                return topics;
            }
            foreach (string file in Directory.GetFiles(path))
            {
                try
                {
                    using (StreamReader sr = new(file))
                    {
                        topics.Add(
                                (JsonSerializer.Deserialize<Messages>(sr.ReadToEnd())
                            ?? throw new InvalidDataException("Invalid message list json")));
                    }
                }
                catch (Exception ex)
                {
                    Log.Error("Error reading messages", ex);
                    Growl.Error(new() { StaysOpen = true, Message = "Error reading messages" });
                }
            }
            return topics;
        }
    }
}
