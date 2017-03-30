using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Live2DPartConfig
{
	public string name;
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
}

[Serializable]
public class Live2DViewerConfig
{
	public string rootFolder;
	public Live2DModelConfig[] models;
	public int currentModelIndex;
	public string backgroundTexturePath;
	public bool loopMotion = false;
}
