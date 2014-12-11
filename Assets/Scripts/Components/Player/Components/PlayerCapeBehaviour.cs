using UnityEngine;
using System.Collections;

public class PlayerCapeBehaviour : MonoBehaviour
{
    public int _positions;
    public float vert_distance;

    private LineRenderer cape;
    private Vector3[] positions;
    private new Transform transform;

    // Use this for initialization
    void Awake()
    {
        transform = GetComponent<Transform>();
        cape = GetComponent<LineRenderer>();
        positions = new Vector3[_positions];
        for (int i = 0; i < positions.Length; i++)
        {
            positions[i] = transform.position;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        positions[0] = transform.position;
        for (int i = 1; i < positions.Length; i++)
        {
            Vector3 diff = positions[i - 1] - positions[i];
            //positions[i] += diff.normalized * vert_distance;
            //diff = positions[i - 1] - positions[i];
            if (diff.magnitude > vert_distance)
            {
                positions[i] = positions[i - 1] - diff.normalized * vert_distance;
            }
            //else
            //{
            //    positions[i] = positions[i - 1];
            //}
        }

        for (int i = 0; i < positions.Length; i++)
        {
            cape.SetPosition(i, positions[i]);
        }
    }
}
