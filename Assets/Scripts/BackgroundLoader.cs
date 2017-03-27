using UnityEngine;
using System.Collections;

public class BackgroundLoader : MonoBehaviour {
	public Texture2D texture;

	private float ratio;

	void Start() {
		GetComponent<Renderer>().material.mainTexture = texture;
		ratio = texture.texelSize.y / texture.texelSize.x;
	}

	void Update() {
		var scale = Vector3.one;
		var screenRatio = (float)Screen.width / Screen.height;
		Debug.Log("screen ratio = " + screenRatio);
		if (screenRatio > ratio) {
			scale.x = Camera.main.orthographicSize * 2 * Screen.width / Screen.height;
			scale.y = scale.x / ratio;
		} else {
			scale.y = Camera.main.orthographicSize * 2;
			scale.x = scale.y * ratio;
		}

		transform.localScale = scale;
	}
}
