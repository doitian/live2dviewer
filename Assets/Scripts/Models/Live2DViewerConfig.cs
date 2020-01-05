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
	public float opacity;
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
	public string poseFile;

	public Live2DPartConfig[] parts;

	public int currentMotionIndex;
	public int currentExpressionIndex;
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

	public void NextModel() {
		currentModelIndex++;
		if (currentModelIndex >= models.Length) {
			currentModelIndex = 0;
		}
	}
	public void PrevModel() {
		currentModelIndex--;
		if (currentModelIndex < 0) {
			currentModelIndex = models.Length - 1;
		}
	}

	public void ScanFolder(string path) {
		var modelList = new List<Live2DModelConfig>();

		foreach (string file in Directory.GetDirectories(path)) {
			var modelConfig = TryScanModelFolder(file);
			if (modelConfig != null) {
				modelList.Add(modelConfig);
			}
		}

		if (modelList.Count == 0) {
			var modelConfig = TryScanModelFolder(path);
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
		if (String.IsNullOrEmpty(config.name)) {
			config.name = Path.GetFileName(Path.GetDirectoryName(path));
		}
		config.path = path;

		var mocFiles = Directory.GetFiles(path, "*.moc", SearchOption.AllDirectories);
		if (mocFiles.Length == 0) {
			mocFiles = Directory.GetFiles(path, "*.moc.bytes", SearchOption.AllDirectories); // also search bytes files
			if (mocFiles.Length == 0)
				return null;
		}

		config.mocFile = mocFiles[0];
		var basename = Path.GetFileNameWithoutExtension(config.mocFile.Replace(".bytes",""));

		foreach (string textureDir in Directory.GetDirectories(path, basename + ".*")) {
			var textures = Directory.GetFiles(textureDir, "*.png");
			if (textures.Length > 0) {
				Array.Sort(textures);
				config.textureFiles = textures;
				break;
			}
		}

		config.motionFiles = Directory.GetFiles(path, "*.mtn", SearchOption.AllDirectories);
		if(config.motionFiles.Length == 0)
			config.motionFiles = Directory.GetFiles(path, "*.mtn.bytes", SearchOption.AllDirectories);

		config.expressionFiles = Directory.GetFiles(path, "*.exp.json", SearchOption.AllDirectories);
		var poseFiles = Directory.GetFiles(path, "*.pose.json", SearchOption.AllDirectories);
		if (poseFiles.Length > 0) config.poseFile = poseFiles[0];

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
