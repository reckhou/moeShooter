﻿using UnityEngine;
using System.Collections;

public class gameController : MonoBehaviour {
	public GameObject mob;
	public Vector3 spawnPoint;
	public float spawnXRange;
	public float endXRange;
	public int mobCount;
	public float spawnWait;
	public float startWait;
	public float waveWait;

	private bool lampOn;
	private int curOrder = 0;

	void Awake() {
		Application.targetFrameRate = 60;
	}

	void Start ()
	{
		lampOn = false;
		StartCoroutine (SpawnWaves ());
	}
	
	IEnumerator SpawnWaves ()
	{
		yield return new WaitForSeconds (startWait);
		while (!lampOn) {
			GameObject[] lights = GameObject.FindGameObjectsWithTag("lampLight");
			foreach (GameObject lmpl in lights) {
			  lmpl.light.intensity = 5;
			}
			lampOn = true;
			yield return new WaitForSeconds(1);
		}
		while (true)
		{
			for (int i = 0; i < mobCount; i++)
			{
				Quaternion spawnRotation = Quaternion.identity;
				spawnPoint.x = Random.Range(-spawnXRange, spawnXRange);
//				spawnPoint.x = 0.5f;
				GameObject newMob = Instantiate (mob, spawnPoint, spawnRotation)  as GameObject;
				float endX = Random.Range (-endXRange, endXRange);
//				Debug.Log(spawnPoint.x+"_"+endX);
				newMob.GetComponent<mobMover>().deltaX = endX - spawnPoint.x;
				newMob.renderer.sortingOrder = curOrder;
				curOrder--;
				yield return new WaitForSeconds (spawnWait);
			}
			yield return new WaitForSeconds (waveWait);
		}
	}
}
