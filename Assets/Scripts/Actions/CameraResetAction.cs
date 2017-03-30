using UnityEngine;
using System.Collections;

public class CameraResetAction : MonoBehaviour
{
	public Vector3 position;
	public Vector3 scale;
	public Quaternion rotation;
	public float orthographicSize;

	// Use this for initialization
	void Start()
	{
		var t = Camera.main.transform;
		position = t.localPosition;
		scale = t.localScale;
		rotation = t.localRotation;
		orthographicSize = Camera.main.orthographicSize;
	}
	
	public void Perform()
	{
		var t = Camera.main.transform;
		t.localPosition = position;
		t.localScale = scale;
		t.localRotation = rotation;

		Camera.main.orthographicSize = orthographicSize;
	}
}

