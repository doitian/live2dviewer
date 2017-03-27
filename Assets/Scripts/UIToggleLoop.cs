using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIToggleLoop : MonoBehaviour {
	public MotionsPlayer motionsPlayer;
	public Color enabledColor = Color.white;
	public Color disabledColor = new Color(0.8f, 0.8f, 0.8f);

	private Image image;

	// Use this for initialization
	void Start() {
		image = GetComponent<Image>();
		var button = GetComponent<Button>();
		button.onClick.AddListener(Toggle);
		SyncState();
	}

	public void Toggle() {
		motionsPlayer.ToggleLoop();
		SyncState();
	}

	void SyncState() {
		if (motionsPlayer.loop) {
			image.color = enabledColor;
		} else {
			image.color = disabledColor;
		}
	}
}
