using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

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

	public void LoadFromFile(string path) {
		try {
			if (path == null) {
				texture = defaultTexture;
			} else {
				var newTexture = new Texture2D(2, 2);
				newTexture.LoadImage(File.ReadAllBytes(path));
				texture = newTexture;
			}
		} catch {
			texture = defaultTexture;
		}

		ApplyTexture();
	}

	public void Update() {
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
