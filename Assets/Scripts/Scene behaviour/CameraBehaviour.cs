using UnityEngine;
using System.Collections;

public class CameraBehaviour : MonoBehaviour {

	public float minSize = 7.5f;
	public float maxSize = 17.5f;

	void Update ()
    {
		camera.orthographicSize = minSize;
		foreach (PlayerControl player in PlayerControl.activePlayers) {
			if (Mathf.Abs(player.transform.position.x - 0f)+3f > camera.aspect*camera.orthographicSize || Mathf.Abs(player.transform.position.y - 0f)+3f > camera.orthographicSize)
			{
				camera.orthographicSize = Mathf.Max ((Mathf.Abs(player.transform.position.x) + 3f)/camera.aspect, Mathf.Abs(player.transform.position.y) + 3f);
			}
		}
		if (camera.orthographicSize > maxSize)
			camera.orthographicSize = maxSize;
	}
}
