using UnityEngine;
using System.Collections;

public class mobMover : MonoBehaviour {
	public float speed;
	public float deltaX;
	public float scaleFactor;

	private float scaleDeltaStep;

	private float moveTime;
	void Start() {
		Vector3 scale = transform.localScale;
		transform.localScale *= scaleFactor;
		moveTime = 7.05f / speed; // 7.05: total range of deltaY
		scaleDeltaStep = (1.0f - scaleFactor) * scale.x / moveTime;
//		scaleDeltaStep = 0.4f;
		Debug.Log(moveTime);
		Debug.Log(scaleDeltaStep);
	}

	void Update() {

	}

	void FixedUpdate() {
		Vector3 newPos = transform.position;
		newPos.x += deltaX / moveTime * Time.deltaTime;
		newPos.y -= speed * Time.deltaTime;
		//		newPos.z -= speed * 0.8f * Time.deltaTime;
		transform.position = newPos;
		Vector3 scale = transform.localScale;
//		Debug.Log(transform.localScale);
		scale.x += scaleDeltaStep * Time.deltaTime;
		scale.y += scaleDeltaStep * Time.deltaTime;
		transform.localScale = scale;

	}
}
