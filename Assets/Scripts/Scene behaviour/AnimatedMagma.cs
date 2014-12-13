using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class AnimatedMagma : MonoBehaviour
{
    public Vector2 speed;

    public new Transform transform;
    public new Renderer renderer;

    void Awake()
    {
        transform = GetComponent<Transform>();
        renderer = GetComponent<Renderer>();
    }

    void Update()
    {
        float size = Camera.main.orthographicSize * 2f;
        transform.localScale = new Vector3(size * Camera.main.aspect, size, 0);
        renderer.material.mainTextureOffset = renderer.material.mainTextureOffset + speed * Time.deltaTime;
    }
}
