using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;
using UnityEngine.UI;

public class Flashlight : MonoBehaviour
{
    private float currentLightTimer = 0f;
    private Light[] lights;

    private GameObject oldHit;

    private bool lightsOn = true;
    private float currentPower;
    public float maxPower = 100f;
    public Slider slider;

	public short portNumber;
	public int baudRate;

	private float degree;

    private bool fingerTriggered;
    public float GloveThreashhold = 10f;
    public ActivateVibModules vib;
    private bool scaredActivated = false;

    private Enemy en;

    private GameMaster gameMaster;

	private SerialPort arduinoPort;

    void Start ()
    {
        lights = GetComponentsInChildren<Light>();

        currentPower = maxPower;

        gameMaster = FindObjectOfType<GameMaster>();

		arduinoPort = new SerialPort("COM" + portNumber, baudRate);
		arduinoPort.Open();
		arduinoPort.ReadTimeout = 12;
	}

	void Update ()
    {
        degree = (float) System.Convert.ToDouble(arduinoPort.ReadLine());
        

        //Debug.Log(currentPower);

		//Debug.Log(degree.ToString());

        if (!fingerTriggered)
        {
            if (degree <= GloveThreashhold)
            {
                fingerTriggered = true;
            }

            if (fingerTriggered)
            {
                LampTrigger();
            }
        }
        else if(fingerTriggered)
        {
            if (degree > GloveThreashhold)
            {
                fingerTriggered = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            LampTrigger();
        }

        HandleLampCharge();

        if (lightsOn)
        {
            RaycastHit hit;
            if (Physics.SphereCast(transform.position, 1, transform.forward, out hit, 100))
            {
                Debug.Log("Hit: " + hit.collider.gameObject);
                Enemy enemy = hit.collider.gameObject.GetComponent<Enemy>();
                en = enemy;
                if (enemy != null)
                {
                    enemy.isLitOn = true;   // enemy is hit by light, tell it
                    if (oldHit != enemy)
                    {
                        enemy.StartShaking();
                    }
                    if(scaredActivated == false)
                    {
                        scaredActivated = true;
                        vib.steadyRiseArms(200, (int) enemy.ScareTime * 1000);
                      
                    }
                    currentLightTimer += Time.deltaTime;
                    if (currentLightTimer >= enemy.ScareTime)
                    {
                        LampOff();
                        scaredActivated = false;
                        enemy.ScareAway();
                        currentLightTimer = 0f;
                    }
                }
                else
                {
                    scaredActivated = false;
                    currentLightTimer = 0f;
                    if (en != null)
                    {
                        // enemy is not hit by light, tell it
                        en.isLitOn = false;
                    }
                }

                oldHit = hit.collider.gameObject;
            }
            else
            {
                scaredActivated = false;
                currentLightTimer = 0f;
                oldHit = null;

                if (en != null)
                {
                    // enemy is not hit by light, tell it
                    en.isLitOn = false;
                }
            }
        }
    }


    public void LampTrigger()
    {
        if (gameMaster != null
            && gameMaster.gameIsRunning)
        {
            if (lightsOn)
            {
                LampOff();
            }
            else
            {
                LampOn();
            }
        }
    }

    public void LampOn()
    {
        if (currentPower <= 0)
        {
            return;
        }

        lightsOn = true;

        if (lights != null)
        {
            foreach (Light l in lights)
            {
                l.gameObject.SetActive(true);
            }
        }
    }

    public void LampOff()
    {
        lightsOn = false;

        if (lights != null)
        {
            foreach (Light l in lights)
            {
                l.gameObject.SetActive(false);
            }
        }
    }

    public void HandleLampCharge()
    {
        // lamp loses charge 
        if (lightsOn &&
            currentPower > 0)
        {
            currentPower -= 0.2f;

            if (currentPower <= 0)
            {
                currentPower = 0;
                LampOff();
            }
        }

        // lamp re-charging
        if (!lightsOn
            && currentPower < maxPower)
        {
            currentPower += 0.2f;

            if (currentPower >= maxPower)
            {
                currentPower = maxPower;
            }
        }

        if (slider != null)
        {
            slider.value = currentPower;
        }
    }
}
