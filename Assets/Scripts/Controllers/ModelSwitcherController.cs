using UnityEngine;
using System.Collections;

public class ModelSwitcherController : MonoBehaviour
{
	public GameObject prefab;
	public Transform parent;
	public ConfigController configController;

	void Start() {
		if (parent == null) {
			parent = transform.parent.transform;
		}
		if (configController == null) {
			configController = GetComponent<ConfigController>();
		}
	}

	public void LaunchDialog() {
		var go = Instantiate<GameObject>(prefab, parent, false);
		go.GetComponent<UISubmitInputField>().onSubmit.AddListener(
			configController.OnSelectModel
		);
	}
}

