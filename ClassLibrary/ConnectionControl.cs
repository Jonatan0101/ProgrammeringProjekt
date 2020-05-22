using System;
using System.Collections.Generic;
using System.Text;

namespace ClassLibrary
{
    [Serializable]
    public class ConnectionControl : BaseMessage
    {
        public ConnectionStatus Status { get; set; }
        public List<string> ListOfUsers { get; set; } = new List<string>();
        public ConnectionControl(string user = "", ConnectionStatus status = ConnectionStatus.Control)
        {
            UserName = user;
            Status = status;
        }
        
    }
}
