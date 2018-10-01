using System.Collections.Generic;

namespace CastleGrimtol.Project
{
  public interface IItem
  {
    string Name { get; set; }
    string Description { get; set; }
    string LookDescription { get; set; }
    bool IsPickup { get; set; }
    IList<IRoom> ReleventRooms { get; set; }
  }
}