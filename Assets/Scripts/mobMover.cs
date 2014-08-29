using UnityEngine;
using System.Collections;

public class mobMover : MonoBehaviour {
	public float speed;
	public float endPosX;

	private float moveTime;
	void Start() {
		moveTime = 7 / speed; // 3.8: total range of deltaY
//		Debug.Log(endPosX);
	}

	void Update() {
		Vector3 newPos = transform.position;
		newPos.x += endPosX / moveTime * Time.deltaTime;
		newPos.y -= speed * Time.deltaTime;
		newPos.z -= speed * 0.8f * Time.deltaTime;
		transform.position = newPos;
	}

	void FixedUpdate() {

	}
}
