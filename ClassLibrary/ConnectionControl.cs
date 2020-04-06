using System;
using System.Collections.Generic;
using System.Text;

namespace ClassLibrary
{
    [Serializable]
    public class ConnectionControl : BaseMessage
    {
        //public ConnectionStatus Status { get; set; } = ConnectionStatus.Control;
        
        public ConnectionControl(string user = "")
        {
            UserName = user;
        }
        
    }
}
