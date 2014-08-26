using UnityEngine;
using System.Collections;

public class removeMob : MonoBehaviour {
	// Update is called once per frame
	void OnTriggerEnter2D(Collider2D other) {
		Destroy(other.gameObject);
	}
}
