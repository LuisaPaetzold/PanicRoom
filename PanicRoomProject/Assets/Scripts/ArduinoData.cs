using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class ArduinoData : MonoBehaviour
{
    SerialPort pulseSerial;
    public string pulseCOM = "COM3";
    public int pulseBaudRate = 115200;

    public int bpm = 0;
    public int beatInterval = 0;
    public int latestSample = 0;

    public float waitForNextBeat = 0;
    internal AudioSource audioSource;
    public AudioClip heartBeat;

    private void Start()
    {
        pulseSerial = new SerialPort(pulseCOM, pulseBaudRate);
        audioSource = GetComponent<AudioSource>();
        pulseSerial.Open();
        StartCoroutine(PulseCoroutine());
    }
    private void Update()
    {
        getPulseData();
        CalculateWaitingTime();

    }

    public void getPulseData()
    {
        string pulseStr = pulseSerial.ReadLine();
        char sep = ',';
        //Debug.Log(pulseStr);
        bpm = int.Parse(pulseStr.Split(sep)[0]);
        beatInterval = int.Parse(pulseStr.Split(sep)[1]);
        latestSample = int.Parse(pulseStr.Split(sep)[2]);
    }

    public IEnumerator PulseCoroutine()
    {
        while (true)
        {

        audioSource.PlayOneShot(heartBeat);
        yield return new WaitForSeconds(beatInterval / 1000f);
        }
    }

    public void CalculateWaitingTime()
    {
    }
}