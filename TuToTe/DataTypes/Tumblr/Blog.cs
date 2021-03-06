﻿using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace TuToTe.DataTypes.Tumblr
{
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    class Blog
    {

        [JsonProperty]
        public string Title { get; set; }


        [JsonProperty]
        public int Posts { get; set; }


        [JsonProperty]
        public string Name { get; set; }


        [JsonProperty]
        public long Updated { get; set; }


        [JsonProperty]
        public string Description { get; set; }


        [JsonProperty]
        public string Uuid { get; set; }


        [JsonProperty]
        public string Url { get; set; }


        [JsonProperty]
        public bool IsNsfw { get; set; }
    }
}
