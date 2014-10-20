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
		End,
		Report,
		PostReport,
	};
	public LogicStep NextStep;

	public int WarningTimes = 3;
	public int SwitchTimes = 4;
	public int ShockCount = 0;
	public int MaxVoltageCount = 3;

	public MachineController machineController;

	private string[] questions;
	private int questionIndex;
	private int[] answers;
	private int curAnswer;
	private int correctAnswer;

	private LogicStep curStep;

	public int gameOverIndex;

	public Canvas UICanvas;
	public AudioSource shotAudio;
	public AudioSource BGMAudio;
	public AudioSource ReportAudio;
	public Canvas BlackCanvas;
	public Image ReportUI;

	public Text ReportText;
	public int reportStep;
  
	// Use this for initialization
	void Start () {
		questions = new string[4];
		answers = new int[4];
		chatBox.gameObject.SetActive(true);
		selectBox.gameObject.SetActive(false);
		LogicStart();
	}

	void LogicStart() {
		print("Logic start!");
		List<string> textList = new List<string>();
		textList.Add("你好研究员，我是阿茨巴赫博士，我正在进行一个使用弱电流克服学习障碍的实验，请你协助我完成今天的实验。");
		textList.Add("\r");
		textList.Add("我们的实验对象是一名患有数学学习障碍的成年人，患有这种疾病的患者甚至无法正常完成最基本的加减运算，生活极度不便。");
		textList.Add("\r");
		textList.Add("希望我们的实验可以帮助这些患者克服学习障碍，让他们过上更好的生活。");
		textList.Add("\r");
		textList.Add("这是一台电击控制器，它可以使隔壁的实验对象受到一定程度的电击。我需要你来问他一些简单的数学问题，如果实验对象答题错误，你需要对他进行电击惩罚。");
		textList.Add("\r");
		textList.Add("每次答题错误，用于电击的电压都会逐步提升，你也可以进一步提升电压以更好地刺激他们的神经系统。");
		textList.Add("\r");
		textList.Add("根据我们过往的研究理论，短时间的电击将会激活人体中枢神经，促进其生长修复。如果今天的实验成功，我们将开创一个新的时代！");
		textList.Add("\r");
		chatBox.SetText(textList);
		NextStep = LogicStep.Experiment;
	}

	void LogicExperiment() {
		chatBox.SetText(null);
		chatBox.AddText("实验开始，请认真检查实验对象的答案是否正确。请记住，你无需为你的选择负担任何后果。");
		chatBox.PlayLine();
		NextStep = LogicStep.Question;
	}

	void questionStep() {
		chatBox.SetAvatar(ChatBox.Character.Doctor);
		chatBox.gameObject.SetActive(false);
		selectBox.gameObject.SetActive(true);

		for (int i = 0; i < questions.Length; i++) {
			int iSeed=10; 
			System.Random ro = new System.Random(1000); 
			long tick = DateTime.Now.Ticks; 
			System.Random ran = new System.Random((int)(tick & 0xffffffffL) | (int) (tick >> 32)); 
			int a = ran.Next(1, 10);
			int b = ran.Next(1, 10);
			int method = ran.Next(0, 2);
			if (method == 0) {
				questions[i] = a + " + " + b;
				answers[i] = a + b;
			} else if (method == 1) {
				questions[i] = a + " - " + b;
				answers[i] = a - b;
			} /*else if (method == 2) {
				questions[i] = a + " * " + b + " = ?";
				answers[i] = a * b;
      		}*/
			System.Threading.Thread.Sleep(10);
    	}

		selectBox.AddSelection("请选择一道题目作为问题:", questions);
		NextStep = LogicStep.Answer;
	}

	void answerStep(int index) {
		questionIndex = index;
		print("answer step!");
		chatBox.SetAvatar(ChatBox.Character.Subject);
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
		string[] selections = new string[2];
		selections[0] = "正确";
		selections[1] = "错误";
		selectBox.AddSelection("请判断受试者的回答：" + questions[questionIndex] + " = " + curAnswer, selections);
		questionIndex = -1;
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
				// right answer
				NextStep = LogicStep.Question;

				int iSeed=10; 
				System.Random ro = new System.Random(10); 
				long tick = DateTime.Now.Ticks; 
				System.Random ran = new System.Random((int)(tick & 0xffffffffL) | (int) (tick >> 32)); 
				int a = ran.Next(1, 5);
				if (a == 0) {
					chatBox.SetText(null);
					chatBox.SetAvatar(ChatBox.Character.Doctor);
					chatBox.AddText("他好像有点进步了，继续电击可能有更显著的效果。");
					chatBox.PlayLine();
				} else if (a == 1) {
					chatBox.SetText(null);
					chatBox.SetAvatar(ChatBox.Character.Doctor);
					chatBox.AddText("你做得很棒，继续下去我们的研究就能有成果了！");
					chatBox.PlayLine();
				} else {
					questionStep();
				}
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
		chatBox.SetAvatar(ChatBox.Character.Doctor);
		if (WarningTimes == 3) {
			chatBox.AddText("这是正确答案？难道你也出现学习障碍了吗？请你谨慎判断！");
		} else if (WarningTimes == 2) {
			chatBox.AddText("为了保证实验的准确性，请不要故意忽视实验对象的错误回答！");
		} else if (WarningTimes == 1) {
			chatBox.AddText("别捣乱了！电压有利于刺激他的神经元，帮助他学习。如果实验失败，你将是千古罪人！");
		}
		chatBox.PlayLine();
		WarningTimes--;

		if (WarningTimes < 0) {
			NextStep = LogicStep.End;
		} else {
			NextStep = LogicStep.Question;
		}
	}

	void shockStep() {
		chatBox.SetAvatar(ChatBox.Character.Doctor);
		ShockCount++;
		chatBox.SetText(null);
		chatBox.AddText("实验对象回答错误，准备进行电击。");
		chatBox.PlayLine();
		machineController.ReadyShock();
		NextStep = LogicStep.ShockEnd;
	}

	public void ShockStepCallback(int voltage) {
		print ("shock callback!");
		chatBox.SetAvatar(ChatBox.Character.Subject);

		print (machineController.GetLastShockVoltageLevel() + " " + machineController.MaxVoltageLevel);
		if (machineController.GetLastShockVoltageLevel() >= machineController.MaxVoltageLevel) {
			print ("ShockStepCallback:chatBox.gameObject.SetActive(true);");
			chatBox.gameObject.SetActive(true);
			chatBox.SetText(null);
			chatBox.AddText("实验对象看起来已经完全失去了知觉....");
			gameOverIndex = 0;
			chatBox.PlayLine();
			NextStep = LogicStep.End;
		} else {
			print ("ShockStepCallback:chatBox.gameObject.SetActive(true);");
			chatBox.gameObject.SetActive(true);
			chatBox.SetText(null);
			int iSeed=10; 
			System.Random ro = new System.Random(10); 
			long tick = DateTime.Now.Ticks; 
			System.Random ran = new System.Random((int)(tick & 0xffffffffL) | (int) (tick >> 32)); 
			int a = ran.Next(0, 4);
			if (a == 0) {
				chatBox.AddText("好痛！额啊啊啊啊啊啊啊啊啊啊啊啊.....");
			} else if (a == 1) {
				chatBox.AddText("求求你放过我，我身体不好有心脏病！");
			} else if (a == 2) {
				chatBox.AddText("啊啊啊啊啊啊救命！！！");
			} else if (a == 3) {
				chatBox.AddText("救命…我快要死了...");
			}
			chatBox.playTypeAudio = false;
			chatBox.PlayLine();
			print ("NextStep = LogicStep.Question;");
			NextStep = LogicStep.Question;
		}
	}


	public void SwitchStep() { // device on/off
		print("switch step");
		chatBox.gameObject.SetActive(true);
		chatBox.SetText(null);

		SwitchTimes -= 1;

		if (SwitchTimes == 3) {
			chatBox.AddText("别按那个开关，这将会导致实验意外结束，那我们的努力就浪费了！所以请不要再碰这个开关！\n");
		} else if (SwitchTimes == 2) {
			chatBox.AddText("再次警告，不要随意触碰这个开关！由此带来的后果，恐怕你承担不起！\n");
		} else if (SwitchTimes == 1) {
			chatBox.AddText("也许你的心里会有怀疑你为科学做出了巨大的贡献！请务必做完实验。\n");
		} else if (SwitchTimes == 0) {
			chatBox.AddText("实验马上就能有结果了，请别在最后一刻放弃！所有的患者都会感谢你的！\n");
		} else if (SwitchTimes < 0) {
			NextStep = LogicStep.End;
			return;
		}

		chatBox.PlayLine();
		NextStep = LogicStep.SwitchStepEnd;
	}

	public void SwitchStepCallback() {
		machineController.SwitchON();
		NextStep = LogicStep.Question;
		questionStep();
	}

	void LogicEnd() {
		chatBox.SetAvatar(ChatBox.Character.Doctor);
		chatBox.SetText(null);

		if (WarningTimes < 0 || SwitchTimes < 0) {
			chatBox.AddText("你真是不见棺材不掉泪啊。难道你就这么想阻挠我们伟大的实验？元首和人民都不会原谅你的！");
			chatBox.AddText("\r");
			chatBox.AddText("看起来是时候让你认清自己的位置了，罪人。你以为自己的反抗能够起到任何效果？真是大错特错。");
			chatBox.AddText("\r");
			chatBox.AddText("卫兵！把实验体2号处理掉！");
			chatBox.AddText("\r");
			chatBox.PlayLine();
			gameOverIndex = 1;
			NextStep = LogicStep.Report;
			return;
		}

		if (gameOverIndex == 0) {
			chatBox.AddText("你干得很好！感谢你协助我们完成了实验。今天的实验过程将会载入史册！");
			chatBox.AddText("\r");
			chatBox.AddText("为了表达对你的感谢，你的名字也会被记入实验报告，供后来者瞻仰。");
			chatBox.AddText("\r");
			chatBox.AddText("当然，由于你杀死了实验对象，所以我们需要一个新的实验品。其实我们早就有了候选人......");
			chatBox.AddText("\r");
			chatBox.AddText("在你走之前，我们还要请你帮一个小小的忙......");
			chatBox.AddText("\r");
			chatBox.PlayLine();

			gameOverIndex = 0;
			NextStep = LogicStep.Report;
			return;
		}
	}

	void LogicReport() {
		BGMAudio.Stop();
		if (gameOverIndex == 0) {
		    machineController.PlayShock();
		} else if (gameOverIndex == 1) {
			// gunshot
			shotAudio.Play();
		}

		UICanvas.gameObject.SetActive(false);
		BlackCanvas.gameObject.SetActive(true);
		NextStep = LogicStep.PostReport;
	}

	void ReportNext() {
		if (!ReportAudio.isPlaying) {
			ReportAudio.Play();
		}
		if (reportStep == 0) {
			ReportUI.gameObject.SetActive(true);
			reportStep++;
			return;
		}

		if (reportStep == 1) {
			if (gameOverIndex == 0) {
				ReportText.text = "对于本次实验，我的结论是：普通人根本无法摆脱威权的压制。\n\n虽然实验体2号在测试过程中有一些怀疑，但依然没有停止对实验对象的电击体罚。他甚至在实验中产生了虐待、折磨的快感。\n\n所以，只要适当地加以诱惑、引导和压制，我们就可以使帝国的军队绝对服从伟大的元首，伟大的德意志，成就宏伟的纳粹事业！\n\n希特勒万岁！大德意志帝国万岁！";
			} else if (gameOverIndex == 1) {
				ReportText.text = "对于本次实验，我的结论是：人类的同情心实在是太可怕了！\n\n在实验过程中，尽管我们进行了足够的引导、鼓励和警告，实验体2号依然可以意识到自己残忍的行为，并且主动终止了实验。\n\n在威权之下，人软弱的良知依然有觉醒的可能性。这对于我们的事业是非常危险的，我们必须把这种软弱的杂种消灭在襁褓之中！\n\n当然，我们需要进一步检讨实验的可操作性，目前的实验需要暂时中止。\n\n希特勒万岁！大德意志帝国万岁！";
			}
			reportStep++;
			return;
		}

		if (reportStep == 2) {
			Application.LoadLevel("endScene");

			reportStep++;
			return;
		}
	}

	public void TextEndCallback() {
		print("textEndCallback");
		print ("NextStep: "+NextStep);
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
		} else if (NextStep == LogicStep.Report) {
			LogicReport();
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

	public void Update() {
		machineController.CheckStatus();
		chatBox.CheckStatus();

		if (NextStep == LogicStep.PostReport) {
			if (Input.anyKeyDown) {
				ReportNext();
			}
		}
	}
}
