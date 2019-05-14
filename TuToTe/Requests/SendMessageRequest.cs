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



        // TODO: add other types

        public SendMessageRequest(long chatId, string text)
        {
            ChatId = chatId;
            Text = text;
        }
    }
}
