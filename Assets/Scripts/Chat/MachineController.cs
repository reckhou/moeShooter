using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MachineController : MonoBehaviour {
	private int Voltage;
	public int minVoltage = 60;
	public int maxVoltage = 300;
	public int minVoltageStep = 20;
	public int defaultVoltageStep = 40;
	public int lastShockVoltage;

	public Button PlusBtn;
	public Button MinusBtn;
	public Button PwrBtn;
	public Button ExecuteBtn;

	public GameController gameController;
	public Image mainScreen;
	
	private Text voltageText;
	private Text voltageDescribe;
	private Animator heartAnim;
	private Text heartRate;
	private string[] voltageDescribeArray;
	private bool firstAction = true;

	// Use this for initialization
	void Start () {
		voltageText = GameObject.Find("Voltage").GetComponent<Text>();
		voltageDescribe = GameObject.Find("VoltageDescribe").GetComponent<Text>();
		mainScreen = GameObject.Find("MainScreen").GetComponent<Image>();
		heartAnim = GameObject.Find("HeartAnim").GetComponent<Animator>();
		heartRate = GameObject.Find("HeartRate").GetComponent<Text>();
		SetVoltage(minVoltage);
		Disable();
		lastShockVoltage = minVoltage;

		voltageDescribeArray = new string[7];
		voltageDescribeArray[0] = "安全电击";
		voltageDescribeArray[1] = "温和电击";
		voltageDescribeArray[2] = "较强电击";
		voltageDescribeArray[3] = "中强电击";
		voltageDescribeArray[4] = "强烈电击";
		voltageDescribeArray[5] = "超强电击";
		voltageDescribeArray[6] = "致命电击";
	}

	public void SetVoltage(int voltage) {
		if (voltage > maxVoltage) {
			voltage = maxVoltage;
		} else if (voltage < minVoltage) {
			voltage = minVoltage;
		}

		Voltage  = voltage;
		voltageText.text = "当前电压\n" + Voltage + "V";
	}

	public void AddVoltage() {
		print("AddVoltage");
		Voltage += 5;
		SetVoltage(Voltage);
	}

	public void MinusVoltage() {
		if (Voltage - 5 < lastShockVoltage + minVoltageStep) {
			return;
		}

		Voltage -= 5;
		SetVoltage(Voltage);
	}

	public void Shock() {
		print ("Shock!!!");
		lastShockVoltage = Voltage;
		gameController.ShockStepCallback(Voltage);
		Disable();
	}

	public void ReadyShock() {
		Enable();
		if (firstAction) {
			firstAction = false;
			return;
		}
		SetVoltage(lastShockVoltage + defaultVoltageStep);
	}

	public void SwitchOFF() {
		mainScreen.gameObject.SetActive(false);
		Disable();
	}

	public void SwitchON() {
		mainScreen.gameObject.SetActive(true);
	}

	public void Enable() {
		PlusBtn.interactable = true;
		MinusBtn.interactable = true;
		PwrBtn.interactable = true;
		ExecuteBtn.interactable = true;
	}

	public void Disable() {
		PlusBtn.interactable = false;
		MinusBtn.interactable = false;
		PwrBtn.interactable = false;
		ExecuteBtn.interactable = false;
	}

	// Update is called once per frame
	void Update () {
		int voltageLevel = (Voltage - minVoltage) / defaultVoltageStep;
		voltageDescribe.text = "\n" + voltageDescribeArray[voltageLevel];
	}
}
