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

    public static long GetTimePassSinceTimeLongValue(long value)
    {
        var time = GetLocalTimeByLong(value);
        //Debug.LogError($"Now: {DateTime.Now} - Save: {time}");
        return DateTime.Now.Subtract(time).Seconds;
    }

    public static long AddTimeSecond2DateTime(long targetTime, long addSec)
    {
        var time = GetLocalTimeByLong(targetTime);
        time.AddSeconds(addSec);
        return time.ToFileTime();
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
