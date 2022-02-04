using System;
using System.Threading.Tasks;

namespace Server
{
    internal class Program
    {
        private const int FPS = 60;

        static async Task Main(string[] args)
        {
            var previousGameTime = DateTime.Now;
            var multiplayerGame = new MultiplayerGame();

            while (true)
            {
                var delta = DateTime.Now - previousGameTime;
                previousGameTime += delta;

                multiplayerGame.Update(delta);
                
                await Task.Delay(1000 / FPS);
            }
        }        
    }
}
