using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.IO;

public class PartOpacitySlidersController : MonoBehaviour
{
	public GameObject prefab;
	public GameObject itemPrefab;
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
		var config = configController.config;
		var vm = go.GetComponent<DialogViewModel>();
		vm.titleText.text = "隐藏部件";
		vm.closeButtonText.text = "关闭";
		var content = vm.content;

		if (config.models.Length > 0) {
			var current = config.currentModel;
			for (int i = 0; i < current.parts.Length; i++) {
				var p = current.parts[i];
				var itemGo = Instantiate<GameObject>(itemPrefab, content.transform, false);
				var title = itemGo.transform.Find("title");
				title.GetComponent<Text>().text = p.name;
				var slider = itemGo.GetComponentInChildren<Slider>();
				slider.value = p.opacity;
				slider.onValueChanged.AddListener((v) => {
					p.opacity = v;
					configController.OnPartsChanged();
				});
			}
		}
	}

	UnityAction OnSelectMotion(GameObject go, int i) {
		return () => {
			configController.OnSelectMotion(i);	
			Destroy(go);
		};
	}
}

