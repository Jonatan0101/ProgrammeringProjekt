using System;
using System.Collections.Generic;
using System.Text;

namespace ClassLibrary
{
    [Serializable]
    public class MeObj
    {
        public MeObj(string message, string user = "")
        {
            TextMessage = message;
            if (user != "")
                UserName = user;
        }
        public string TextMessage { get; set; }
        public string UserName { get; set; }
    }
}
