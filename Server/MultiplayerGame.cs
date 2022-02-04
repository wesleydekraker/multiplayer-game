using Microsoft.Xna.Framework;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server
{
    public class MultiplayerGame
    {
        private const float PlayerSpeed = 0.1f;

        private UdpServer udpServer = new UdpServer();
        private Dictionary<string, Vector2> players = new Dictionary<string, Vector2>();

        public void Update(TimeSpan delta)
        {
            var userInputCollection = this.udpServer.GetReceivedMessages<UserInput>();

            foreach (var userInput in userInputCollection)
            {
                if (!this.players.ContainsKey(userInput.UserId))
                    this.players[userInput.UserId] = new Vector2(0, 0);

                var distance = (int) (PlayerSpeed * delta.TotalMilliseconds);

                if (userInput.Left)
                    this.players[userInput.UserId] += new Vector2(-distance, 0);
                if (userInput.Right)
                    this.players[userInput.UserId] += new Vector2(distance, 0);
                if (userInput.Up)
                    this.players[userInput.UserId] += new Vector2(0, -distance);
                if (userInput.Down)
                    this.players[userInput.UserId] += new Vector2(0, distance);

                var players = this.players
                    .Select(player => new Player(player.Key, player.Value.X, player.Value.Y))
                    .ToList();

                udpServer.Send(players);
            }
        }
    }
}
