using UnityEngine;
using System.Collections;

public class gameController : MonoBehaviour {
	public GameObject mob;
	public Vector3 spawnPoint;
	public float endXRange;
	public int mobCount;
	public float spawnWait;
	public float startWait;
	public float waveWait;

	private bool lampOn;
	
	void Start ()
	{
		lampOn = false;
		StartCoroutine (SpawnWaves ());
	}
	
	IEnumerator SpawnWaves ()
	{
		yield return new WaitForSeconds (startWait);
		while (!lampOn) {
			GameObject lmpl = GameObject.FindGameObjectWithTag("lampLight");
			if (lmpl != null) {
			  lmpl.light.intensity = 8;
			}
			lampOn = true;
			yield return new WaitForSeconds(1);
		}
		while (true)
		{
			for (int i = 0; i < mobCount; i++)
			{
				Vector3 spawnPosition = spawnPoint; //new Vector3(, spawnValues.y, spawnValues.z);
				Quaternion spawnRotation = Quaternion.identity;
				Instantiate (mob, spawnPoint, spawnRotation);
				mob.GetComponent<mobMover>().endPosX = Random.Range (-endXRange, endXRange);
				yield return new WaitForSeconds (spawnWait);
			}
			yield return new WaitForSeconds (waveWait);
		}
	}
}
