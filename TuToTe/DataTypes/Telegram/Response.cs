using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace TuToTe.DataTypes.Telegram
{
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    class Response
    {
        /// <summary>
        /// Simple status showing if response got no errors
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public bool Ok { get; set; }

        /// <summary>
        /// An array of updates
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Update[] Result { get; set; }
    }
}
