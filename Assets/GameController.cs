using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour {
	public ChatBox chatBox;
	// Use this for initialization
	void Start () {
		chatBox.SetText("三十多年来，D&D作为定义游戏流派的规则，制定了奇幻类角色扮演游戏的统一标准。\nD&D是一个充满奇幻经历的世界，这里有富有传奇色彩的英雄，致命的怪物，以及复杂多变的设定，让玩家有身临其境的真实体验。\n\r\n玩家们创造了无数英雄角色：或是彪悍勇猛的战士，或是神出鬼没的盗贼，又或是强大的法师……他们领着人们不断探索冒险，合力击败怪物并挑战更加强劲的敌人，继而在力量、荣誉与成就中逐渐成长起来。");
	}
}
