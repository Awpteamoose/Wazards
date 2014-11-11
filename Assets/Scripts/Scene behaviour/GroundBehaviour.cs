using UnityEngine;
using System.Collections;

public class GroundBehaviour : MonoBehaviour
{
	public float shrinkSpeed;
	public float minScaleFactor;
	private float maxScale;
	private float maxTextureScale;

	void Start()
	{
		maxScale = transform.localScale.x;
		maxTextureScale = renderer.material.mainTextureScale.x;
	}

	void FixedUpdate()
	{
		float newScale = transform.localScale.x-shrinkSpeed*Time.fixedDeltaTime;
		if (newScale > minScaleFactor*maxScale)
		{
            float percent_newScale = newScale / transform.localScale.x;
            float shaderNewScale = renderer.material.GetVector("_Distort").z / percent_newScale;
			float textureSize = renderer.material.mainTexture.width;
            float textureScale = percent_newScale * renderer.material.mainTextureScale.x;
			float shrinkby = (textureSize-(textureScale/maxTextureScale*textureSize))/textureSize;

            renderer.material.SetVector("_Distort", new Vector4(1, 1, shaderNewScale, shaderNewScale));
            transform.localScale = new Vector3(newScale, newScale, 0);
			renderer.material.mainTextureScale = new Vector2(textureScale, textureScale);
			renderer.material.mainTextureOffset = new Vector2(shrinkby, shrinkby);
		}
	}
}
