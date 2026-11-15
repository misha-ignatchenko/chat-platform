using System;
using System.Collections.Generic;
using System.Text;

namespace ChatPlatform.Client.Options
{
    public class ServerOptions
    {
        public string BaseUrl { get; set; } = "";
        public string HubPath { get; set; } = "/chat";
    }
}
