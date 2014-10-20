using UnityEngine;
using System.Collections;

public class startSceneController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		singleMusicInstance.Instance.Play(0);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("space")) {
			Application.LoadLevel("testScene");
		}
	}
}
