using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class AnimatedMagma : MonoBehaviour
{
    public Vector2 speed;

    public new Transform transform { get; set; }

    void Awake()
    {
        transform = GetComponent<Transform>();
    }

    void Update()
    {
        float size = Camera.main.orthographicSize * 3f;
        transform.localScale = new Vector3(size * Camera.main.aspect, size, 0);
    }
}
