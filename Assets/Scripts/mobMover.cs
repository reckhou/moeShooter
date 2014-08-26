using UnityEngine;
using System.Collections;

public class mobMover : MonoBehaviour {
	public float speed;
	void Start() {

	}

	void FixedUpdate() {
		Vector3 newPos = transform.position;
		newPos.y -= speed;
		transform.position = newPos;
	}
}
