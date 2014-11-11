using UnityEngine;

public struct Direction
{
    private Vector3 _vector;
    private float _angle;
    private Quaternion _rotation;

    public Direction(Vector3 __vector)
    {
        _vector = Vector3.zero;
        _angle = 0f;
        _rotation = Quaternion.identity;

        vector = __vector;
    }

    public Direction(float __angle)
    {
        _vector = Vector3.zero;
        _angle = 0f;
        _rotation = Quaternion.identity;

        angle = __angle;
    }

    public Vector3 vector
    {
        get
        {
            return _vector;
        }
        set
        {
            if (value.magnitude > 1.0f)
                _vector = value.normalized;
            else
                _vector = value;

            _angle = Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg - 90f;
            _rotation = Quaternion.Euler(0, 0, _angle);
        }
    }

    public float angle
    {
        get
        {
            return _angle;
        }
        set
        {
            if (value > 360f)
                _angle = value % 360f;
            else
                _angle = value;

            _vector = (Quaternion.Euler(0, 0, angle) * Vector3.up).normalized;
            _rotation = Quaternion.Euler(0, 0, _angle);
        }
    }

    public Quaternion rotation
    {
        get
        {
            return _rotation;
        }
    }
}