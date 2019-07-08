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
        try
        {
            pulseSerial.Open();
        }
        catch
        {

        }
        bpm = 65;
        beatInterval = 900;
        latestSample = 0;
        StartCoroutine(PulseCoroutine());
    }
    private void Update()
    {
        if (pulseSerial != null && pulseSerial.IsOpen)
        {
            getPulseData();
            CalculateDifficultyTime();
        }
        else
        {
            CalculateDifficultyTime();
        }
    }

    public void getPulseData()
    {
        string pulseStr = pulseSerial.ReadLine();
        char sep = ',';
        Debug.Log(pulseStr);

        string[] a = pulseStr.Split(sep);
        if (a.Length < 3)
        {
            return;
        }

        newBpm = int.Parse(a[0]);
        newBeatInterval = int.Parse(a[1]);
        newLatestSample = int.Parse(a[2]);



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

        if (newBpm == 0)
        {
            bpm = 65;
            beatInterval = 900;
            latestSample = 0;
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

    public void CalculateDifficultyTime()
    {
        float awayFromLowerBound = bpm - 55f;
        difficultyMultiplier = awayFromLowerBound / (150f - 55f);
        //Debug.Log(scaled);
    }

    public void ClosePort()
    {
        //pulseSerial.Close();
    }
}