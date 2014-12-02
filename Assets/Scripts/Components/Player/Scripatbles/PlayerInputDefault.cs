using UnityEngine;
using System.Collections;

public class PlayerInputDefault : ScriptableObject
{
    [System.Serializable]
    public class Binding
    {
        public string name;
        public KeyCode key;
    }

    public Binding[] bindings;
}
