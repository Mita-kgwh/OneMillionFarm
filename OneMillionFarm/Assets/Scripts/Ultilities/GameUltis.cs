using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameUltis
{
    public static long GetLocalLongTime()
    {
        return DateTime.Now.ToFileTime();
    }

    public static DateTime GetLocalTimeByLong(long value)
    {
        return DateTime.FromFileTime(value);
    }

    public static ItemType ConvertTypeSeed2Creature(ItemType seedType)
    {
        return (ItemType)((int)seedType + 100);
    }
    
    public static ItemType ConvertTypeCreature2Product(ItemType creatureType)
    {
        return (ItemType)((int)creatureType + 100);
    }


}
