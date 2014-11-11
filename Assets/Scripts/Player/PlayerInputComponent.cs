using UnityEngine;
using System.Collections;

public class PlayerInputComponent : InputComponent
{
	public string playername;

	public override float getHorizontal()
	{
		if (_active)
			return Input.GetAxisRaw (playername+" Horizontal");
		else
			return 0;
	}

	public override float getVertical()
	{
		if (_active)
			return Input.GetAxisRaw (playername+" Vertical");
		else
			return 0;
	}

	public override bool getFire(int num)
	{
		if (_active)
			return Input.GetButton (playername+" Fire "+num);
		else
			return false;
	}

	public override bool getFireDown(int num)
	{
		if (_active)
			return Input.GetButtonDown (playername+" Fire "+num);
		else
			return false;
	}

	public override bool getFireUp(int num)
	{
		if (_active)
			return Input.GetButtonUp (playername+" Fire "+num);
		else
			return true;
	}
}
