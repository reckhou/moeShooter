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

	// Use this for initialization
	void Start () {
		if (TextSpeed != 0) {
			textDeltaTime = 1.0f/TextSpeed;
		} else {
			textDeltaTime = 1;
		}

		textBuffer = new List<char>();
	}

	public void SetText(string text) {
		TextBox.text = "";
		string[] textArray = text.Split(new char[1]{'\n'});
		textList = new List<string>(textArray);
	}

	public void PlayLine() {
		if (textList.Count < 1) {
			return;
		}

		if (textList[0] == "\r") {
			clearText();
			textList.RemoveAt(0);
		}

		List<char> tmpList = textList[0].ToList();
		textBuffer.AddRange(tmpList);
		textBuffer.Add('\n');
		textList.RemoveAt(0);
	}

	public void Update() {
		if (Time.time - lastUpdateTime > textDeltaTime) {
			updateText();
			lastUpdateTime = Time.time;
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
