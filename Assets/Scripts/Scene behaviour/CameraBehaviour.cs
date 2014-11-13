using UnityEngine;
using System.Collections;

public class CameraBehaviour : MonoBehaviour {

	public float minSize = 7.5f;
	public float maxSize = 25f;
	private PlayerControl[] players;

	// Use this for initialization
	void Start ()
    {
		players = FindObjectsOfType(typeof(PlayerControl)) as PlayerControl[];
	}

	void Update ()
    {
		camera.orthographicSize = minSize;
		foreach (PlayerControl player in players) {
			if (Mathf.Abs(player.transform.position.x - 0f)+3f > camera.aspect*camera.orthographicSize || Mathf.Abs(player.transform.position.y - 0f)+3f > camera.orthographicSize)
			{
				camera.orthographicSize = Mathf.Max ((Mathf.Abs(player.transform.position.x) + 3f)/camera.aspect, Mathf.Abs(player.transform.position.y) + 3f);
			}
		}
		if (camera.orthographicSize > maxSize)
			camera.orthographicSize = maxSize;
	}
}
