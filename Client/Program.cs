using System;

namespace Client
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            new MultiplayerGame().Run();
        }
    }
}
