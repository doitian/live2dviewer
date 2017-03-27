using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using live2d;

public class MotionsPlayer : MonoBehaviour {
	public SimpleModel model;
	public TextAsset[] motionFiles;
	public bool loop;

	private MotionQueueManager motionMgr;
	private Live2DMotion[] motions;
	private bool running;
	private int currentMotionIndex = -1;

	public void ToggleLoop() {
		loop = !loop;
	}

	void Start() {
		motionMgr = new MotionQueueManager();
		motions = new Live2DMotion[motionFiles.Length];
		for (int i = 0; i < motionFiles.Length; i++) {
			motions[i] = Live2DMotion.loadMotion(motionFiles[i].bytes);
		}
		currentMotionIndex = motionFiles.Length - 1;

		if (model == null) {
			model = GetComponent<SimpleModel>();
		}
	}

	void Update() {
		if (motions == null || motions.Length == 0) {
			return;
		}

		if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) {
			currentMotionIndex++;
			if (currentMotionIndex >= motions.Length) {
				currentMotionIndex = 0;
			}
			var motion = motions[currentMotionIndex];
			motionMgr.stopAllMotions();
			motionMgr.startMotion(motion);
			running = true;
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
		motionMgr.updateParam(model.model);
	}
}
