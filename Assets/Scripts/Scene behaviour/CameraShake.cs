using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public float shakeAmount;
    public float shakeFactor;
    public float decrease;

    // Update is called once per frame
    void Update()
    {
        if (shakeAmount > 0)
        {
            foreach (Camera camera in Camera.allCameras)
            camera.transform.localPosition = new Vector3(Random.insideUnitSphere.x * shakeAmount, Random.insideUnitSphere.y * shakeAmount, -1f);
            shakeAmount -= Time.deltaTime * (Mathf.Pow(shakeAmount, shakeFactor) + decrease);
            if (shakeAmount <= 0.001f)
            {
                shakeAmount = 0;
                GetComponent<Camera>().transform.localPosition = new Vector3(0, 0, -1f);
            }
        }
    }

    public static void Shake(Camera cam, float amount)
    {
        cam.GetComponent<CameraShake>().shakeAmount += amount;
    }
}

public static class Extensions
{
    public static void Shake(this Camera cam, float amount)
    {
        CameraShake.Shake(cam, amount);
    }
}
