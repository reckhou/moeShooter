using UnityEngine;
using System.Collections;

public class gameController : MonoBehaviour {
	public GameObject mob;
	public Vector3 spawnValues;
	public int mobCount;
	public float spawnWait;
	public float startWait;
	public float waveWait;
	
	void Start ()
	{
		//StartCoroutine (SpawnWaves ());
	}
	
	IEnumerator SpawnWaves ()
	{
		yield return new WaitForSeconds (startWait);
		while (true)
		{
			for (int i = 0; i < mobCount; i++)
			{
				Vector3 spawnPosition = new Vector3 (Random.Range (-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
				Quaternion spawnRotation = Quaternion.identity;
				Instantiate (mob, spawnPosition, spawnRotation);
				yield return new WaitForSeconds (spawnWait);
			}
			yield return new WaitForSeconds (waveWait);
		}
	}
}
