﻿using Newtonsoft.Json;

namespace Unicorn.ReportPortalAgent.Configuration
{
    public class Config
    {
        [JsonProperty("enabled")]
        public bool IsEnabled { get; set; }

        public Server Server { get; set; }

        public Launch Launch { get; set; }
    }
}
