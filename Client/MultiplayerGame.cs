using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Client.Rendering;
using Shared;
using System;
using System.Collections.Generic;

namespace Client
{
    public class MultiplayerGame : Game
    {
        private const int ScreenWidth = 704;
        private const int ScreenHeight = 576;
        private readonly string userId = Guid.NewGuid().ToString();
        private readonly GraphicsDeviceManager graphicsDeviceManager;
        private readonly UdpClient udpClient = new UdpClient();

        private SpriteBatch spriteBatch;
        private Text text;
        private Sprite map;
        private Dictionary<string, Sprite> players = new Dictionary<string, Sprite>();

        private SpriteFont font;
        private Texture2D playerTexture;
        private Texture2D mapTexture;

        public MultiplayerGame()
        {
            this.graphicsDeviceManager = new GraphicsDeviceManager(this);
            this.Content.RootDirectory = "Content/Assets";
            this.IsMouseVisible = true;
        }

        protected override void LoadContent()
        {
            this.spriteBatch = new SpriteBatch(this.GraphicsDevice);

            this.font = this.Content.Load<SpriteFont>(assetName: "font");
            this.playerTexture = this.Content.Load<Texture2D>(assetName: "player");
            this.mapTexture = this.Content.Load<Texture2D>(assetName: "map");
        }

        protected override void Initialize()
        {
            base.Initialize();

            this.graphicsDeviceManager.PreferredBackBufferWidth = ScreenWidth;
            this.graphicsDeviceManager.PreferredBackBufferHeight = ScreenHeight;
            this.graphicsDeviceManager.ApplyChanges();
            
            this.text = new Text("", this.font, new Vector2(480, 515), Color.Black);
            this.map = new Sprite(this.mapTexture, 0, 0, ScreenWidth, ScreenHeight);
        }

        protected override void Update(GameTime gameTime)
        {
            var currentKeyState = Keyboard.GetState();

            var up = currentKeyState.IsKeyDown(Keys.W);
            var down = currentKeyState.IsKeyDown(Keys.S);
            var left = currentKeyState.IsKeyDown(Keys.A);
            var right = currentKeyState.IsKeyDown(Keys.D);

            udpClient.Send(new UserInput(userId, up, down, left, right));

            var players = udpClient.GetReceivedMessage<List<Player>>();

            this.text.Value = players == null ? "No connection" : "Multiplayer RPG Game";

            if (players == null)
                return;

            foreach (var player in players)
            {
                this.players[player.UserId] = new Sprite(playerTexture, (int) player.X, (int) player.Y, 24, 34);
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            this.spriteBatch.Begin();

            this.map.Draw(spriteBatch);
            this.text.Draw(this.spriteBatch);

            foreach (var player in this.players.Values)
            {
                player.Draw(this.spriteBatch);
            }

            this.spriteBatch.End();
        }
    }
}
