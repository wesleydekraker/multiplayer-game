using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.Rendering
{
    public class Sprite
    {
        public Texture2D Texture { get; }

        private Rectangle rectangle;

        public Sprite(Texture2D texture, int positionX, int positionY, int width, int height)
        {
            this.Texture = texture;
            this.rectangle = new Rectangle(positionX, positionY, width, height);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, this.rectangle, Color.White);
        }
    }
}