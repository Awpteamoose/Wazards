using System;
using System.Collections.Generic;

namespace StateMachine
{
	public class State
	{
		public virtual void Start () {}
		public virtual void Update() {}
		public virtual void FixedUpdate() {}
		public virtual void OnGUI() {}
		public virtual void Exit() {}
	}
	
	public class Machine <T>
	{
		private State _activeState;
		public Dictionary<T, State> states = new Dictionary<T, State>();

		public Machine()
		{
			_activeState = new State ();
		}
		public void Update()
		{
			_activeState.Update ();
		}

		public void FixedUpdate()
		{
			_activeState.FixedUpdate ();
		}

		public void OnGUI()
		{
			_activeState.OnGUI ();
		}
		
		public State get()
		{
			return _activeState;
		}
		public void set(State newstate)
		{
			_activeState.Exit ();
			_activeState = newstate;
			_activeState.Start ();
		}
		
	}
}