using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace TuToTe.DataTypes.Telegram
{
    /// <summary>
    /// This object contains information about one answer option in a poll.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    class PollOption
    {
        /// <summary>
        /// Option text, 1-100 characters
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string Text { get; set; }

        /// <summary>
        /// Number of users that voted for this option
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public int VoterCount { get; set; }
    }
}
