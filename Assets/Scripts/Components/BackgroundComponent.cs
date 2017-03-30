using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BackgroundComponent : MonoBehaviour {
	public Texture2D defaultTexture;
	public Texture2D texture;
	private float ratio;

	void Start() {
		if (texture == null) {
			texture = defaultTexture;
		}
		ApplyTexture();
	}

	void Update() {
		var scale = Vector3.one;
		var screenRatio = (float)Screen.width / Screen.height;
		if (screenRatio > ratio) {
			scale.x = Camera.main.orthographicSize * 2 * Screen.width / Screen.height;
			scale.y = scale.x / ratio;
		} else {
			scale.y = Camera.main.orthographicSize * 2;
			scale.x = scale.y * ratio;
		}

		transform.localScale = scale;
	}

	void ApplyTexture() {
		GetComponent<Renderer>().material.mainTexture = texture;
		ratio = texture.texelSize.y / texture.texelSize.x;
	}
}
