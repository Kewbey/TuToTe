using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace TuToTe.DataTypes.Tumblr
{
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    class Error
    {

        [JsonProperty]
        public string Title { get; set; }


        [JsonProperty]
        public int Code { get; set; }


        [JsonProperty]
        public string Detail { get; set; }
    }
}
