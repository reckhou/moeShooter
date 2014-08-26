using UnityEngine;
using System.Collections;

public class moonMover : MonoBehaviour {
	public float endPosY;
	public float speed;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position.y < endPosY) {
			Vector3 newPos = transform.position;
			newPos.y += speed * Time.deltaTime;
			transform.position = newPos;
		}
	}
}
