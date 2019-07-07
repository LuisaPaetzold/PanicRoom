using UnityEngine;
using System.Collections;
using System.IO.Ports;
using System.Threading;

public class ActivateVibModules : MonoBehaviour
{
	//if serial & port cannot be found go to edit -> project settings -> player and change api compability level to .NET 2.0
	//Vibro 0 auf Pin 9   -> Left Arm
	//Vibro 1 -> Pin 10   -> Right Arm
	//Vibro 2 -> Pin 11   -> Left Leg
	//Vibro 3 -> Pin 6	  -> Right Leg
	public short portNumber;
	public int baudRate;
	enum Vib
	{
		LArm = 0, RArm, LLeg, RLeg
	};
	private SerialPort arduinoPort;

	void Start()
	{
		arduinoPort = new SerialPort("COM" + portNumber, baudRate);
		arduinoPort.Open();
	}

	void Update()
	{
		//enable for testing purposes
		//testing();

	}

	#region game vibration patterns
	//Activate when player aims flashlight at monster
	public void steadyRiseArms(int maxStrength, int timeToMax) {
		byte[] a = new byte[2];
		a[0] = (byte)Vib.LArm;
		a[1] = (byte)Vib.RArm;
		StartCoroutine(steadyRise(a, maxStrength, timeToMax));
	}

	public void shiftVibrationsLegs(int maxStrength)
	{
		int start = Random.Range(0,2);
		Debug.Log("ra: " + start);
		byte fromM = (byte) (start+2);
		byte toM = (byte)(((start+1) % 2) + 2);
		Debug.Log("From: " + fromM);
		Debug.Log("to" + toM);
		StartCoroutine(shiftVibrations(fromM, toM, 150));
	}


	public void shortFuzzLoopLegs(byte maxStrength, byte duration, int loops, int intervallMS)
	{
		byte[] a = new byte[2];
		a[0] = (byte)Vib.LLeg;
		a[1] = (byte)Vib.RLeg;
		StartCoroutine(shortFuzzLoop(a, maxStrength, duration, loops, intervallMS));
	}


	public void shortFuzzLoopArms(byte maxStrength, byte duration, int loops, int intervallMS)
	{
		byte[] a = new byte[2];
		a[0] = (byte)Vib.LArm;
		a[1] = (byte)Vib.RArm;
		StartCoroutine(shortFuzzLoop(a, maxStrength, duration, loops, intervallMS));
	}

	public void shortFuzzLoopAll(byte maxStrength, byte duration, int loops, int intervallMS)
	{
		byte[] a = new byte[4];
		a[0] = (byte)Vib.LArm;
		a[1] = (byte)Vib.RArm;
		a[2] = (byte)Vib.LLeg;
		a[3] = (byte)Vib.RLeg;
		StartCoroutine(shortFuzzLoop(a, maxStrength, duration, loops, intervallMS));
	}

	public void hitAlert(byte vibId)
	{
		writeToArduino(vibId, 100, 20);
	}
	#endregion
	#region arduino logic
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

	public void writeToArduino(byte vibro, byte intensity, byte duration)
	{
		byte[] inputToArduino = new byte[] { vibro, intensity, duration };
		if (arduinoPort.IsOpen == false)
		{
			arduinoPort.Open();
		}
		arduinoPort.Write(inputToArduino, 0, 3);
	}
	#endregion
	#region general vibration patterns
	public IEnumerator shortFuzzLoop(byte[] sensors, byte maxStrength, byte duration, int loops, int intervallMS)
	{
		for (int i = 0; i < loops; i++)
		{
			for (int j = 0; j < sensors.Length; j++)
			{
				writeToArduino(sensors[j], maxStrength, duration);
			}
			yield return new WaitForSeconds(intervallMS / 1000f);
		}
	}

	public IEnumerator steadyRise(byte[] sensors, int maxStrength, int timeToMax)
	{
		int step = 5;
		float time = timeToMax / (maxStrength / step);
		for (int i = 5; i < maxStrength && i > 0; i = i + step)
		{
			for (int j = 0; j < sensors.Length; j++)
			{
				writeToArduino(sensors[j], (byte)i, 100);
			}
			yield return new WaitForSeconds(time / 1000f);
			if (i >= maxStrength * 0.75f && step == 5)
			{
				step = step + 10;
			}

			if (i >= maxStrength - step)
			{
				step = -step * 3;

			}
		}
	}

	public IEnumerator shiftVibrations(byte fromModule, byte toModule, int maxStrength)
	{
		byte activeModule = fromModule;
		int currentStrength = 5;
		for (int i = 5; i < maxStrength && i > 0; i = i + currentStrength)
		{
			writeToArduino(activeModule, (byte)i, 20);

			yield return new WaitForSeconds(10 / 1000f);
			if (i >= maxStrength / 1.7)
			{
				activeModule = toModule;
			}
			if (i == maxStrength - 5)
			{
				currentStrength = -currentStrength;
			}
		}
	}

	#endregion

	private void testing()
	{
		if (Input.GetKeyDown(KeyCode.Q))
		{
			steadyRiseArms(200, 2000);
		}
		if (Input.GetKeyDown(KeyCode.E))
		{
			shiftVibrationsLegs(150);
		}
		if(Input.GetKeyDown(KeyCode.R))
		{
			shortFuzzLoopLegs(200, 10, 4, 150);
		}
		if (Input.GetKeyDown(KeyCode.T))
		{
			shortFuzzLoopArms(200, 10, 4, 150);
		}
		if (Input.GetKeyDown(KeyCode.Z))
		{
			shortFuzzLoopAll(200, 10, 4, 150);
		}

		// print(arduinoPort.ReadLine());
		//if (Input.GetKeyDown(KeyCode.Q))
		//{
		//    // writeToArduino(0, 100, 100);
		//    byte[] a = new byte[2];
		//    a[0] = 0;
		//    a[1] = 1;
		//    StartCoroutine(shortFuzzLoop(a, 200, 10, 4, 150));
		//}
		//if (Input.GetKeyDown(KeyCode.A))
		//{
		//    // writeToArduino(0, 100, 100);
		//    byte[] a = new byte[2];
		//    a[0] = 0;
		//    a[1] = 1;
		//    StartCoroutine(steadyRise(a, 200, 2000));
		//}

		//if (Input.GetKeyDown(KeyCode.W))
		//{
		//    StartCoroutine(shiftVibrations((byte)Vib.LArm, (byte)Vib.RArm, 150));
		//    //writeToArduino(1, 100, 100);
		//}
		//if (Input.GetKeyDown(KeyCode.S))
		//{
		//    writeToArduino((byte)Vib.LArm, 200, 100);
		//}

		//if (Input.GetKeyDown(KeyCode.E))
		//{
		//    writeToArduino(2, 100, 100);
		//}
		//if (Input.GetKeyDown(KeyCode.D))
		//{
		//    writeToArduino(2, 200, 100);
		//}
	}
}
