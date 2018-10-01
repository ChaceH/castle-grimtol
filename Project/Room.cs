using System.Collections.Generic;

namespace CastleGrimtol.Project
{
    public class Room : IRoom
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Dictionary<string, IItem> Items { get; set; }
        public Dictionary<string, IRoom> Exits { get; set; }
        public IList<IItem> ItemsRequiredForEntry { get; set; }
        public bool IsLocked { get; set; }

        public Room(string name, string description)
        {
            Name = name;
            Description = description;
            Items = new Dictionary<string, IItem>();
            Exits = new Dictionary<string, IRoom>();
            ItemsRequiredForEntry = new List<IItem>();
            IsLocked = false;
        }
    }
}