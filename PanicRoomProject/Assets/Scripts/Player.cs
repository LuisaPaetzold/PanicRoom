using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class Player : MonoBehaviour
{
    public VRTK_ControllerEvents rightController;

    void Start ()
    {

    }
	

	void Update ()
    {
        if (rightController != null)
        {
            if (rightController.IsButtonPressed(VRTK_ControllerEvents.ButtonAlias.TriggerClick))
            {
                Debug.Log("TRIGGER"); 
            }
        }
    }
}
