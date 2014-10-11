using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MachineController : MonoBehaviour {
	private int Voltage;
	const int minVoltage = 75;
	const int maxVoltage = 450;
	private Text voltageText;

	// Use this for initialization
	void Start () {
		voltageText = GameObject.Find("Voltage").GetComponent<Text>();
		SetVoltage(minVoltage);
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
		SetVoltage(Voltage+5);
	}

	public void MinusVoltage() {
		SetVoltage(Voltage-5);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
