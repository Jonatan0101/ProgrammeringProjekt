using System;
using System.Collections.Generic;
using System.Text;

namespace ClassLibrary
{
    [Serializable]
    public class ChatMessage : BaseMessage
    {
        public ChatMessage(string message, string user = "")
        {
            TextMessage = message;
            if (user != "")
                UserName = user;
        }
        public string TextMessage { get; set; }
    }
}
