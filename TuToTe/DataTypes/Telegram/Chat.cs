using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace TuToTe.DataTypes.Telegram
{
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    class Chat
    {
        /// <summary>
        /// Unique identifier for this chat
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public long Id { get; set; }

        /// <summary>
        /// Type of chat, can be either “private”, “group”, “supergroup” or “channel”
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string Type { get; set; }

        /// <summary>
        /// Optional. Title, for supergroups, channels and group chats
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Title { get; set; }

        /// <summary>
        /// Optional. Username, for private chats, supergroups and channels if available
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Username { get; set; }

        /// <summary>
        /// Optional. First name of the other party in a private chat
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string FirstName { get; set; }

        /// <summary>
        /// Optional. Last name of the other party in a private chat
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string LastName { get; set; }

        /// <summary>
        /// Optional. True if a group has ‘All Members Are Admins’ enabled.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool AllMembersAreAdministrators { get; set; }

        /// <summary>
        /// Optional. Chat photo. Returned only in getChat.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ChatPhoto Photo { get; set; }

        /// <summary>
        /// Optional. Description, for supergroups and channel chats. Returned only in getChat.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Description { get; set; }

        /// <summary>
        /// Optional. Chat invite link, for supergroups and channel chats. 
        /// Each administrator in a chat generates their own invite links, so the bot must first generate the link using exportChatInviteLink. 
        /// Returned only in getChat.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string InviteLink { get; set; }

        /// <summary>
        /// Optional. Pinned message, for groups, supergroups and channels. Returned only in getChat.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Message PinnedMessage { get; set; }

        /// <summary>
        /// Optional. For supergroups, name of group sticker set. Returned only in getChat.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string StickerSetName { get; set; }

        /// <summary>
        /// Optional. True, if the bot can change the group sticker set. Returned only in getChat.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool CanSetStickerSet { get; set; }
    }
}
