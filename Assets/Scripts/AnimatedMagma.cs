using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

[ExecuteInEditMode]
public class AnimatedMagma : MonoBehaviour
{
    public Vector2 speed;

    private Vector2 _speed;

    void Start()
    {
        _speed = speed * Time.fixedDeltaTime;
    }

    void FixedUpdate()
    {
        renderer.material.mainTextureOffset = renderer.material.mainTextureOffset + _speed;
    }
}
