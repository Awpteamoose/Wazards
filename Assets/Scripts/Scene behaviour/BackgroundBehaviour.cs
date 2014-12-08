using UnityEngine;
using System.Collections;

public class BackgroundBehaviour : MonoBehaviour {

    private new Camera camera;
    private new Transform transform;

	// Use this for initialization
	void Start ()
    {
        camera = GetComponent<Camera>();
        transform = GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        camera.orthographicSize = Camera.main.orthographicSize;
        transform.position = Camera.main.transform.position;
	}
}
