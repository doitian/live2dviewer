using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class Live2DViewerController : MonoBehaviour
{
	public Live2DViewerConfig config;
	public BackgroundComponent backgroundComponent;
	public Live2DModelComponent modelComponent;
	public Live2DMotionsComponent motionsComponent;
	public Live2DExpressionComponent expComponent;
	public CameraResetAction cameraResetAction;

	public Text indicatorTitle;
	public Text indicatorBody;

	public GameObject settingsPrefab;

	public void OnConfigChanged(Live2DViewerConfig c, Live2DViewerConfigChangeType t) {
		config = c;
		switch(t) {
			case Live2DViewerConfigChangeType.RootFolder:
			case Live2DViewerConfigChangeType.Model:
				if (config.models.Length > 0) {
					var current = config.currentModel;
					modelComponent.LoadFromFiles(current.mocFile, current.textureFiles, current.poseFile);
					if (current.parts == null || current.parts.Length == 0) {
						current.parts = modelComponent.LoadParts();
					} else {
						modelComponent.SetParts(current.parts);
					}

					motionsComponent.LoadFromFile(current.motionFiles);
				} else {
					modelComponent.ReleaseModel();
					motionsComponent.ReleaseMotions();
				}
				expComponent.ReleaseExpression();
				cameraResetAction.Perform();
				UpdateIndicator();
				break;
			case Live2DViewerConfigChangeType.Motion:
				motionsComponent.PlayMotion(config.currentModel.currentMotionIndex);
				break;
			case Live2DViewerConfigChangeType.Expression:
				expComponent.LoadFromFile(config.currentModel.expressionFiles[config.currentModel.currentExpressionIndex]);
				break;
			case Live2DViewerConfigChangeType.LoopMotion:
				motionsComponent.loop = config.loopMotion;
				break;
			case Live2DViewerConfigChangeType.Parts:
				modelComponent.SetParts(config.currentModel.parts);
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
		config.PrevModel();
		OnConfigChanged(config, Live2DViewerConfigChangeType.Model);
	}

	public void NextModel() {
		config.NextModel();
		OnConfigChanged(config, Live2DViewerConfigChangeType.Model);
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

	void Update() {
		if (Input.GetKeyUp(KeyCode.LeftArrow)) {
			PrevModel();
		}
		if (Input.GetKeyUp(KeyCode.RightArrow)) {
			NextModel();
		}
		if (Input.GetKeyUp(KeyCode.Space)) {
			motionsComponent.NextMotion();
		}
	}
}
