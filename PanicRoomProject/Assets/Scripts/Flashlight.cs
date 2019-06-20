using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    private float currentLightTimer = 0f;

	void Start ()
    {
		
	}
	
	void Update ()
    {
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, 1, transform.forward, out hit, 100))
        {
            Enemy enemy = hit.collider.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
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
        }
        else
        {
            currentLightTimer = 0f;
        }
    }
}
