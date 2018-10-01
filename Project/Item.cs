using System.Collections.Generic;

namespace CastleGrimtol.Project
{
    public class Item : IItem
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IList<IRoom> ReleventRooms { get; set; }
        public string LookDescription { get; set; }
        public bool IsPickup { get; set; }
        public Item(string name, string description)
        {
            Name = name;
            Description = description;
            ReleventRooms = new List<IRoom>();
        }
    }
}