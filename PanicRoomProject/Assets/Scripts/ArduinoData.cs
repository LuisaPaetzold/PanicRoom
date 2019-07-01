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

    public float difficultyMultiplier = 0;

    internal int newBpm = 0;
    internal int newBeatInterval = 0;
    internal int newLatestSample = 0;

    bool firstFrame = true;

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

        newBpm = int.Parse(pulseStr.Split(sep)[0]);
        newBeatInterval = int.Parse(pulseStr.Split(sep)[1]);
        newLatestSample = int.Parse(pulseStr.Split(sep)[2]);

        if (firstFrame)
        {
            bpm = newBpm;
            beatInterval = newBeatInterval;
            latestSample = newLatestSample;
            firstFrame = false;
        }
        else
        {


            if (newBpm > 54 && newBpm < 150)
            {
                bpm = newBpm;
            }
            if (newBeatInterval > 600 && newBeatInterval < 1200)
            {
                beatInterval = newBeatInterval;
            }
            if (Mathf.Abs(latestSample - newLatestSample) < 10000)
            {
                latestSample = newLatestSample;
            }
        }
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
        float awayFromLowerBound = bpm - 55f;
        difficultyMultiplier = awayFromLowerBound / (150f - 55f);
        //Debug.Log(scaled);
    }
}