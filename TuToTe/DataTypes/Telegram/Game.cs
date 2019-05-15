using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace TuToTe.DataTypes.Telegram
{
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    class Game
    {
        /// <summary>
        /// Title of the game
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string Title { get; set; }

        /// <summary>
        /// Description of the game
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string Description { get; set; }

        /// <summary>
        /// Photo that will be displayed in the game message in chats.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public PhotoSize[] Photo { get; set; }

        /// <summary>
        /// Optional. Brief description of the game or high scores included in the game message. 
        /// Can be automatically edited to include current high scores for the game when the bot calls setGameScore, or manually edited using editMessageText. 0-4096 characters.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Text { get; set; }

        /// <summary>
        /// Optional. Special entities that appear in text, such as usernames, URLs, bot commands, etc.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public MessageEntity[] TextEntities { get; set; }

        /// <summary>
        /// Optional. Animation that will be displayed in the game message in chats. Upload via BotFather
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Animation Animation { get; set; }
    }
}
