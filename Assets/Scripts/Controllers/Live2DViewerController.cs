using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class Live2DViewerController : MonoBehaviour
{
	public Live2DViewerConfig config;
	public BackgroundComponent backgroundComponent;
	public Live2DModelComponent modelComponent;

	public Text indicatorTitle;
	public Text indicatorBody;

	public GameObject settingsPrefab;

	public void OnConfigChanged(Live2DViewerConfig c, Live2DViewerConfigChangeType t) {
		config = c;
		switch(t) {
			case Live2DViewerConfigChangeType.RootFolder:
				if (config.models.Length > 0) {
					var current = config.currentModel;
					modelComponent.LoadFromFiles(current.mocFile, current.textureFiles);
				} else {
					modelComponent.ReleaseModel();
				}
				UpdateIndicator();
				break;
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

	void UpdateIndicator() {
		if (config.models.Length > 0) {
			var current = config.currentModel;
			indicatorTitle.text = current.name;
			indicatorBody.text = String.Format("{0}/{1}", config.currentModelIndex + 1, config.models.Length);
		} else {
			indicatorTitle.text = "未加载";
			indicatorBody.text = "";
		}
	}
}
