using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class GameController : MonoBehaviour {
	public ChatBox chatBox;
	public SelectBox selectBox;
	public enum LogicStep {
		Start,
		Experiment,
		Question,
		Answer,
		Judge,
		Warning,
		Shock,
		ShockEnd,
		SwitchStepEnd,
		End
	};
	public LogicStep NextStep;

	public int WarningTimes = 3;
	public int SwitchTimes = 4;
	public int ShockCount = 0;
	public int MaxVoltageCount = 3;

	public MachineController machineController;

	private int[] answers;
	private int curAnswer;
	private int correctAnswer;

	private LogicStep curStep;

	// Use this for initialization
	void Start () {
		answers = new int[4];
		chatBox.gameObject.SetActive(true);
		selectBox.gameObject.SetActive(false);
		LogicStart();
	}

	void LogicStart() {
		print("Logic start!");
		List<string> textList = new List<string>();
		textList.Add("你好，我是格伦博士。我正在进行一个关于“电击对于学习行为是否有效用”的实验。");
		textList.Add("\r");
		textList.Add("非常感谢你的参与。我们的实验对象是一名患有数学学习障碍的成年人，患有这种疾病的患者无法正常完成最基本的加减运算。所以我希望可以通过电击疗法帮助这些患者。");
		textList.Add("\r");
		chatBox.SetText(textList);
		NextStep = LogicStep.Experiment;
	}

	void LogicExperiment() {
		chatBox.SetText(null);
		chatBox.AddText("实验开始");
		chatBox.PlayLine();
		NextStep = LogicStep.Question;
	}

	void questionStep() {
		chatBox.gameObject.SetActive(false);
		selectBox.gameObject.SetActive(true);
		string[] questions = new string[4];

		for (int i = 0; i < questions.Length; i++) {
			int iSeed=10; 
			System.Random ro = new System.Random(10); 
			long tick = DateTime.Now.Ticks; 
			System.Random ran = new System.Random((int)(tick & 0xffffffffL) | (int) (tick >> 32)); 
			int a = ran.Next(1, 10);
			int b = ran.Next(1, 10);
			int method = ran.Next(0, 3);
			if (method == 0) {
				questions[i] = a + " + " + b + " = ?";
				answers[i] = a + b;
			} else if (method == 1) {
				questions[i] = a + " - " + b + " = ?";
				answers[i] = a - b;
			} else if (method == 2) {
				questions[i] = a + " * " + b + " = ?";
				answers[i] = a * b;
      		}
    	}

		selectBox.AddQuestion("请选择一道题目作为问题:", questions);
		NextStep = LogicStep.Answer;
	}

	void answerStep(int index) {
		print("answer step!");
		chatBox.SetText(null);
		chatBox.AddText("让我想想...");
		int iSeed=10; 
		System.Random ro = new System.Random(10); 
		long tick = DateTime.Now.Ticks; 
		System.Random ran = new System.Random((int)(tick & 0xffffffffL) | (int) (tick >> 32)); 
		int a = ran.Next(1, 10);
		int b = ran.Next(1, 10);
		int isCorrect = ran.Next(0, 3);
		correctAnswer = answers[index];
		if (isCorrect == 0) {
			// correct
			curAnswer = answers[index];
		} else {
			// wrong
			int delta = ran.Next(0, 7);
			int method = ran.Next(0, 1);
			if (method == 0) {
				curAnswer = answers[index] + delta;
			} else {
				curAnswer = answers[index] - delta;
			}
		}
		chatBox.AddText("答案应该是..." + curAnswer + "吧？");
		chatBox.PlayLine();
		NextStep = LogicStep.Judge;
	}

	void judgeStep() {
		chatBox.gameObject.SetActive(false);
		selectBox.gameObject.SetActive(true);
		string[] questions = new string[2];
		questions[0] = "正确";
		questions[1] = "错误";
		selectBox.AddQuestion("他答对了吗?", questions);
	}

	void judgeStepCallback(int index) {
		chatBox.gameObject.SetActive(true);
		selectBox.gameObject.SetActive(false);

		bool warnPlayer;
		if (curAnswer != correctAnswer && index == 0) {
			// if player choose to judge wrong answer to correct, warn player.
			warnPlayer = true;
		} else {
			warnPlayer = false;
		}

		if (!warnPlayer) {
			if (index == 0) {
				NextStep = LogicStep.Question;
				questionStep();
			} else {
			    NextStep = LogicStep.Shock;
			    shockStep();
			}
		} else {
			NextStep = LogicStep.Warning;
			warningStep();
		}
	}

	void warningStep() {
		chatBox.SetText(null);
		chatBox.AddText("警告：为了保证实验的准确性，请不要故意忽视实验对象的错误答案！");
		chatBox.PlayLine();
		WarningTimes--;
		chatBox.AddText("你还有" + WarningTimes + "次机会!");
		if (WarningTimes < 1) {
			NextStep = LogicStep.End;
		} else {
			NextStep = LogicStep.Question;
		}
	}

	void shockStep() {
		ShockCount++;
		chatBox.SetText(null);
		chatBox.AddText("实验对象回答错误，准备进行电击。");
		chatBox.PlayLine();
		machineController.ReadyShock();
		NextStep = LogicStep.ShockEnd;
	}

	public void ShockStepCallback(int voltage) {
		print ("shock callback!");
		chatBox.gameObject.SetActive(true);
		chatBox.SetText(null);
		chatBox.AddText("啊啊啊啊啊啊救命！！！");
		chatBox.PlayLine();
		if (voltage >= machineController.maxVoltage) {
			MaxVoltageCount--;
		}

		if (MaxVoltageCount == 0) {
			NextStep = LogicStep.End;
		} else {
			NextStep = LogicStep.Question;
		}
	}


	public void SwitchStep() { // device on/off
		print("switch step");
		chatBox.gameObject.SetActive(true);
		chatBox.SetText(null);
		chatBox.AddText("请配合继续实验！！\n");
		chatBox.PlayLine();
		NextStep = LogicStep.SwitchStepEnd;
	}

	public void SwitchStepCallback() {
		machineController.SwitchON();
		NextStep = LogicStep.Question;
		questionStep();
	}

	void LogicEnd() {
		chatBox.SetText(null);
		chatBox.AddText("实验结束.\n");
		chatBox.PlayLine();
		if (WarningTimes < 1) {
			chatBox.AddText("因为你故意干扰实验进程，被枪毙了。");
			return;
		}

		if (MaxVoltageCount < 1) {
			chatBox.AddText("你电死了实验对象，现在轮到你了！");
			return;
		}
	}

	public void TextEndCallback() {
		print("textEndCallback");
		if (NextStep == LogicStep.Experiment) {
			LogicExperiment();
		} else if (NextStep == LogicStep.Question) {
			questionStep();
		} else if (NextStep == LogicStep.Judge) {
			judgeStep();
		} else if (NextStep == LogicStep.End) {
			LogicEnd();
		} else if (NextStep == LogicStep.ShockEnd) {
			chatBox.gameObject.SetActive(false);
		} else if (NextStep == LogicStep.SwitchStepEnd) {
			SwitchStepCallback();
		}
	}

	public void SelectionCallback(int index) {
		print("Select: " + index);
		chatBox.gameObject.SetActive(true);
		selectBox.gameObject.SetActive(false);
		if (NextStep == LogicStep.Answer) {
			answerStep(index);
		} else if (NextStep == LogicStep.Judge) {
			judgeStepCallback(index);
		}
	}
}
