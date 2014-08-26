using UnityEngine;
using System.Collections;

public class moonMover : MonoBehaviour {
	public float endPosY;
	public float speed;
	public float startPosY;
	public float moonLight, moonLightBack, envLight;
	public float startWait;
	public GameObject sceneFar;
	public GameObject char_0;

	private bool startSpawn = false;
	private float spawnTime;

	// Use this for initialization
	void Start () {
		Vector3 pos = transform.position;
		pos.y = startPosY;
		transform.position = pos;
		spawnTime = (endPosY - startPosY) / speed;
		Debug.Log(spawnTime);
		StartCoroutine (SpawnMoon());
	}

	IEnumerator SpawnMoon ()
	{
		yield return new WaitForSeconds (startWait);
		 
		startSpawn = true;

		yield return null;
	}
	
	// Update is called once per frame
	void Update () {
		if (startSpawn)
		{
			if (transform.position.y >= endPosY) {
				startSpawn = false;
			} else {
				Vector3 newPos = transform.position;
				newPos.y += speed * Time.deltaTime;
				transform.position = newPos;
				float intensityPerStep = moonLightBack / spawnTime;
				float opcacityPerStep = 255.0f / spawnTime;
				float changeRate = moonLight / moonLightBack;
				float changeRate2 = moonLightBack / envLight;
				GameObject ml = GameObject.FindGameObjectWithTag("moonlight");
				GameObject mlb = GameObject.FindGameObjectWithTag("moonlightback");
				GameObject envl = GameObject.FindGameObjectWithTag("envLight");
				if (ml.light.intensity < moonLight) {
					ml.light.intensity += intensityPerStep * Time.deltaTime * changeRate;
				}
				if (mlb.light.intensity < moonLightBack) {
					mlb.light.intensity += intensityPerStep * Time.deltaTime;
				}
				if (envl.light.intensity < envLight) {
					envl.light.intensity += intensityPerStep * Time.deltaTime / changeRate2 ;
				}
				SpriteRenderer sp = sceneFar.GetComponent<SpriteRenderer>();
				Color newColor =  sp.color;
				newColor.a += (1.0f/255.0f)*opcacityPerStep * Time.deltaTime;
				Debug.Log(newColor.a);
				sp.color = newColor;

				sp = char_0.GetComponent<SpriteRenderer>();
				sp.color = newColor;
			}
		}
	}
}
