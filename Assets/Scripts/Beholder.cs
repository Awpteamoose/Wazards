using System;
using System.Collections.Generic;

/// <summary>
/// A class that implements the Observer pattern without resorting to individual events, makes it all a lot more abstract.
/// </summary>
public static class Beholder
{
    /// <summary>
    /// Typical usage: Beholder.Trigger("event_name", ...);
    /// </summary>
    /// <param name="key">plain string name of the event</param>
    /// <param name="list">varargs that will be passed as object[] to all subscribers.</param>
    public static void Trigger(string key, params object[] list)
    {
        if (subscriptions.ContainsKey(key)) {
            foreach (Callback cb in subscriptions[key])
            {
                cb(list);
            }
        }
    }

    /// <summary>
    /// Typical usage:
    /// Beholder.Callback cb = list =>
    /// {
    ///     function body, operating on list[]
    ///     most likely statically casting it to proper types
    /// };
    /// Beholder.Observe("event_name", cb);
    /// </summary>
    /// <param name="key">plain string name of the event</param>
    /// <param name="cb">callback that will be executed upon the event being triggered</param>
    public static void Observe(string key, Callback cb)
    {
        if (!subscriptions.ContainsKey(key)) {
            subscriptions.Add(key, new List<Callback>());
        }
        subscriptions[key].Add(cb);
    }

    /// <summary>
    /// Typical usage: Beholder.StopObserving("event_name", cb);
    /// </summary>
    /// <param name="key">plain string name of the event</param>
    /// <param name="cb">callback that is meant to be removed</param>
    public static void StopObserving(string key, Callback cb)
    {
        int i;
        for (i = 0; i < subscriptions[key].Count; i++)
        {
            if (subscriptions[key][i] == cb) break;
        }

        if (i == 0) {
            subscriptions.Remove(key);
        } else {
            subscriptions[key].RemoveAt(i);
        }
    }

    private static Dictionary<String, List<Callback>> subscriptions = new Dictionary<string,List<Callback>>();

    public delegate void Callback(params object[] list);
}
