using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace TuToTe.DataTypes.Tumblr
{
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    class Post
    {

        [JsonProperty]
        public string BlogName { get; set; }


        [JsonProperty]
        public BlogInPost Blog { get; set; }


        [JsonProperty]
        public long Id { get; set; }


        [JsonProperty]
        public string PostUrl { get; set; }


        [JsonProperty]
        public string Type { get; set; }


        [JsonProperty]
        public long Timestamp { get; set; }


        [JsonProperty]
        public string Date { get; set; }


        [JsonProperty]
        public string ShortUrl { get; set; }
    }
}
