using UnityEngine;

namespace nfd
{

	public class FileDialogResultLogger : MonoBehaviour {
		public FileDialog[] targets;

		void Awake() {
			if (targets == null || targets.Length == 0) {
				var target = GetComponent<FileDialog>();
				if (target != null) {
					targets = new FileDialog[] { target };
				}
			}
		}

		void Start() {
			if (targets != null) {
				for (int i = 0; i < targets.Length; i++) {
					targets[i].onResult.AddListener(Log);
				}
			}
		}

		void OnDestroy() {
			if (targets != null) {
				for (int i = 0; i < targets.Length; i++) {
					targets[i].onResult.RemoveListener(Log);
				}
			}
		}

		public void Log(NfdResult result, string outPath, string[] outPaths) {
			switch(result) {
				case NfdResult.NFD_OKAY:
					if (outPaths != null) {
						Debug.LogFormat("[NFD] Selected multiple ‹{0}›", string.Join("› ‹", outPaths));
					} else {
						Debug.LogFormat("[NFD] Selected ‹{0}›", outPath);
					}
					break;
				case NfdResult.NFD_ERROR:
					var err = NativeFileDialog.GetError();
					if (!string.IsNullOrEmpty(err)) {
						Debug.LogError("[NFD] Error: " + err);
					} else {
						Debug.LogError("[NFD] Error: Unknown Reason");
					}
					break;
				case NfdResult.NFD_CANCEL:
					Debug.Log("[NFD] Cancelled");
					break;
			}
		}
	}

}
