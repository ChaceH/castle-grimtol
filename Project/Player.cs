using System.Collections.Generic;

namespace CastleGrimtol.Project
{
    public class Player : IPlayer
    {
        public string PlayerName { get; set; }
        public List<IItem> Inventory { get; set; }

        public Player(string name)
        {
            PlayerName = name;
            Inventory = new List<IItem>();
        }
    }
}