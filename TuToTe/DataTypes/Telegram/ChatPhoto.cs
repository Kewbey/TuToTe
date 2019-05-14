using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace TuToTe.DataTypes.Telegram
{
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    class ChatPhoto
    {
        /// <summary>
        /// Unique file identifier of small (160x160) chat photo. This file_id can be used only for photo download.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string SmallFileId { get; set; }

        /// <summary>
        /// Unique file identifier of big (640x640) chat photo. This file_id can be used only for photo download.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string BigFileId { get; set; }
    }
}
