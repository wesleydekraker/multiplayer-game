using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.Rendering
{
    public class Text
    {
        public string Value { get; set; }

        private SpriteFont font;
        private Vector2 position;
        private Color color;        

        public Text(string value, SpriteFont font, Vector2 position, Color color)
        {
            this.Value = value;

            this.font = font;
            this.position = position;
            this.color = color;            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, this.Value, this.position, this.color);
        }
    }
}