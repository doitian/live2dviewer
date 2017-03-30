using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[System.Serializable]
public class UISubmitInputFieldEvent : UnityEvent<string> {
}

public class UISubmitInputField : MonoBehaviour {
	public InputField inputField;

	public UISubmitInputFieldEvent onSubmit;

	public void Submit() {
		if (onSubmit != null) {
			onSubmit.Invoke(inputField.text);
		}
	}
}
