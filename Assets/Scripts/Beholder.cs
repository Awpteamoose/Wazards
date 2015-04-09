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
	public static void Trigger(string key)
	{
		_Trigger<object, object, object, object, object>(key, null, null, null, null, null);
	}
	public static void Trigger<T1>(string key, T1 arg1)
	{
		_Trigger<T1, object, object, object, object>(key, arg1, null, null, null, null);
	}
	public static void Trigger<T1, T2>(string key, T1 arg1, T2 arg2)
	{
		_Trigger<T1, T2, object, object, object>(key, arg1, arg2, null, null, null);
	}
	public static void Trigger<T1, T2, T3>(string key, T1 arg1, T2 arg2, T3 arg3)
	{
		_Trigger<T1, T2, T3, object, object>(key, arg1, arg2, arg3, null, null);
	}
	public static void Trigger<T1, T2, T3, T4>(string key, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
	{
		_Trigger<T1, T2, T3, T4, object>(key, arg1, arg2, arg3, arg4, null);
	}
	public static void Trigger<T1, T2, T3, T4, T5>(string key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
	{
		_Trigger<T1, T2, T3, T4, T5>(key, arg1, arg2, arg3, arg4, arg5);
	}
	private static void _Trigger<T1, T2, T3, T4, T5>(string key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
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
	///	Beholder.Subscription sub = Beholder.Observe("event_name", cb);
	/// </summary>
	/// <param name="key">Plain string event name</param>
	/// <param name="cb">Callback function which will be called</param>
	public static Subscription Observe(string key, Callback cb)
	{
		Callback<object, object, object, object, object> _cb = (arg1, arg2, arg3, arg4, arg5) => cb();
		return _Observe<object, object, object, object, object>(key, _cb);
	}
	public static Subscription Observe<T1>(string key, Callback<T1> cb)
	{
		Callback<T1, object, object, object, object> _cb = (arg1, arg2, arg3, arg4, arg5) => cb(arg1);
		return _Observe<T1, object, object, object, object>(key, _cb);
	}
	public static Subscription Observe<T1, T2>(string key, Callback<T1, T2> cb)
	{
		Callback<T1, T2, object, object, object> _cb = (arg1, arg2, arg3, arg4, arg5) => cb(arg1, arg2);
		return _Observe<T1, T2, object, object, object>(key, _cb);
	}
	public static Subscription Observe<T1, T2, T3>(string key, Callback<T1, T2, T3> cb)
	{
		Callback<T1, T2, T3, object, object> _cb = (arg1, arg2, arg3, arg4, arg5) => cb(arg1, arg2, arg3);
		return _Observe<T1, T2, T3, object, object>(key, _cb);
	}
	public static Subscription Observe<T1, T2, T3, T4>(string key, Callback<T1, T2, T3, T4> cb)
	{
		Callback<T1, T2, T3, T4, object> _cb = (arg1, arg2, arg3, arg4, arg5) => cb(arg1, arg2, arg3, arg4);
		return _Observe<T1, T2, T3, T4, object>(key, _cb);
	}
	public static Subscription Observe<T1, T2, T3, T4, T5>(string key, Callback<T1, T2, T3, T4, T5> cb)
	{
		Callback<T1, T2, T3, T4, T5> _cb = (arg1, arg2, arg3, arg4, arg5) => cb(arg1, arg2, arg3, arg4, arg5);
		return _Observe<T1, T2, T3, T4, T5>(key, _cb);
	}
	private static Subscription _Observe<T1, T2, T3, T4, T5>(string key, Callback<T1, T2, T3, T4, T5> cb)
	{
		if (!_subscriptions.ContainsKey(key))
		{
			_subscriptions.Add(key, new List<object>());
		}
		_subscriptions[key].Add(cb);
		return new Subscription(key, cb);
	}
	
	/// <summary>
	/// Example usage:
	/// Beholder.Subscription sub = Beholder.Observe("event_name", cb);
	/// Beholder.Cancel(sub);
	/// </summary>
	/// <param name="sub"></param>
	public static void Cancel(Subscription sub)
	{
		int i;
		for (i = 0; i < _subscriptions[sub.key].Count; i++)
		{
			if (_subscriptions[sub.key][i] == sub.value) break;
		}

		if (i == 0)
		{
			_subscriptions.Remove(sub.key);
		}
		else
		{
			_subscriptions[sub.key].RemoveAt(i);
		}
	}

	public delegate void Callback();
	public delegate void Callback<T1>(T1 arg1);
	public delegate void Callback<T1, T2>(T1 arg1, T2 arg2);
	public delegate void Callback<T1, T2, T3>(T1 arg1, T2 arg2, T3 arg3);
	public delegate void Callback<T1, T2, T3, T4>(T1 arg1, T2 arg2, T3 arg3, T4 arg4);
	public delegate void Callback<T1, T2, T3, T4, T5>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);

	public struct Subscription
	{
		public string key;
		public object value;
		public Subscription(string _key, object _value)
		{
			key = _key;
			value = _value;
		}
	}

	private static Dictionary<String, List<object>> _subscriptions = new Dictionary<string, List<object>>();
}
