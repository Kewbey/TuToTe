using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace TuToTe.DataTypes.Tumblr
{
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    class Meta
    {

        [JsonProperty]
        public string Msg { get; set; }


        [JsonProperty]
        public int Status { get; set; }


        [JsonProperty]
        public string XTumblrContentRating { get; set; }
    }
}
