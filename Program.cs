using System;
using CastleGrimtol.Project;

namespace CastleGrimtol
{
    public class Program
    {
        private Game game;
        private static ConsoleColor bgColor;
        private static ConsoleColor fgColor;
        public static void Main(string[] args)
        {
                Game game = new Game();
                game.StartGame();
                Console.Clear();
        }
    }
}
