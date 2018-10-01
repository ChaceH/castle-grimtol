using System.Collections.Generic;

namespace CastleGrimtol.Project
{
    public interface IRoom
    {
        string Name { get; set; }
        string Description { get; set; }
        Dictionary<string, IItem> Items {get; set;}
        Dictionary<string, IRoom> Exits { get; set; }
        IList<IItem> ItemsRequiredForEntry {get; set;}
        bool IsLocked { get; set; }
    }
}
