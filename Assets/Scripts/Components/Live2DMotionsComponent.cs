using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.IO;
using live2d;
using live2d.framework;

public class Live2DMotionsComponent : MonoBehaviour {
	public Live2DModelComponent modelComponent;
	public TextAsset[] motionFiles;
	public bool loop;
	public float dragWaitSeconds = 0.5f;

	private MotionQueueManager motionMgr;
	private Live2DMotion[] motions;
	private bool running;
	private int currentMotionIndex = -1;
	private float startClickTime = 0f;

	public void ToggleLoop() {
		loop = !loop;
	}

	void Start() {
		motionMgr = new MotionQueueManager();

		if (modelComponent == null) {
			modelComponent = GetComponent<Live2DModelComponent>();
		}

		motions = new Live2DMotion[motionFiles.Length];
		for (int i = 0; i < motionFiles.Length; i++) {
			motions[i] = Live2DMotion.loadMotion(motionFiles[i].bytes);
		}
		currentMotionIndex = motionFiles.Length - 1;
	}

	public void LoadFromFile(string[] motionFilePaths) {
		motionMgr.stopAllMotions ();
		running = false;

		motions = new Live2DMotion[motionFilePaths.Length];
		for (int i = 0; i < motionFilePaths.Length; i++) {
			motions[i] = Live2DMotion.loadMotion(File.ReadAllBytes(motionFilePaths[i]));
		}

		currentMotionIndex = motionFiles.Length - 1;
	}

	public void ReleaseMotions() {
		motions = null;
	}

	public void NextMotion() {
		if (motions == null || motions.Length == 0) {
			return;
		}

		currentMotionIndex++;
		if (currentMotionIndex >= motions.Length) {
			currentMotionIndex = 0;
		}
		var motion = motions[currentMotionIndex];
		motionMgr.stopAllMotions();
		motionMgr.startMotion(motion);
		running = true;
	}

	public void PlayMotion(int i) {
		if (motions == null || motions.Length == 0) {
			return;
		}

		if (i < 0 || i >= motions.Length) {
			i = 0;
		}

		currentMotionIndex = i;
		var motion = motions[i];
		motionMgr.stopAllMotions();
		motionMgr.startMotion(motion);
		running = true;
	}

	void Update() {
		if (motions == null || motions.Length == 0) {
			return;
		}

		if (!EventSystem.current.IsPointerOverGameObject()) {
			if (Input.GetMouseButtonDown (0)) {
				startClickTime = Time.realtimeSinceStartup;
			}

			if (Input.GetMouseButtonUp (0) && Time.realtimeSinceStartup - startClickTime < dragWaitSeconds) {
				NextMotion();
			}
		}

		if (motionMgr.isFinished()) {
			if (running) {
				if (loop) {
					motionMgr.startMotion(motions[currentMotionIndex]);
				} else {
					running = false;
				}
			} else {
				return;
			}
		}
		motionMgr.updateParam(modelComponent.model);
	}
}
