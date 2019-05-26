using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.IO;

public class ExpressionChooserController : MonoBehaviour
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
		vm.titleText.text = "选择表情";
		var content = vm.content;

		if (config.models.Length > 0) {
			var current = config.currentModel;
			for (int i = 0; i < current.expressionFiles.Length; i++) {
				var file = current.expressionFiles[i];
				var itemGo = Instantiate<GameObject>(itemPrefab, content.transform, false);
				itemGo.GetComponent<Button>().onClick.AddListener(OnSelectExpression(go, i));
				var title = itemGo.transform.Find("title");
				title.GetComponent<Text>().text = Path.GetFileNameWithoutExtension(file);
			}
		}
	}

	UnityAction OnSelectExpression(GameObject go, int i) {
		return () => {
			configController.OnSelectExpression(i);	
			Destroy(go);
		};
	}
}

