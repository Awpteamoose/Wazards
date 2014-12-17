using UnityEngine;
using System.Collections;

public class ArrowBehaviour : MonoBehaviour {

    public Direction direction;
    public new Transform transform { get; set; }
    public PlayerControl parent;
    private SpriteRenderer parentSpriteRenderer;
    public Transform parentImage;
    private SpriteRenderer parentImageSpriteRenderer;

	// Use this for initialization
	void Start () {
        transform = GetComponent<Transform>();
        parentSpriteRenderer = parent.GetComponent<SpriteRenderer>();
        parentImageSpriteRenderer = parentImage.GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 closestPointInCamera = new Vector2(
            Mathf.Clamp(
                parent.transform.position.x,
                -Camera.main.orthographicSize * Camera.main.aspect,
                Camera.main.orthographicSize * Camera.main.aspect
            ),
            Mathf.Clamp(
                parent.transform.position.y,
                -Camera.main.orthographicSize,
                Camera.main.orthographicSize
            )
        );
        transform.position = closestPointInCamera - (parent.transform.position - closestPointInCamera);
        direction.vector = parent.transform.position - transform.position;
        transform.rotation = direction.rotation;

        parentImage.rotation = parent.transform.rotation;
        parentImageSpriteRenderer.sprite = parentSpriteRenderer.sprite;
	}
}
