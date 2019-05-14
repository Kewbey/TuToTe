using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace TuToTe.DataTypes.Telegram
{
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    class User
    {
        /// <summary>
        /// Unique identifier for this user or bot
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string Id { get; set; }

        /// <summary>
        /// True, if this user is a bot
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public bool IsBot { get; set; }

        /// <summary>
        /// User‘s or bot’s first name
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string FirstName { get; set; }

        /// <summary>
        /// Optional. User‘s or bot’s last name
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string LastName { get; set; }

        /// <summary>
        /// Optional. User‘s or bot’s username
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Username { get; set; }

        /// <summary>
        /// Optional. IETF language tag of the user's language
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string LanguageCode { get; set; }
    }
}
