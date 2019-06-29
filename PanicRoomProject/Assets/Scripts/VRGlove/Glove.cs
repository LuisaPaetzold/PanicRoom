using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;

public class Glove : MonoBehaviour {
	public short portNumber;
	public int baudRate;

	private GloveFinger[] fingers;

	private SerialPort arduinoPort;

	void Start()
	{
		arduinoPort = new SerialPort("COM" + portNumber, baudRate);
		arduinoPort.Open();
        
		fingers = gameObject.GetComponentsInChildren<GloveFinger> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void stopFinger(int index){
		writeToArduino(index.ToString());
	}

	void writeToArduino(string message)
	{
		if (arduinoPort.IsOpen == false)
		{
			arduinoPort.Open();
			arduinoPort.Write(message);
		}
		else
		{
			arduinoPort.Write(message);
		}
	}
}
