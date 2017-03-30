using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

[Serializable]
public class Live2DPartConfig
{
	public string name;
	public int index;
	public bool visible;
}

[Serializable]
public class Live2DModelConfig
{
	public string name;

	public string path;
	public string mocFile;
	public string[] textureFiles;
	public string[] motionFiles;
	public string[] expressionFiles;

	public Live2DPartConfig[] parts;

	public int currentMotion = 0;
	public int currentExpression = 0;
}

[Serializable]
public class Live2DViewerConfig
{
	public string rootFolder;
	public Live2DModelConfig[] models;
	public int currentModelIndex;
	public string backgroundTexturePath;
	public bool loopMotion = false;

	public Live2DModelConfig currentModel {
		get { return models[currentModelIndex]; } 
	}

	public void ScanFolder(string path) {
		var modelList = new List<Live2DModelConfig>();

		foreach (string file in Directory.GetDirectories(path)) {
			var modelConfig = TryScanModelFolder(file);
			if (modelConfig != null) {
				modelList.Add(modelConfig);
			}
		}

		models = modelList.ToArray();
		rootFolder = path;
		currentModelIndex = 0;
	}

	private Live2DModelConfig TryScanModelFolder(string path) {
		var config = new Live2DModelConfig();

		config.name = Path.GetFileName(path);
		config.path = path;

		var mocFiles = Directory.GetFiles(path, "*.moc", SearchOption.AllDirectories);
		if (mocFiles.Length == 0) {
			return null;
		}

		config.mocFile = mocFiles[0];
		var basename = Path.GetFileNameWithoutExtension(config.mocFile);

		foreach (string textureDir in Directory.GetDirectories(path, basename + ".*")) {
			var textures = Directory.GetFiles(textureDir, "*.png");
			if (textures.Length > 0) {
				Array.Sort(textures);
				config.textureFiles = textures;
				break;
			}
		}

		config.motionFiles = Directory.GetFiles(path, "*.mtn", SearchOption.AllDirectories);
		config.expressionFiles = Directory.GetFiles(path, "*.exp.json", SearchOption.AllDirectories);

		return config;
	}
}

public enum Live2DViewerConfigChangeType {
	RootFolder,
	Model,
	Background,
	LoopMotion,
	Motion,
	Expression,
	Parts
}

[System.Serializable]
public class Live2DViewerConfigEvent : UnityEvent<Live2DViewerConfig, Live2DViewerConfigChangeType>
{
}
