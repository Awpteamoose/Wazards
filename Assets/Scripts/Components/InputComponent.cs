using UnityEngine;
using System.Collections;

public class InputComponent : MonoBehaviour
{
	public bool _active = true;
	public virtual float getHorizontal() {return 0;}
	public virtual float getVertical() {return 0;}
	public virtual bool getFire(int num) {return false;}
	public virtual bool getFireDown(int num) {return false;}
	public virtual bool getFireUp(int num) {return false;}
}
