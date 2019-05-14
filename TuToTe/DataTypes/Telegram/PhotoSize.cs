using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace TuToTe.DataTypes.Telegram
{
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    class PhotoSize
    {
        /// <summary>
        /// Unique identifier for this file
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string FileId { get; set; }

        /// <summary>
        /// Photo width
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public int Width { get; set; }

        /// <summary>
        /// Photo height
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public int Height { get; set; }

        /// <summary>
        /// Optional. File size
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int FileSize { get; set; }
    }
}
