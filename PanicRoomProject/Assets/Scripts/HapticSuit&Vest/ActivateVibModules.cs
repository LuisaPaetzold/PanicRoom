using UnityEngine;
using System.Collections;
using System.IO.Ports;
using System.Threading;

public class ActivateVibModules : MonoBehaviour
{
	//if serial & port cannot be found go to edit -> project settings -> player and change api compability level to .NET 2.0
    // Start is called before the first frame update
    public short portNumber;
    public int baudRate;

    private SerialPort arduinoPort;

    void Start()
    {
        arduinoPort = new SerialPort("COM" + portNumber, baudRate);
        arduinoPort.Open();
    }

    // Update is called once per frame
    void Update()
    {
        // print(arduinoPort.ReadLine());
        if (Input.GetKeyDown(KeyCode.Q))
        {
            // writeToArduino(0, 100, 100);
            byte[] a = new byte[2];
            a[0] = 0;
            a[1] = 1;
           StartCoroutine( shortFuzzLoop(a, 200, 10, 4, 150));
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
			// writeToArduino(0, 100, 100);
			byte[] a = new byte[2];
			a[0] = 0;
			a[1] = 1;
			StartCoroutine(steadyRise(a, 200, 2000));
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
			StartCoroutine(shiftVibrations(0, 1, 150));
            //writeToArduino(1, 100, 100);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            writeToArduino(1, 200, 100);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            writeToArduino(2, 100, 100);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            writeToArduino(2, 200, 100);
        }
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

    void writeToArduino(byte vibro, byte intensity, byte duration)
    {
        byte[] inputToArduino = new byte[] { vibro, intensity, duration };
        if (arduinoPort.IsOpen == false)
        {
            arduinoPort.Open();
        }
        arduinoPort.Write(inputToArduino, 0, 3);
    }

    IEnumerator shortFuzzLoop(byte[] sensors, byte maxStrength,byte duration, int loops, int intervallMS)
    {
        for(int i = 0; i < loops; i++)
        {
            for(int j=0; j< sensors.Length; j++)
            {
                writeToArduino(sensors[j],maxStrength, duration);
            }
            yield return new WaitForSeconds(intervallMS/1000f);
        }
    }

    IEnumerator steadyRise(byte[] sensors, int maxStrength, int timeToMax)
    {
        int currentStrength = 5;
        int timeStep = timeToMax / (maxStrength - 5) / 5;
        for (int i = 5; i < maxStrength && i>0; i=i+currentStrength)
        {
            for (int j = 0; j < sensors.Length; j++)
            {
                
                writeToArduino(sensors[j], (byte)i, 100);
            }
            yield return new WaitForSeconds(25/1000f);
            if (i == maxStrength - 5)
            {
                currentStrength = -currentStrength*3;

            }
        }
    }

	IEnumerator shiftVibrations(byte fromModule, byte toModule, int maxStrength){
		byte activeModule = fromModule;
		int currentStrength = 5;
		for (int i = 5; i < maxStrength && i>0; i=i+currentStrength)
		{
				writeToArduino(activeModule, (byte)i, 20);

			yield return new WaitForSeconds(10/1000f);
			if (i >= maxStrength/1.5)
			{
				activeModule = toModule;
			}
			if (i == maxStrength - 5)
			{
				currentStrength = -currentStrength;
			}
		}
	}	

}
