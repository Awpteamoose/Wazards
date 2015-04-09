using System;
using System.Collections.Generic;

/// <summary>
/// A class that implements the Observer pattern without resorting to individual events, makes it all a lot more abstract.
/// </summary>
public static class Beholder
{
	/// <summary>
	/// Example usage: Beholder.Trigger("event_name", 50, true);
	/// </summary>
	/// <param name="key">Plain string event name</param>
	/// <param name="...">Up to 5 arguments to be passed to all subscribers</param>
	public static void Trigger<T1>(string key, T1 arg1)
	{
		if (_subscriptions.ContainsKey(key)) {
			foreach (Callback<T1> cb in _subscriptions[key])
			{
				cb(arg1);
			}
		}
	}
	public static void Trigger<T1, T2>(string key, T1 arg1, T2 arg2)
	{
		if (_subscriptions.ContainsKey(key))
		{
			foreach (Callback<T1, T2> cb in _subscriptions[key])
			{
				cb(arg1, arg2);
			}
		}
	}
	public static void Trigger<T1, T2, T3>(string key, T1 arg1, T2 arg2, T3 arg3)
	{
		if (_subscriptions.ContainsKey(key))
		{
			foreach (Callback<T1, T2, T3> cb in _subscriptions[key])
			{
				cb(arg1, arg2, arg3);
			}
		}
	}
	public static void Trigger<T1, T2, T3, T4>(string key, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
	{
		if (_subscriptions.ContainsKey(key))
		{
			foreach (Callback<T1, T2, T3, T4> cb in _subscriptions[key])
			{
				cb(arg1, arg2, arg3, arg4);
			}
		}
	}
	public static void Trigger<T1, T2, T3, T4, T5>(string key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
	{
		if (_subscriptions.ContainsKey(key))
		{
			foreach (Callback<T1, T2, T3, T4, T5> cb in _subscriptions[key])
			{
				cb(arg1, arg2, arg3, arg4, arg5);
			}
		}
	}

	/// <summary>
	/// Example usage:
	/// Beholder.Callback<int, bool> cb = (i, b) =>
	///	{
	///		if (b) {
	///			Debug.Log(i + 50);
	///		} else {
	///			Debug.Log(i - 50);
	///		}
	///	};
	///	Beholder.Observe("event_name", cb);
	/// </summary>
	/// <param name="key">Plain string event name</param>
	/// <param name="cb">Callback function which will be called</param>
	public static void Observe<T1>(string key, Callback<T1> cb)
	{
		if (!_subscriptions.ContainsKey(key)) {
			_subscriptions.Add(key, new List<object>());
		}
		_subscriptions[key].Add(cb);
	}
	public static void Observe<T1, T2>(string key, Callback<T1, T2> cb)
	{
		if (!_subscriptions.ContainsKey(key))
		{
			_subscriptions.Add(key, new List<object>());
		}
		_subscriptions[key].Add(cb);
	}
	public static void Observe<T1, T2, T3>(string key, Callback<T1, T2, T3> cb)
	{
		if (!_subscriptions.ContainsKey(key))
		{
			_subscriptions.Add(key, new List<object>());
		}
		_subscriptions[key].Add(cb);
	}
	public static void Observe<T1, T2, T3, T4>(string key, Callback<T1, T2, T3, T4> cb)
	{
		if (!_subscriptions.ContainsKey(key))
		{
			_subscriptions.Add(key, new List<object>());
		}
		_subscriptions[key].Add(cb);
	}
	public static void Observe<T1, T2, T3, T4, T5>(string key, Callback<T1, T2, T3, T4, T5> cb)
	{
		if (!_subscriptions.ContainsKey(key))
		{
			_subscriptions.Add(key, new List<object>());
		}
		_subscriptions[key].Add(cb);
	}

	/// <summary>
	/// Example usage: Beholder.StopObserving("event_name", cb);
	/// </summary>
	/// <param name="key">Plain string event name</param>
	/// <param name="cb">The callback you wish to remove</param>
	public static void StopObserving<T1>(string key, Callback<T1> cb)
	{
		int i;
		for (i = 0; i < _subscriptions[key].Count; i++)
		{
			if (_subscriptions[key][i] == cb) break;
		}

		if (i == 0)
		{
			_subscriptions.Remove(key);
		}
		else
		{
			_subscriptions[key].RemoveAt(i);
		}
	}
	public static void StopObserving<T1, T2>(string key, Callback<T1, T2> cb)
	{
		int i;
		for (i = 0; i < _subscriptions[key].Count; i++)
		{
			if (_subscriptions[key][i] == cb) break;
		}

		if (i == 0)
		{
			_subscriptions.Remove(key);
		}
		else
		{
			_subscriptions[key].RemoveAt(i);
		}
	}
	public static void StopObserving<T1, T2, T3>(string key, Callback<T1, T2, T3> cb)
	{
		int i;
		for (i = 0; i < _subscriptions[key].Count; i++)
		{
			if (_subscriptions[key][i] == cb) break;
		}

		if (i == 0)
		{
			_subscriptions.Remove(key);
		}
		else
		{
			_subscriptions[key].RemoveAt(i);
		}
	}
	public static void StopObserving<T1, T2, T3, T4>(string key, Callback<T1, T2, T3, T4> cb)
	{
		int i;
		for (i = 0; i < _subscriptions[key].Count; i++)
		{
			if (_subscriptions[key][i] == cb) break;
		}

		if (i == 0)
		{
			_subscriptions.Remove(key);
		}
		else
		{
			_subscriptions[key].RemoveAt(i);
		}
	}
	public static void StopObserving<T1, T2, T3, T4, T5>(string key, Callback<T1, T2, T3, T4, T5> cb)
	{
		int i;
		for (i = 0; i < _subscriptions[key].Count; i++)
		{
			if (_subscriptions[key][i] == cb) break;
		}

		if (i == 0)
		{
			_subscriptions.Remove(key);
		}
		else
		{
			_subscriptions[key].RemoveAt(i);
		}
	}

	
	public delegate void Callback<T1>(T1 arg1);
	public delegate void Callback<T1, T2>(T1 arg1, T2 arg2);
	public delegate void Callback<T1, T2, T3>(T1 arg1, T2 arg2, T3 arg3);
	public delegate void Callback<T1, T2, T3, T4>(T1 arg1, T2 arg2, T3 arg3, T4 arg4);
	public delegate void Callback<T1, T2, T3, T4, T5>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);

	private static Dictionary<String, List<object>> _subscriptions = new Dictionary<string, List<object>>();
}
