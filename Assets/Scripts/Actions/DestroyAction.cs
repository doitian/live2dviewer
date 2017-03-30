using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAction : MonoBehaviour {
	public GameObject target;

	void Start() {
		if (target == null) {
			target = gameObject;
		}
	}

	public void Perform() {
		Destroy(target);
	}
}
