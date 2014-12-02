using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public float shakeAmount;
    public float decreaseFactor;

    // Update is called once per frame
    void Update()
    {
        if (shakeAmount > 0)
        {
            Camera.main.transform.localPosition = new Vector3(Random.insideUnitSphere.x * shakeAmount, Random.insideUnitSphere.y * shakeAmount, -1f);
            shakeAmount -= Time.deltaTime * shakeAmount * decreaseFactor;
            if (shakeAmount <= 0.001f)
            {
                shakeAmount = 0;
                Camera.main.transform.localPosition = new Vector3(0, 0, -1f);
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
