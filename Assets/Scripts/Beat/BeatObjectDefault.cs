using UnityEngine;
using System.Collections;

public class BeatObjectDefault : BeatObject {
	public float BarTime { get; set; }
	public float AnimationPerBar = 2;


	public override void Init() {
		base.Init();
		updateSpeed();
  }

	public override void Beat() {
		base.Beat();
		if (curBeat == 0) {
			updateSpeed();
		}
  }

	private void updateSpeed() {
		/* All animation is 1 sec length by default */
		float animationTime = BarTime / AnimationPerBar;
		Animator anim = GetComponent<Animator>();
		anim.speed = 1.0f / animationTime;
//		print (1.0f / animationTime);
  }
}
