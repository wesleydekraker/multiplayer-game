namespace Shared
{
    public class Player
    {
        public string UserId { get; }
        public float X { get; }
        public float Y { get; }

        public Player(string userId, float x, float y)
        {
            this.UserId = userId;
            this.X = x;
            this.Y = y;
        }
    }
}
