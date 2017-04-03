using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

public class ConfigController : MonoBehaviour
{
	const string ROOT_DIR_DEFAULT_PATH_PREF_KEY = "ROOT_DIR_DEFUALT_PATH";

	public Live2DViewerConfig config;

	public Live2DViewerConfigEvent onConfigChanged;

	public Text rootFolderPathText;
	public Text currentModelText;
	public Toggle builtinBackgroundToggle;
	public Text backgroundPathText;
	public Toggle loopMotionToggle;
	public nfd.FileDialog rootFileDialog;

	public void Start() {
		var historyFolder = PlayerPrefs.GetString(ROOT_DIR_DEFAULT_PATH_PREF_KEY) ?? "";
		if (Directory.Exists(historyFolder)) {
			rootFileDialog.defaultPath = historyFolder;
		}
		this.onConfigChanged.AddListener(Render);
	}

	public void Render() {
		Render(config, Live2DViewerConfigChangeType.RootFolder);
		Render(config, Live2DViewerConfigChangeType.Model);
		Render(config, Live2DViewerConfigChangeType.Background);
		Render(config, Live2DViewerConfigChangeType.LoopMotion);
		Render(config, Live2DViewerConfigChangeType.Motion);
		Render(config, Live2DViewerConfigChangeType.Expression);
		Render(config, Live2DViewerConfigChangeType.Parts);
	}

	private void Render(Live2DViewerConfig c, Live2DViewerConfigChangeType t) {
		switch(t) {
			case Live2DViewerConfigChangeType.RootFolder:
			case Live2DViewerConfigChangeType.Model:
				rootFolderPathText.text = c.rootFolder;
				if (config.models.Length > 0) {
					var current = config.currentModel;
					currentModelText.text = String.Format("{0} ({1}/{2})", current.name, config.currentModelIndex + 1, config.models.Length);
				} else {
					currentModelText.text = "未加载";
				}
				break;
			case Live2DViewerConfigChangeType.LoopMotion:
				loopMotionToggle.isOn = c.loopMotion;
				break;
			case Live2DViewerConfigChangeType.Background:
				if (String.IsNullOrEmpty(config.backgroundTexturePath)) {
					backgroundPathText.text = "未选择";
				} else {
					backgroundPathText.text = config.backgroundTexturePath;
					builtinBackgroundToggle.isOn = false;
				}
				break;
		}
	}

	public void OnSelectRootFolder(nfd.NfdResult r, string path, string[] paths) {
		if (r == nfd.NfdResult.NFD_OKAY) {
			try {
				config.ScanFolder(path);
				TriggerChanges(Live2DViewerConfigChangeType.RootFolder);

				PlayerPrefs.SetString(ROOT_DIR_DEFAULT_PATH_PREF_KEY, path);

				Destroy(gameObject);
			} catch (Exception ex) {
				Debug.LogException(ex);
			}
		}
	}

	public void OnSelectModel(string name) {
		for (int i = 0; i < config.models.Length; i++) {
			var model = config.models[i];
			if (model.name == name) {
				config.currentModelIndex = i;
				TriggerChanges(Live2DViewerConfigChangeType.Model);

				Destroy(gameObject);
			}
		}
	}

	public void OnSelectMotion(int motionIndex) {
		if (config.models.Length == 0) {
			return;
		}

		var current = config.currentModel;
		if (motionIndex >= 0 && motionIndex < current.motionFiles.Length) {
			current.currentMotionIndex = motionIndex;
			TriggerChanges(Live2DViewerConfigChangeType.Motion);

			Destroy(gameObject);
		}
	}

	public void OnSelectExpression(int index) {
		if (config.models.Length == 0) {
			return;
		}

		var current = config.currentModel;
		if (index >= 0 && index < current.expressionFiles.Length) {
			current.currentExpressionIndex =  index;
			TriggerChanges(Live2DViewerConfigChangeType.Expression);

			Destroy(gameObject);
		}
	}

	public void OnLoopMotionChanged(bool loop) {
		config.loopMotion = loop;
		TriggerChanges(Live2DViewerConfigChangeType.LoopMotion);
	}

	public void OnUseBuiltinBackgroundChanged(bool enabled) {
		config.backgroundTexturePath = null;
		TriggerChanges(Live2DViewerConfigChangeType.Background);
	}

	public void OnSelectBackgroundPath(nfd.NfdResult r, string path, string[] paths) {
		if (r == nfd.NfdResult.NFD_OKAY) {
			config.backgroundTexturePath = path;
			TriggerChanges(Live2DViewerConfigChangeType.Background);
			Destroy(gameObject);
		}
	}

	public void OnPartsChanged() {
		TriggerChanges(Live2DViewerConfigChangeType.Parts);
	}

	private void TriggerChanges(Live2DViewerConfigChangeType t) {
		onConfigChanged.Invoke(config, t);
	}
}

