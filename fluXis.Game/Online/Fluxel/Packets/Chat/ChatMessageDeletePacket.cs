using Newtonsoft.Json;

namespace fluXis.Game.Online.Fluxel.Packets.Chat;

public class ChatMessageDeletePacket : Packet
{
    public override string ID => "chat/delete";

    [JsonProperty("id")]
    public string Id { get; init; }

    public ChatMessageDeletePacket(string id)
    {
        Id = id;
    }
}
