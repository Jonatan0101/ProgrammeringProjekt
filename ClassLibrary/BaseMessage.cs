using System;
using System.Collections.Generic;
using System.Text;

namespace ClassLibrary
{
    [Serializable]
    public abstract class BaseMessage
    {
        public string UserName { get; set; }
    }
}
