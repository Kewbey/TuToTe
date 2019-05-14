using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace TuToTe.DataTypes.Telegram
{
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    class Audio
    {
        /// <summary>
        /// Unique identifier for this file
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string FileId { get; set; }

        /// <summary>
        /// Duration of the audio in seconds as defined by sender
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public int Duration { get; set; }

        /// <summary>
        /// Optional. Performer of the audio as defined by sender or by audio tags
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Performer { get; set; }

        /// <summary>
        /// Optional. Title of the audio as defined by sender or by audio tags
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Title { get; set; }

        /// <summary>
        /// Optional. MIME type of the file as defined by sender
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string MimeType { get; set; }

        /// <summary>
        /// Optional. File size
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int FileSize { get; set; }

        /// <summary>
        /// Optional. Thumbnail of the album cover to which the music file belongs
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public PhotoSize Thumb { get; set; }
    }
}
