using System;
using System.Collections.Generic;

/// <summary>
/// A class that implements the Observer pattern without resorting to individual events, makes it all a lot more abstract.
/// </summary>
public static class Beholder
{
	/// <summary>
	/// Example usage:<para />
	/// Beholder.Trigger("event_name", ...);
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
	/// Example usage:<para />
	/// Beholder.Callback&lt;int, bool&gt; cb = (i, b) =&gt; { if (b) Debug.Log(i); };<para />
	///	Beholder.Observe("event_name", cb);
	/// </summary>
	/// <param name="key">Plain string event name</param>
	/// <param name="cb">Callback function which will be called</param>
	/// <param name="sub">An optional subscription, that can be used to cancel any number of observations</param>
	public static void Observe(string key, Callback cb, Subscription sub = null)
	{
		Callback<object, object, object, object, object> _cb = (arg1, arg2, arg3, arg4, arg5) => cb();
		_Observe<object, object, object, object, object>(key, _cb, sub);
	}
	public static void Observe<T1>(string key, Callback<T1> cb, Subscription sub = null)
	{
		Callback<T1, object, object, object, object> _cb = (arg1, arg2, arg3, arg4, arg5) => cb(arg1);
		_Observe<T1, object, object, object, object>(key, _cb, sub);
	}
	public static void Observe<T1, T2>(string key, Callback<T1, T2> cb, Subscription sub = null)
	{
		Callback<T1, T2, object, object, object> _cb = (arg1, arg2, arg3, arg4, arg5) => cb(arg1, arg2);
		_Observe<T1, T2, object, object, object>(key, _cb, sub);
	}
	public static void Observe<T1, T2, T3>(string key, Callback<T1, T2, T3> cb, Subscription sub = null)
	{
		Callback<T1, T2, T3, object, object> _cb = (arg1, arg2, arg3, arg4, arg5) => cb(arg1, arg2, arg3);
		_Observe<T1, T2, T3, object, object>(key, _cb, sub);
	}
	public static void Observe<T1, T2, T3, T4>(string key, Callback<T1, T2, T3, T4> cb, Subscription sub = null)
	{
		Callback<T1, T2, T3, T4, object> _cb = (arg1, arg2, arg3, arg4, arg5) => cb(arg1, arg2, arg3, arg4);
		_Observe<T1, T2, T3, T4, object>(key, _cb, sub);
	}
	public static void Observe<T1, T2, T3, T4, T5>(string key, Callback<T1, T2, T3, T4, T5> cb, Subscription sub = null)
	{
		Callback<T1, T2, T3, T4, T5> _cb = (arg1, arg2, arg3, arg4, arg5) => cb(arg1, arg2, arg3, arg4, arg5);
		_Observe<T1, T2, T3, T4, T5>(key, _cb, sub);
	}
	private static void _Observe<T1, T2, T3, T4, T5>(string key, Callback<T1, T2, T3, T4, T5> cb, Subscription sub)
	{
		if (!_subscriptions.ContainsKey(key))
		{
			_subscriptions.Add(key, new List<object>());
		}
		_subscriptions[key].Add(cb);
		if (sub != null) sub._subscriptions.Add(key, cb);
	}
	
	/// <summary>
	/// Example usage:<para />
	/// Beholder.Subscription sub = new Beholder.Subscription();<para />
	/// Beholder.Observe("event_name", cb, sub);<para />
	/// Beholder.Cancel(sub);
	/// </summary>
	/// <param name="sub">The subscription to be cancelled</param>
	public static void Cancel(Subscription sub)
	{
		foreach (var item in sub._subscriptions)
		{
			int i;
			for (i = 0; i < _subscriptions[item.Key].Count; i++)
			{
				if (_subscriptions[item.Key][i] == item.Value) break;
			}

			if (i == 0)
			{
				_subscriptions.Remove(item.Key);
			}
			else
			{
				_subscriptions[item.Key].RemoveAt(i);
			}
		}
	}

	public delegate void Callback();
	public delegate void Callback<T1>(T1 arg1);
	public delegate void Callback<T1, T2>(T1 arg1, T2 arg2);
	public delegate void Callback<T1, T2, T3>(T1 arg1, T2 arg2, T3 arg3);
	public delegate void Callback<T1, T2, T3, T4>(T1 arg1, T2 arg2, T3 arg3, T4 arg4);
	public delegate void Callback<T1, T2, T3, T4, T5>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);

	public class Subscription
	{
		public Dictionary<string, object> _subscriptions;
		public Subscription()
		{
			_subscriptions = new Dictionary<string, object>();
		}
	}

	private static Dictionary<String, List<object>> _subscriptions = new Dictionary<string, List<object>>();
}
