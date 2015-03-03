using UnityEngine;
using System.Collections;

public class CameraBehaviour : MonoBehaviour {

	public float minSize = 7.5f;
	public float maxSize = 17.5f;

	void Update ()
    {
		GetComponent<Camera>().orthographicSize = minSize;
		foreach (PlayerControl player in PlayerControl.activePlayers) {
			if (Mathf.Abs(player.transform.position.x - 0f)+3f > GetComponent<Camera>().aspect*GetComponent<Camera>().orthographicSize || Mathf.Abs(player.transform.position.y - 0f)+3f > GetComponent<Camera>().orthographicSize)
			{
				GetComponent<Camera>().orthographicSize = Mathf.Max ((Mathf.Abs(player.transform.position.x) + 3f)/GetComponent<Camera>().aspect, Mathf.Abs(player.transform.position.y) + 3f);
			}
		}
		if (GetComponent<Camera>().orthographicSize > maxSize)
			GetComponent<Camera>().orthographicSize = maxSize;
	}
}
