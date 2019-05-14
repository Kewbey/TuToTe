using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace TuToTe.DataTypes.Telegram
{
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    class Message
    {
        /// <summary>
        /// Unique message identifier inside this chat
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public int MessageId { get; set; }

        /// <summary>
        /// Optional. Sender, empty for messages sent to channels
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public User From { get; set; }

        /// <summary>
        /// Date the message was sent in Unix time
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public int Date { get; set; }

        /// <summary>
        /// Conversation the message belongs to
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public Chat Chat { get; set; }

        /// <summary>
        /// Optional. For text messages, the actual UTF-8 text of the message, 0-4096 characters.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Text { get; set; }

        // TODO: Add other types
    }
}
