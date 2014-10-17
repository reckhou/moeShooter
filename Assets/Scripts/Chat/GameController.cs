using UnityEngine;
using UnityEngine.UI;
using System.Collections;

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
		End
	};
	public LogicStep NextStep;

	public int WarningTimes = 3;
	public int SwitchTimes = 4;
	public int ShockCount = 0;

	// Use this for initialization
	void Start () {
		chatBox.gameObject.SetActive(true);
		selectBox.gameObject.SetActive(false);
		LogicStart();
	}

	void LogicStart() {
		print("Logic start!");
		chatBox.SetText("你好，我是格伦博士。我正在进行一个关于“电击对于学习行为是否有效用”的实验。\n\r\n非常感谢你的参与。我们的实验对象是一名患有数学学习障碍的成年人，患有这种疾病的患者无法正常完成最基本的加减运算。所以我希望可以通过电击疗法帮助这些患者。\n\r");
		chatBox.AddText("Preparing....\n");
		NextStep = LogicStep.Experiment;
	}

	void LogicExperiment() {
		chatBox.SetText("Start experiment\n");
		questionStep();
	}

	void questionStep() {
		print("question step!");
		chatBox.gameObject.SetActive(false);
		selectBox.gameObject.SetActive(true);
		string[] questions = new string[4];
		questions[0] = "123";
		questions[1] = "456";
		questions[2] = "789";
		questions[3] = "012345";
		selectBox.AddQuestion("Test question:", questions);
		NextStep = LogicStep.Answer;
	}

	void answerStep() {
		print("answer step!");
		chatBox.SetText("answer is 123");
		NextStep = LogicStep.Judge;
	}

	void judgeStep() {
		chatBox.gameObject.SetActive(false);
		selectBox.gameObject.SetActive(true);
		string[] questions = new string[2];
		questions[0] = "YES";
		questions[1] = "NO";
		selectBox.AddQuestion("is that correct?", questions);
	}

	void judgeStepCallback() {
		chatBox.gameObject.SetActive(true);
		selectBox.gameObject.SetActive(false);

		// todo: decide player answer good or not;
		bool isGood = true;
		if (isGood) {
			NextStep = LogicStep.Shock;
			shockStep();
		} else {
			NextStep = LogicStep.Warning;
			warningStep();
		}
	}

	void warningStep() {
		chatBox.SetText("warning!!!");
		NextStep = LogicStep.Question;
	}

	void shockStep() {
		chatBox.SetText("shock!!!");
		NextStep = LogicStep.Question;
	}

	public void SwitchStep() { // device on/off
	}

	void LogicEnd() {
		chatBox.SetText("experiment end!\n");
	}

	public void TextEndCallback() {
		if (NextStep == LogicStep.Experiment) {
			LogicExperiment();
		} else if (NextStep == LogicStep.Question) {
			questionStep();
		} else if (NextStep == LogicStep.Judge) {
			judgeStep();
		} else if (NextStep == LogicStep.End) {
			LogicEnd();
		}
	}

	public void SelectionCallback(int index) {
		print("Select: " + index);
		chatBox.gameObject.SetActive(true);
		selectBox.gameObject.SetActive(false);
		if (NextStep == LogicStep.Answer) {
			answerStep();
		} else if (NextStep == LogicStep.Judge) {
			judgeStepCallback();
		}
	}
}
