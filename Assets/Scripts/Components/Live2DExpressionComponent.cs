using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.IO;
using live2d;
using live2d.framework;

public class Live2DExpressionComponent : MonoBehaviour {
	public Live2DModelComponent modelComponent;

	private MotionQueueManager motionMgr;
	private L2DExpressionMotion motion;

	void Start() {
		motionMgr = new MotionQueueManager();

		if (modelComponent == null) {
			modelComponent = GetComponent<Live2DModelComponent>();
		}
	}

	public void ReleaseExpression() {
		motionMgr.stopAllMotions();
		motion = null;
	}

	public void LoadFromFile(string file) {
		motionMgr.stopAllMotions();
		motion = L2DExpressionMotion.loadJson(File.ReadAllBytes(file));
		motionMgr.startMotion(motion, false);
	}

	void Update() {
		if (motion == null) {
			return;
		}

		motionMgr.updateParam(modelComponent.model);
	}
}
