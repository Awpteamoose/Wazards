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
		maxTextureScale = GetComponent<Renderer>().material.mainTextureScale.x;
	}

	void FixedUpdate()
	{
		float newScale = transform.localScale.x-shrinkSpeed*Time.fixedDeltaTime;
		if (newScale > minScaleFactor*maxScale)
		{
            float percent_newScale = newScale / transform.localScale.x;
			float textureSize = GetComponent<Renderer>().material.mainTexture.width;
            float textureScale = percent_newScale * GetComponent<Renderer>().material.mainTextureScale.x;
			float shrinkby = (textureSize-(textureScale/maxTextureScale*textureSize))/textureSize;

            transform.localScale = new Vector3(newScale, newScale, 0);
			GetComponent<Renderer>().material.mainTextureScale = new Vector2(textureScale, textureScale);
			GetComponent<Renderer>().material.mainTextureOffset = new Vector2(shrinkby, shrinkby);
		}
	}
}
