using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerInputComponent : InputComponent
{
	public string playername;
    public static Dictionary<string, KeyCode> mappings;
    public static string[] inputNames;
    public PlayerInputDefault playerInputDefault;

    public void Awake ()
    {
        if (mappings == null)
        {
            inputNames = new string[8];
            inputNames[0] = "Up";
            inputNames[1] = "Left";
            inputNames[2] = "Down";
            inputNames[3] = "Right";
            inputNames[4] = "Spell 1";
            inputNames[5] = "Spell 2";
            inputNames[6] = "Spell 3";
            inputNames[7] = "Attack";

            mappings = new Dictionary<string, KeyCode>();
            for (int i = 1; i <= 4; i++)
            {
                for (int j = 0; j < inputNames.Length; j++)
                {
                    string name = "Player " + i + " " + inputNames[j];
                    KeyCode key = (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString(name, KeyCode.None.ToString()));
                    mappings.Add(name, key);
                }
            }

            for (int i = 0; i < playerInputDefault.bindings.Length; i++)
            {
                if (mappings[playerInputDefault.bindings[i].name] == KeyCode.None)
                {
                    PlayerPrefs.SetString(playerInputDefault.bindings[i].name, playerInputDefault.bindings[i].key.ToString());
                    mappings[playerInputDefault.bindings[i].name] = playerInputDefault.bindings[i].key;
                }
            }
            PlayerPrefs.Save();
        }
    }

	public override float getHorizontal()
	{
        if (_active)
        {
            if (Input.GetKey(mappings[playername + " Left"]) && !Input.GetKey(mappings[playername + " Right"]))
                return -1;
            else if (!Input.GetKey(mappings[playername + " Left"]) && Input.GetKey(mappings[playername + " Right"]))
                return 1;
            else
                return 0;
        }
        else
            return 0;
	}

	public override float getVertical()
	{
        if (_active)
        {
            if (Input.GetKey(mappings[playername + " Down"]) && !Input.GetKey(mappings[playername + " Up"]))
                return -1;
            else if (!Input.GetKey(mappings[playername + " Down"]) && Input.GetKey(mappings[playername + " Up"]))
                return 1;
            else
                return 0;
        }
        else
            return 0;
	}

	public override bool getFire(int num)
	{
        if (_active)
        {
            if (num < 4)
                return Input.GetKey(mappings[playername + " Spell " + num]);
            else
                return Input.GetKey(mappings[playername + " Attack"]);
        }
        else
            return false;
	}

	public override bool getFireDown(int num)
	{
        if (_active)
        {
            if (num < 4)
                return Input.GetKeyDown(mappings[playername + " Spell " + num]);
            else
                return Input.GetKeyDown(mappings[playername + " Attack"]);
        }
		else
			return false;
	}

	public override bool getFireUp(int num)
	{
        if (_active)
        {
            if (num < 4)
                return Input.GetKeyUp(mappings[playername + " Spell " + num]);
            else
                return Input.GetKeyUp(mappings[playername + " Attack"]);
        }
        else
            return true;
	}
}
