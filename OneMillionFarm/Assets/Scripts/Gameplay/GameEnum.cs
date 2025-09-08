using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    NONE = -1,
    
    /// SEED ITEM 1XX
    SEED_TOMATO = 100,
    SEED_BLUE_BERRY = 101,
    SEED_STRAW_BERRY = 102,
    SEED_COW = 103,

    /// CREATURE ITEM 2XX
    CREATURE_TOMATO = 200,
    CREATURE_BLUE_BERRY = 201,
    CREATURE_STRAW_BERRY = 202,
    CREATURE_COW = 203,

    ///PRODUCT ITEM 3XX
    PRODUCT_TOMATO = 300,
    PRODUCT_BLUE_BERRY = 301,
    PRODUCT_STRAW_BERRY = 302,
    PRODUCT_COW = 303,


    WORKER = 500,

    FARMTILE = 600,
}

public enum SlotType
{
    NONE = -1,
    INVENTORY = 0,
    TOOLBAR = 1,
}

public enum DialogType
{
    NONE = -1,
    INVENTORY_DIALOG = 0,
    STORE_DIALOG = 1,
    WIN_GAME_DIALOG = 2,
    INFO_CREATURE_DIALOG = 3,
}