using System.Collections.Generic;
using UnityEngine;

public static class DataHolder
{
    public static List<string> teams = new List<string>() { "Black", "White", "Red", "Yellow" };
    public static Dictionary<string, int> TeamsCoins = new Dictionary<string, int>()
    {
        { "Black", 0 },
        { "White", 0 },
        { "Red", 0 },
        { "Yellow", 0 }
    };
    public static List<string> materials = new List<string>() { "free", "ice", "trap", "crocodile", "cannibal", "fort", "revive fort", "chest 1", "chest 2", "chest 3", "chest 4", "chest 5", "plane", "plane", "cannon left", "cannon up", "cannon right", "cannon down", "rum", "left", "right", "up", "down", "4 sides", "up or down", "up or left", "up or right", "down or left", "left or right", "down or right", "2 turns", "3 turns", "4 turns", "5 turns", "horse", "water", "grass", "BlackShip", "WhiteShip", "RedShip", "YellowShip" };
    public static List<string> darkMaterials = new List<string>() { "dark free", "dark ice", "dark trap", "dark crocodile", "dark cannibal", "dark fort", "dark revive fort", "dark chest 1", "dark chest 2", "dark chest 3", "dark chest 4", "dark chest 5", "dark plane", "dark plane", "dark cannon left", "dark cannon up", "dark cannon right", "dark cannon down", "dark rum", "dark left", "dark right", "dark up", "dark down", "dark 4 sides", "dark up or down", "dark up or left", "dark up or right", "dark down or left", "dark left or right", "dark down or right", "dark 2 turns", "dark 3 turns", "dark 4 turns", "dark 5 turns", "dark horse", "dark water", "dark grass", "dark BlackShip", "dark WhiteShip", "dark RedShip", "dark YellowShip" };

}
