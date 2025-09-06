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
}
