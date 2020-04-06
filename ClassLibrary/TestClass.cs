using System;
using System.Collections.Generic;
using System.Text;

namespace ClassLibrary
{
    [Serializable]
    public class TestClass : BaseMessage
    {
        public ConnectionStatus Status { get; set; } = new ConnectionStatus();

        public TestClass(ConnectionStatus status, string user)
        {
            Status = status;
            UserName = user;
        }
    }
}
