namespace Shared
{
    public class UserInput
    {
        public string UserId { get; }
        public bool Up { get; }
        public bool Down { get; }
        public bool Left { get; }
        public bool Right { get; }

        public UserInput(string userId, bool up, bool down, bool left, bool right)
        {
            this.UserId = userId;
            this.Up = up;
            this.Down = down;
            this.Left = left;
            this.Right = right;
        }
    }
}
