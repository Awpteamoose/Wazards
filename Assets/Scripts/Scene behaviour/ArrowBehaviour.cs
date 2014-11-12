using UnityEngine;
using System.Collections;

public class ArrowBehaviour : MonoBehaviour {

    public Direction direction;
    public new Transform transform;
    public Transform parent;
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
	void Update () {
        float xInCamera = Mathf.Clamp(
            parent.transform.position.x,
            -Camera.main.orthographicSize * Camera.main.aspect,
            Camera.main.orthographicSize * Camera.main.aspect
        );
        float yInCamera = Mathf.Clamp(
            parent.transform.position.y,
            -Camera.main.orthographicSize,
            Camera.main.orthographicSize
        );
        Vector3 closestPointInCamera = new Vector3(xInCamera, yInCamera, 0);
        transform.position = closestPointInCamera - (parent.transform.position - closestPointInCamera);
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(parent.transform.position.y - transform.position.y, parent.transform.position.x - transform.position.x) * Mathf.Rad2Deg - 90f);

        parentImage.rotation = parent.transform.rotation;
        parentImageSpriteRenderer.sprite = parentSpriteRenderer.sprite;
	}
}
