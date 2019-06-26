using System.Collections;
using System.Collections.Generic;
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

    void Start ()
    {
        lights = GetComponentsInChildren<Light>();

        currentPower = maxPower;
	}
	
	void Update ()
    {
        Debug.Log(currentPower);

        if (Input.GetKeyDown(KeyCode.F))
        {
            LampTrigger();
        }

        HandleLampCharge();


        RaycastHit hit;
        if (Physics.SphereCast(transform.position, 1, transform.forward, out hit, 100))
        {
            Enemy enemy = hit.collider.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                if (oldHit != enemy)
                {
                    enemy.StartShaking();
                }

                currentLightTimer += Time.deltaTime;
                if (currentLightTimer >= enemy.ScareTime)
                {
                    enemy.ScareAway();
                    currentLightTimer = 0f;
                }
            }
            else
            {
                currentLightTimer = 0f;
            }

            oldHit = hit.collider.gameObject;
        }
        else
        {
            currentLightTimer = 0f;
            oldHit = null;
        }
    }


    public void LampTrigger()
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
