using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace TuToTe.Requests
{
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    class SendMessageRequest
    {
        /// <summary>
        /// Unique identifier for the target chat or username of the target channel (in the format @channelusername)
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public long ChatId { get; set; }

        /// <summary>
        /// Text of the message to be sent
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string Text { get; set; }

        /// <summary>
        /// Send Markdown or HTML, if you want Telegram apps to show bold, italic, fixed-width text or inline URLs in your bot's message.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string ParseMode { get; set; }

        /// <summary>
        /// Disables link previews for links in this message
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool DisableWebPagePreview { get; set; }

        /// <summary>
        /// Sends the message silently. Users will receive a notification with no sound.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool DisableNotification { get; set; }

        /// <summary>
        /// If the message is a reply, ID of the original message
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int ReplyToMessageId { get; set; }

        // The list of types is not completed

        public SendMessageRequest(long chatId, string text)
        {
            ChatId = chatId;
            Text = text;
        }
    }
}
