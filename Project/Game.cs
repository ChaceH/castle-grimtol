using System;
using System.Collections.Generic;

namespace CastleGrimtol.Project
{
    public class Game : IGame
    {
        public IRoom CurrentRoom { get; set; }
        public IPlayer CurrentPlayer { get; set; }

        private Item mainKey;

        bool playing = true;

        public Game()
        {

        }

        public void Setup()
        {
            // Rooms
            IRoom PrisonCell = new Room("Prison Cell", "You are in an old prison cell. On the North side of the cell there is a door, it is slightly ajar.");
            IRoom CellBlock = new Room("Cell Block", "You are in a long hallway with cells lining the walls. At the North end of the hall you see a light.");
            IRoom Courtyard = new Room("Prison's Courtyard", "You are outside, but not free. You are in a courtyard surrounded by walls. \nTo the North, there is a door. To the West there is a small crevasse.");
            IRoom MainRoom = new Room("Prison's Main Room", "You are in a giant room with large double doors to the North, and a small door to the West.");
            IRoom Outside = new Room("Outside", "You made it out!");
            IRoom Waterway = new Room("Waterway", "You are standing in a dimbly lit room filled with shallow water.");
            IRoom DeathRoom = new Room("Mysterious Room", "You stumble into a dark room. You fumble around for a second trying to find an exit. \nYou hear something behind you you turn and scream in horror! You were eaten by a grue.");

            // Cell exits
            PrisonCell.Exits.Add("north", CellBlock);

            // CellBlock exits
            CellBlock.Exits.Add("north", Courtyard);
            CellBlock.Exits.Add("south", PrisonCell);

            // Courtyard exits
            Courtyard.Exits.Add("north", MainRoom);
            Courtyard.Exits.Add("south", CellBlock);
            Courtyard.Exits.Add("west", DeathRoom);

            // MainRoom exits
            MainRoom.Exits.Add("north", Outside);
            MainRoom.Exits.Add("south", Courtyard);
            MainRoom.Exits.Add("west", Waterway);

            // Waterway exits
            Waterway.Exits.Add("east", MainRoom);



            // Items
            mainKey = new Item("key", "It's a large key!");
            mainKey.IsPickup = true;
            mainKey.LookDescription = "Just under the surface of the water you can barely make out the edges of a key.";
            Waterway.Items.Add("key", mainKey);
            mainKey.ReleventRooms.Add(MainRoom);

            //Items required to enter rooms
            Outside.ItemsRequiredForEntry.Add(mainKey);
            Outside.IsLocked = true;

            CurrentRoom = PrisonCell;

            Console.Clear();
            Console.WriteLine("What's your name?");
            var name = Console.ReadLine();
            CurrentPlayer = new Player(name);
            Console.WriteLine(CurrentRoom.Description);
        }

        public void StartGame()
        {
            Setup();
            while (playing)
            {
                CheckWinConditions();
                GetUserInput();
            }
            Console.WriteLine("Thanks for playing!");
            Console.ReadLine();
        }

        public void GetUserInput()
        {
            Console.WriteLine("Type help for commands.");
            Console.WriteLine("What would you like to do?");
            string[] UserCommands;
            string input = Console.ReadLine().ToLower();
            UserCommands = input.Split(' ');

            switch (UserCommands[0])
            {

                // GO
                case "go":
                    Go(UserCommands[1]);
                    break;

                // TAKEITEM 
                case "take":
                    TakeItem(UserCommands[1]);
                    break;

                // USEITEM
                case "use":
                    UseItem(UserCommands[1]);
                    break;

                // INVENTORY
                case "inv":
                case "inventory":
                    Inventory();
                    break;

                // LOOK
                case "look":
                    Look();
                    break;

                // SEARCH
                case "search":
                    Search();
                    break;

                // RESET
                case "reset":
                    Reset();
                    break;

                // QUIT
                case "quit":
                    Quit();
                    break;

                //HELP
                case "help":
                    Help();
                    break;

                default:
                    Console.WriteLine("Unrecognized command... Type help for usable commands.");
                    break;
            }
            UserCommands = null;
        }

