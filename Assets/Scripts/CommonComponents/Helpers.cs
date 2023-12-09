using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helpers
{
    //this helper method calls the given action after a given amount of seconds
    public static IEnumerator SetTimer(float seconds, Action action)
    {
        yield return new WaitForSeconds(seconds);
        action();
        yield return null;
    }
    //this helper method calls the given action for a given iteration count,
    //waiting a given amount of seconds between each call
    public static IEnumerator RepeatWithDelay(int iterations, float seconds, Action action)
    {
        for(int i=0;i<iterations; i++)
        {
            yield return new WaitForSeconds(seconds);
            action();
        }
    }
}
