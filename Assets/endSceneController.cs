using UnityEngine;
using System.Collections;

public class endSceneController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("space")) {
			Application.LoadLevel("startScene");
		}
	}
}
