using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace TuToTe.DataTypes.Telegram
{
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    class Sticker
    {
        /// <summary>
        /// Unique identifier for this file
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string FileId { get; set; }

        /// <summary>
        /// Sticker width
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public int Width { get; set; }

        /// <summary>
        /// Sticker height
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public int Height { get; set; }

        /// <summary>
        /// Optional. Sticker thumbnail in the .webp or .jpg format
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public PhotoSize Thumb { get; set; }

        /// <summary>
        /// Optional. Emoji associated with the sticker
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Emoji { get; set; }

        /// <summary>
        /// Optional. Name of the sticker set to which the sticker belongs
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string SetName { get; set; }

        /// <summary>
        /// Optional. For mask stickers, the position where the mask should be placed
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public MaskPosition MaskPosition { get; set; }

        /// <summary>
        /// Optional. File size
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int FileSize { get; set; }
    }
}
