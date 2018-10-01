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
            bgColor = Console.BackgroundColor;
            fgColor = Console.BackgroundColor;
            try
            {
                Game game = new Game();
                game.StartGame();
                Console.Clear();
            }
            finally
            {
                Console.BackgroundColor = bgColor;
                Console.ForegroundColor = fgColor;
            }
        }
    }
}
