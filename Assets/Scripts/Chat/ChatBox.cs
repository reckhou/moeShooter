using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ChatBox : MonoBehaviour {
	public Text TextBox;
	public float TextSpeed;
	private float textDeltaTime;
	private List<string> textList;
	private List<char> textBuffer;
	private float lastUpdateTime;
	private GameController gameController;

	// Use this for initialization
	void Start () {
		if (TextSpeed != 0) {
			textDeltaTime = 1.0f/TextSpeed;
		} else {
			textDeltaTime = 1;
		}

		textBuffer = new List<char>();

		gameController = GameObject.Find("GameController").GetComponent<GameController>();
		if (gameController == null) {
			print ("failed to get game controller!");
		}

		textList = new List<string>();
	}

	public void SetText(List<string> list) {
		clearText();
		textList.Clear();
		if (list == null) {
			return;
		}

		textList.AddRange(list);
		for (int i = 0; i < textList.Count; i++) {
			textList[i] += "\n";
		}
		PlayLine();
	}

	public void AddText(string text) {
		textList.Add(text + "\n");
	}

	public void PlayLine() {
		print(textList.Count);
		if (textList.Count < 1) {
			textEnd();
			return;
		}

		if (textList[0] == "\r\n") {
			clearText();
			textList.RemoveAt(0);
		}

		if (textList.Count < 1) {
			textEnd();
			return;
		}
		print (textList[0]);
		List<char> tmpList = textList[0].ToList();
		textBuffer.AddRange(tmpList);
		textBuffer.Add('\n');
		textList.RemoveAt(0);
	}

	void textEnd() {
		gameController.TextEndCallback();
	}

	public void Update() {
		if (Time.time - lastUpdateTime > textDeltaTime) {
			updateText();
			lastUpdateTime = Time.time;
		}

		if (Input.GetKeyUp("space")) {
			PlayLine();
		}
	}

	private void updateText() {
		if (textBuffer.Count < 1) {
			return;
		}
		string curText = TextBox.text;
		curText += textBuffer[0];
		TextBox.text = curText;
		textBuffer.RemoveAt(0);
	}

	private void clearText() {
		textBuffer.Clear();
		TextBox.text = "";
	}
}
