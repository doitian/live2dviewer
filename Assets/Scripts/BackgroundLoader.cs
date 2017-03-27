using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BackgroundLoader : MonoBehaviour {
	public Texture2D[] defaultTextures;
	public Texture2D[] textures;

	public Button button;

	private int currentTextureIndex = 0;
	private float ratio;

	void Start() {
		currentTextureIndex = 0;
		ApplyTexture();

		if (button != null) {
			button.onClick.AddListener(NextBackground);
			button.interactable = textures.Length > 1;
		}
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

	void NextBackground() {
		currentTextureIndex++;
		if (currentTextureIndex >= textures.Length) {
			currentTextureIndex = 0;
		}
		ApplyTexture();
	}

	void ApplyTexture() {
		var texture = textures[currentTextureIndex];
		GetComponent<Renderer>().material.mainTexture = texture;
		ratio = texture.texelSize.y / texture.texelSize.x;
	}
}
