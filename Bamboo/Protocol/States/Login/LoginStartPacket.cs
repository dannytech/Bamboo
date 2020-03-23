using Bamboo.Game;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;

namespace Bamboo.Protocol.States.Login
{
    class LoginStartPacket : ServerboundPacket
    {
        public override int PacketID { get => 0x00; }

        public LoginStartPacket(Client client) : base(client) { }

        public override async void Parse(IReadable buffer)
        {
            string username = buffer.Reader.ReadVarChar();

            // Query Mojang's API to get the player UUID
            HttpResponseMessage response = await Helpers.HttpClient.GetAsync($"https://api.mojang.com/users/profiles/minecraft/{username}");
            string body = await response.Content.ReadAsStringAsync();

            // Parse the response JSON
            Dictionary<string, string> reply = JsonSerializer.Deserialize<Dictionary<string, string>>(body);
            Guid uuid = new Guid(reply["id"]);

            _Client.Player = new Player(username, uuid);

            if (Settings.Configuration["online"] == "true")
            {
                _Client.ClientboundPackets.Add(new EncryptionRequestPacket(_Client));
            }
            else
            {
                _Client.ClientboundPackets.Add(new SetCompressionPacket(_Client));
                _Client.ClientboundPackets.Add(new LoginSuccessPacket(_Client));
            }
        }
    }
}
