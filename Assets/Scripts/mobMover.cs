using UnityEngine;
using System.Collections;

public class mobMover : MonoBehaviour {
	public float speed;
	public float deltaX;

	private float moveTime;
	void Start() {
		moveTime = 1.3f / speed; // 1.3: total range of deltaY
		Debug.Log(deltaX/moveTime);
	}

	void Update() {

	}

	void FixedUpdate() {
		Vector3 newPos = transform.position;
		newPos.x += deltaX / moveTime * Time.deltaTime;
		newPos.y -= speed * Time.deltaTime;
		//		newPos.z -= speed * 0.8f * Time.deltaTime;
		transform.position = newPos;
	}
}
