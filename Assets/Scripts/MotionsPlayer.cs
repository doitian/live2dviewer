using UnityEngine;
using System.Collections;
using live2d;

public class MotionsPlayer : MonoBehaviour {
	public SimpleModel model;
	public TextAsset[] motionFiles;

	private MotionQueueManager motionMgr;
	private Live2DMotion[] motions;
	private int currentMotionIndex = 0;

	void Start() {
		motionMgr = new MotionQueueManager();
		motions = new Live2DMotion[motionFiles.Length];
		for (int i = 0; i < motionFiles.Length; i++) {
			motions[i] = Live2DMotion.loadMotion(motionFiles[i].bytes);
		}

		if (model == null) {
			model = GetComponent<SimpleModel>();
		}
	}
	
	void Update() {
		if (motions == null || motions.Length == 0) {
			return;
		}

		if (Input.GetMouseButtonDown(0)) {
			var motion = motions[currentMotionIndex];
			currentMotionIndex++;
			if (currentMotionIndex >= motions.Length) {
				currentMotionIndex = 0;
			}
			motionMgr.stopAllMotions();
			motionMgr.startMotion(motion);
		}

		motionMgr.updateParam(model.model);
	}
}
