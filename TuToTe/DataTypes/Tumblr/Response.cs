using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace TuToTe.DataTypes.Tumblr
{
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    class Response
    {

        [JsonProperty]
        public Blog Blog { get; set; }


        [JsonProperty]
        public Post[] Posts { get; set; }


        [JsonProperty]
        public Error[] Errors { get; set; }
    }
}