        public void Go(string direction)
        {
            if (CurrentRoom.Exits.ContainsKey(direction))
            {
                var nextRoom = CurrentRoom.Exits[direction];
                if (nextRoom.ItemsRequiredForEntry.Count > 0)
                {
                    var isMissingItems = false;
                    foreach (IItem item in nextRoom.ItemsRequiredForEntry)
                    {
                        if (!CurrentPlayer.Inventory.Contains(item))
                        {
                            Console.WriteLine($"You do not have the required item {item.Name} to enter this room.");
                            isMissingItems = true;
                        }
                    }
                    if (isMissingItems)
                    {
                        return;
                    } 
                }
                if(nextRoom.IsLocked) {
                    Console.WriteLine($"You must use {nextRoom.ItemsRequiredForEntry[0].Name} to unlock the door.");
                    return;
                }
                Console.Clear();
                CurrentRoom = CurrentRoom.Exits[direction];
                Console.WriteLine(CurrentRoom.Description);
                return;
            }
            System.Console.WriteLine("You run into a wall..");
        }

        public void TakeItem(string itemName)
        {
            IItem item;
            CurrentRoom.Items.TryGetValue(itemName, out item);
            if (item != null)
            {
                CurrentPlayer.Inventory.Add(item);
                Console.WriteLine($"You put the {item.Name} in your inventory.");
                CurrentRoom.Items.Remove(itemName);
            }
            else {
                System.Console.WriteLine("There are no items in this room.");
            }
        }

        public void UseItem(string itemName)
        {
            foreach (var item in CurrentPlayer.Inventory) {
                if(item.Name.Equals(itemName))
                {
                    foreach(var room in CurrentRoom.Exits) {
                        if (room.Value.ItemsRequiredForEntry.Contains(item)) {
                            System.Console.WriteLine($"You use {item.Name}.");
                            room.Value.IsLocked = false;
                            return;
                        }
                    }
                    System.Console.WriteLine($"{item.Name} cannot be used here.");
                    return;
                }
            }
            System.Console.WriteLine("You do not have that item.");
        }

        public void Inventory()
        {
            if (CurrentPlayer.Inventory.Count > 0)
            {
                foreach (var item in CurrentPlayer.Inventory)
                {
                    Console.WriteLine($"Name:{item.Name}\nDescription:{item.Description}");
                }
            }
            else
            {
                Console.WriteLine("You have nothing in your inventory.");
            }
        }

        public void Look()
        {
            Console.WriteLine(CurrentRoom.Description);
        }

        public void Search()
        {
            if (CurrentRoom.Items.Count > 0)
            {
                foreach (var item in CurrentRoom.Items)
                {
                    Console.WriteLine(item.Value.LookDescription);
                }
                return;
            }
            Console.WriteLine("There is no items in this room.");
        }

        public void Reset()
        {
            Console.WriteLine("Are you sure you would like to restart? (Y/N)");
            var line = Console.ReadLine();
            if ("Y".Equals(line.ToUpper()))
            {
                Setup();
                return;
            }
            Console.WriteLine(CurrentRoom.Description);
        }

        public void Quit()
        {
            playing = false;
        }

        public void Help()
        {
            Console.WriteLine("Go <direction> \nUse <item> \nTake <item> \nInventory, or inv (displays player inventory) \nLook (displays current room description) \nSearch (searches current room for items) \nReset (resets the game) \nQuit (quits current game) ");
        }

        public void CheckWinConditions()
        {
            if (CurrentRoom.Name.Equals("Outside"))
            {
                Console.Clear();
                Console.WriteLine("Congratulations you won! Press any key to continue...");
                Console.ReadLine();
                Console.WriteLine("Would you like to play again? (Y/N)");
                var line = Console.ReadLine();
                if ("Y".Equals(line.ToUpper()))
                {
                    Setup();
                    return;
                }
                System.Console.WriteLine("Thanks for playing! BYE!");
            }
            else if(CurrentRoom.Name.Equals("Mysterious Room")){
                Console.WriteLine("Would you like to play again? (Y/N)");
                var line = Console.ReadLine();
                if ("Y".Equals(line.ToUpper()))
                {
                    Setup();
                    return;
                }
                System.Console.WriteLine("Thanks for playing! BYE!");
            }
        }
    }
}