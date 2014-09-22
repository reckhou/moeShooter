using UnityEngine;
using System.Collections;

public class BeatObject : MonoBehaviour {
	protected int curBeat { get; set; }
	public int beatStatCnt { get; set; }
	public Vector3 originScale { get; set; }

	void Start() {
		Init();
	}

	public virtual void Init() {
		beatStatCnt = GlobalConfig.BeatStatCnt;
		originScale = transform.localScale;
	}

	public virtual void Beat() {
		curBeat++;
		if (curBeat >= beatStatCnt) {
			curBeat = 0;
		}
	}
}
