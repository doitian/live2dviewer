using UnityEngine;
using UnityEngine.EventSystems;

public class CameraDragMove : MonoBehaviour {
	private Vector3 dragOrigin;
	private Vector3 originPosition;

	void Update() {
		if (Input.GetMouseButtonDown(0)) {
			dragOrigin = Input.mousePosition;
			originPosition = transform.position;
			return;
		}

		if (!Input.GetMouseButton(0)) return;

		var diff = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
		transform.position = originPosition - new Vector3(
			diff.x * 2 * Camera.main.orthographicSize * Screen.width / Screen.height,
			diff.y * 2 * Camera.main.orthographicSize,
			0f
		);
	}
}