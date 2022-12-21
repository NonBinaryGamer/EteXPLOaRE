using System.Collections.Generic;

public static class Globals
{
    // ...

    public static List<UnitController> SELECTED_UNITS = new List<UnitController>();
    public static List<Utils.InventoryItem> REQUIRED_ITEMS = new List<Utils.InventoryItem>();
    public static int MUSIC_VOLUME = 25;
    public static int SFX_VOLUME = 50;
    public static GameManager MANAGER;

    public static bool GAME_ACTIVE = true;
}