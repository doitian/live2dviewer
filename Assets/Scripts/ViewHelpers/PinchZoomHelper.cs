using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class PinchZoomHelper : MonoBehaviour
{
	public float perspectiveZoomSpeed = 0.5f;        // The rate of change of the field of view in perspective mode.
	public float orthoZoomSpeed = 0.5f;        // The rate of change of the orthographic size in orthographic mode.
	public float mouseScrollWheelSpeed = 1.0f;

	public Camera zoomCamera;

	public UnityEvent onZoomChanged;

	void Start() {
		if (zoomCamera == null) {
			zoomCamera = Camera.main;
		}
	}

	void Update() {
		// If there are two touches on the device...
		if (Input.touchCount == 2) {
			// Store both touches.
			Touch touchZero = Input.GetTouch(0);
			Touch touchOne = Input.GetTouch(1);

			// Find the position in the previous frame of each touch.
			Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
			Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

			// Find the magnitude of the vector (the distance) between the touches in each frame.
			float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
			float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

			// Find the difference in the distances between each frame.
			float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

			Zoom(deltaMagnitudeDiff);

			return;
		}

		if (!EventSystem.current.IsPointerOverGameObject()) {
			var d = Input.GetAxis("Mouse ScrollWheel");
			if (d == 0f) {
				return;
			}
			Zoom(mouseScrollWheelSpeed * d);
		}
	}

	public void Zoom(float delta) {
		// If the camera is orthographic...
		if (zoomCamera.orthographic) {
			// ... change the orthographic size based on the change in distance between the touches.
			zoomCamera.orthographicSize += delta * orthoZoomSpeed;

			// Make sure the orthographic size never drops below zero.
			zoomCamera.orthographicSize = Mathf.Max(zoomCamera.orthographicSize, 0.1f);
		} else {
			// Otherwise change the field of view based on the change in distance between the touches.
			zoomCamera.fieldOfView += delta * perspectiveZoomSpeed;

			// Clamp the field of view to make sure it's between 0 and 180.
			zoomCamera.fieldOfView = Mathf.Clamp(zoomCamera.fieldOfView, 0.1f, 179.9f);
		}
		if (onZoomChanged != null) {
			onZoomChanged.Invoke();
		}
	}
}