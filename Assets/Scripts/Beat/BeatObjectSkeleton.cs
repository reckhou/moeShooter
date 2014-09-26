using UnityEngine;
using System.Collections;
using Spine;

public class BeatObjectSkeleton : BeatObjectDefault {

	public override void Init(float barTime) {
		base.Init(barTime);
	}
	
	public override void Beat() {
		base.Beat();
		SkeletonAnimation sk = this.GetComponent<SkeletonAnimation>();
		Spine.AnimationState state = sk.state;
		if (curBeat % 4 == 0) {
			state.SetAnimation(0, "idle", true);
//			sk.loop = true;
		} else if (curBeat % 4 == 2) {
			state.SetAnimation(0, "skill2", false);
//			sk.loop = true;
		}
	}
	
	public override void updateSpeed() {
		/* All animation is 1 sec length by default */
		float animationTime = BarTime / AnimationPerBar;
		SkeletonAnimation sk = this.GetComponent<SkeletonAnimation>();
		Debug.Log(BarTime + ","+ AnimationPerBar+","+ animationTime);
		if (BarTime == 0) {
			sk.timeScale = 0;
		} else {
//			sk.timeScale = 1.82f;
			sk.timeScale = 1.0f / animationTime / (50.0f/60.0f);
		}
	}
}
