using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBackgroundToggle : MonoBehaviour {
	public Image graphic;

	public void OnValueChanged(bool enabled) {
		graphic.enabled = !enabled;
	}
}
