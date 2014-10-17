using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MachineController : MonoBehaviour {
	private int Voltage;
	public int minVoltage = 60;
	public int maxVoltage = 300;
	public int minVoltageStep = 20;
	public int defaultVoltageStep = 30;
	public int lastShockVoltage;

	public GameController gameController;
	
	private Text voltageText;

	// Use this for initialization
	void Start () {
		voltageText = GameObject.Find("Voltage").GetComponent<Text>();
		SetVoltage(minVoltage);
		lastShockVoltage = minVoltage;
	}

	public void SetVoltage(int voltage) {
		if (voltage > maxVoltage) {
			voltage = maxVoltage;
		} else if (voltage < minVoltage) {
			voltage = minVoltage;
		}

		Voltage  = voltage;
		voltageText.text = "当前电压\n\n" + Voltage + "V";
	}

	public void AddVoltage() {
		print("AddVoltage");
		Voltage++;
		SetVoltage(Voltage);
	}

	public void MinusVoltage() {
		if (Voltage - 1 < lastShockVoltage + minVoltageStep) {
			return;
		}

		Voltage--;
		SetVoltage(Voltage);
	}

	public void Shock() {
		print ("Shock!!!");
		lastShockVoltage = Voltage;
		gameController.ShockStepCallback(Voltage);
	}

	public void ReadyShock() {
		SetVoltage(lastShockVoltage + defaultVoltageStep);
	}

	public void Warning() {
		// professor warning
	}

	public void StopWarning() {
	}

	public void SwitchOFF() {
	}

	public void SwitchON() {
	}

	// Update is called once per frame
	void Update () {
	
	}
}
