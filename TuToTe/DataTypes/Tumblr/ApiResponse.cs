using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace TuToTe.DataTypes.Tumblr
{
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    class ApiResponse
    {

        [JsonProperty]
        public Meta Meta { get; set; }


        [JsonProperty]
        public Response Response { get; set; }
    }
}
