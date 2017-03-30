using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIButtonLabelsInteractable : MonoBehaviour
{
	public Button button;
	public Text[] labels;
	public Color[] colors;

	private Color[] normalColors;

	public bool interactable {
		get { return button.interactable; }
		set {
			button.interactable = value;
			SyncState();
		}
	}

	public bool nonInteractable {
		get { return !button.interactable; }
		set {
			interactable = !value;
		}
	}

	void Awake() {
		if (button == null) {
			button = GetComponent<Button>();
			if (button == null) {
				return;
			}
		}

		normalColors = new Color[labels.Length];
		for (int i = 0; i < labels.Length; i++) {
			normalColors[i] = labels[i].color;
		}
	}

	void Start() {
		SyncState();
	}

	void SyncState() {
		if (button.interactable) {
			for (int i = 0; i < labels.Length; i++) {
				labels[i].color = normalColors[i];
			}
		} else {
			for (int i = 0; i < labels.Length; i++) {
				labels[i].color = colors[i];
			}
		}
	}
}

