using UnityEngine;
using System.Collections;

public class Live2DViewerController : MonoBehaviour
{
	public Live2DViewerConfig config;
	public BackgroundComponent backgroundComponent;

	public GameObject settingsPrefab;

	public void OnConfigChanged(Live2DViewerConfig c, Live2DViewerConfigChangeType t) {
		config = c;
		switch(t) {
			case Live2DViewerConfigChangeType.Background:
				backgroundComponent.LoadFromFile(config.backgroundTexturePath);
				break;
		}
	}

	public void ShowConfigPanel() {
		var configController = Instantiate<GameObject>(settingsPrefab, transform.parent.transform, false).GetComponent<ConfigController>();
		configController.config = config;
		configController.Render();
		configController.onConfigChanged.AddListener(OnConfigChanged);
	}

	public void PrevModel() {
	}

	public void NextModel() {
	}

	public void NextMotion() {
	}
}
